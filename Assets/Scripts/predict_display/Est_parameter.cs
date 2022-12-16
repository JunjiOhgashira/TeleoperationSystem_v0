

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Random = UnityEngine.Random;

namespace Predictive_calc
{
    public class Est_parameter : MonoBehaviour
    {
        public Est_velocity est_velocity;

        public Predict_param predict_param;
        public Vehicle_param vehicle_param;

        public Udp_com.UdpReceiver udpreceiver;
        public Other_param other_param;

        private bool compen_mode;
        System.Diagnostics.Stopwatch sw;


        private double timeOut;
        private double K_pdl;  //アクセルゲイン
        private double K_brk; // ブレーキゲイン
        private double dead_band; //不感帯幅
        private double gear_rasio; //ステアリング指令値との比率
        private double W; //ホイールベース
        private double T; //トレッド
        private double acc_max; //最高速度m/s
        private double acc_min;
        private double steer_max;
        private double steer_min;
        private double lf; //重心から前輪までの長さm
        private double lr; //重心から後輪までの長さm
        private int BUF_NUMBER;
        private double center_side;  //車両幅の半分 m
        private double center_long; //車両長さの半分 m
        private double center_up; //車両高さ
        private double traj_length; //軌跡ながさ
        private double traj_res; //軌跡の細かさ
        private double vehicle_pos; //車両モデルの位置

        //各種変数定義
        internal double handle, accel_pedal, brake_pedal, all_time,down_time;
        internal double handle_rev, accelpedal_rev, brakepedal_rev, diam;
        private double acceleration;
        private double filter_steer;
        private double kinematic_steer;
        private double slip, yawrate;
        internal double velocity = 0.0;
        private double last_steering = 0.0;
        private double prepoint_z = 0;
        private double prepoint_x = 0;
        private double sum_yaw = 0;
        internal int count;
        internal int send_count=0;
        private double delta = 0;
        //private const double a= 0.000349270758023033;
        private const double a = 0.000349270758023033*0.96;


        private double dec_NUMBER;
        internal int NUMBER;

        private double[] past_velocity;
        private double[] past_yawrate;
        private double[] past_slip;
        private double[] use_past_velocity;
        private double[] use_past_yawrate;
        private double[] use_past_slip;
        private double[] dis;
        private double[] yaw_angle;
        internal double[] sum_yaw_angle;
        internal double[] all_prepoint_x;
        internal double[] all_prepoint_z;
        internal double[] all_yaw_angle;

        internal double[] send_prepoint_x;
        internal double[] send_prepoint_z;
        internal double[] send_yaw_angle;

        internal double model_prepoint_x;
        internal double model_prepoint_z;
        internal double model_yaw_angle;

        internal Vector3 v_up_right;
        internal Vector3 v_up_left;
        internal Vector3 v_down_left;
        internal Vector3 v_down_right;


        internal double real_acc = 0;
        internal double real_vel = 0;
        internal double real_vel1 = 0;
        internal double yaw_rate = 0;

        private int check_mode = 0;
        private long time_ts = 0;
        private bool est_vel_mode = false;

        private double slv_test_time = 25;
 


        //初期化処理
        private void Awake()
        {
            timeOut = predict_param.timeOut;
            K_pdl = predict_param.K_pdl;
            K_brk = predict_param.K_brk;
            dead_band = predict_param.dead_band;
            gear_rasio = vehicle_param.gear_rasio;
            W = vehicle_param.W;
            T = vehicle_param.T;
            acc_max = predict_param.acc_max;
            acc_min = predict_param.acc_min;
            steer_max = predict_param.steer_max;
            steer_min = predict_param.steer_min;
            lf = vehicle_param.lf;
            lr = vehicle_param.lr;
            BUF_NUMBER = predict_param.BUF_NUMBER;
            center_side = vehicle_param.center_side;
            center_long = vehicle_param.center_long;
            center_up = vehicle_param.center_up;
            traj_length = predict_param.traj_length;
            traj_res = predict_param.traj_res;
            vehicle_pos = predict_param.vehicle_pos;
            past_velocity = new double[BUF_NUMBER + 1];
            past_yawrate = new double[BUF_NUMBER + 1];
            past_slip = new double[BUF_NUMBER + 1];
            use_past_velocity = new double[BUF_NUMBER + 1];
            use_past_yawrate = new double[BUF_NUMBER + 1];
            use_past_slip = new double[BUF_NUMBER + 1];
            dis = new double[BUF_NUMBER + 1];
            yaw_angle = new double[BUF_NUMBER + 1];
            sum_yaw_angle = new double[BUF_NUMBER + 1];
            all_prepoint_x = new double[BUF_NUMBER + 1];
            all_prepoint_z = new double[BUF_NUMBER + 1];
            all_yaw_angle = new double[BUF_NUMBER + 1];

            send_prepoint_x = new double[BUF_NUMBER + 1];
            send_prepoint_z = new double[BUF_NUMBER + 1];
            send_yaw_angle = new double[BUF_NUMBER + 1];

            est_vel_mode = other_param.Est_vel_mode;
        }

        public void command_receiver()
        {
            handle = udpreceiver.steering;
            accel_pedal = udpreceiver.accel_pedal;
            brake_pedal = udpreceiver.brake_pedal;
            all_time = udpreceiver.RTT + udpreceiver.slvin_time + slv_test_time;
            down_time = slv_test_time + udpreceiver.RTT;
            real_vel1 = udpreceiver.real_vel;
            yaw_rate = udpreceiver.yaw_rate;
        }
        //指令値から滑り角、よーレート、速度、加速度などを計算
        public void command_proc()
        {
            handle_rev = Math.Round(handle * 0.1, 7, MidpointRounding.AwayFromZero);
            accelpedal_rev = Math.Round(accel_pedal / 100.0, 7, MidpointRounding.AwayFromZero);
            brakepedal_rev = Math.Round(brake_pedal / 100.0, 7, MidpointRounding.AwayFromZero);

            if (dead_band > brakepedal_rev)
            {
                brakepedal_rev = 0.0;
            }

            //ペダル入力量から指示加速度に変換
            est_velocity.constructor();
            est_velocity.real_vel_proc();
            est_velocity.dir_acc_calc();

            //アクセルペダルを踏んだ時の速度の推定
            if(accelpedal_rev > 0)
            {
                
                if(real_vel == 0 && check_mode !=1)
                {
                    
                    sw = new System.Diagnostics.Stopwatch();
                    sw.Start();
                }else if(real_vel != 0)
                {
                    time_ts = 500;
                }

                if (check_mode != 1)
                {
                    check_mode = 1;
                }

                if(real_vel == 0)
                {
                    velocity = est_velocity.est_plus_vel(sw.ElapsedMilliseconds);
                }
                else
                {
                    velocity = est_velocity.est_plus_vel(time_ts);
                }
                    

            }
            //ブレーキペダルを踏んだ時の速度の推定
            else if(brakepedal_rev > 0)
            {
                if (check_mode != 2)
                {
                    check_mode = 2;
                }
                velocity = est_velocity.est_minus_vel();
            }
            //ペダルを踏まなかったときの速度の推定
            else
            {
                if (check_mode != 3)
                {
                    check_mode = 3;
                }

                velocity = est_velocity.est_inertia();
            }

            //ステア角から横滑り角、ヨー角などの計算
            delta = Math.Asin(a*W*handle_rev);
            slip = Math.Atan(lr * Math.Tan(delta) / (lf + lr));  //横滑り角
            yawrate = velocity * Math.Sin(slip) / lr;
        }



        //遅延データからの計算
        public void delay_proc()
        {
            double RTT_time = all_time * Math.Pow(10,-3);
            double tmp_NUMBER = RTT_time / timeOut;
            tmp_NUMBER = Math.Round(tmp_NUMBER, 1, MidpointRounding.AwayFromZero);  //少数第2位で四捨五入
            int integer_NUMBER = (int)tmp_NUMBER; //整数点
            dec_NUMBER = tmp_NUMBER - integer_NUMBER;  //少数点
            int past_NUMBER = NUMBER;
            NUMBER = integer_NUMBER;

        }




        //予測表示の座標を計算
        public void calc_predict()
        {
            compen_mode = other_param.compen_mode;

            int i = 0, j = 0, k = 0, l = 0;
            //最新の予測表示座標の初期化
            double sum_prepoint_x = 0;
            double sum_prepoint_z = 0;
            double sum = 0;

            model_prepoint_x = 0;
            model_prepoint_z = 0;
            model_yaw_angle = 0;
            bool model_count = false;

            //すべての予測表示の初期化
            all_prepoint_x = Enumerable.Repeat<double>(0, BUF_NUMBER + 1).ToArray();
            all_prepoint_z = Enumerable.Repeat<double>(0, BUF_NUMBER + 1).ToArray();
            all_yaw_angle = Enumerable.Repeat<double>(0, BUF_NUMBER + 1).ToArray();

            //過去の履歴データを更新
            for (i = 0; i < BUF_NUMBER; i++)
            {
                past_velocity[i] = past_velocity[i + 1];
                past_yawrate[i] = past_yawrate[i + 1];
                past_slip[i] = past_slip[i + 1];
            }

            past_velocity[BUF_NUMBER] = velocity;
            past_slip[BUF_NUMBER] = slip;
            past_yawrate[BUF_NUMBER] = yawrate;

            if (NUMBER == 0) { }
            //履歴から使用する入力を選択
            else
            {
                Array.Copy(past_velocity, BUF_NUMBER - NUMBER - 1, use_past_velocity, 0, NUMBER + 1);
                Array.Copy(past_slip, BUF_NUMBER - NUMBER - 1, use_past_slip, 0, NUMBER + 1);
                Array.Copy(past_yawrate, BUF_NUMBER - NUMBER - 1, use_past_yawrate, 0, NUMBER + 1);
            }

            //速度が0じゃないときの予測座標の計算
            if (velocity != 0)
            {
                for (k = 0; k < NUMBER + 1; k++)
                {
                    dis[k] = (use_past_velocity[k] + use_past_velocity[k + 1]) * timeOut * 0.5;
                }

                sum = 0;
                for (k = 0; k < NUMBER + 1; k++)
                {
                    yaw_angle[k] = (use_past_yawrate[k] + use_past_yawrate[k + 1]) * timeOut * 0.5;
                    sum += yaw_angle[k];
                    sum_yaw_angle[k] = sum;
                    all_yaw_angle[k] = sum_yaw_angle[k];
                }


                for (j = 0; j < NUMBER; j++)
                {
                    sum_prepoint_z += dis[j] * Math.Cos(sum_yaw_angle[j] + use_past_slip[j]);//ラジアン表記
                    sum_prepoint_x += dis[j] * Math.Sin(sum_yaw_angle[j] + use_past_slip[j]);//ラジアン表記
                    all_prepoint_z[j] = sum_prepoint_z;
                    all_prepoint_x[j] = sum_prepoint_x;
                }

                //小数部分の位置を推定
                sum_prepoint_z += dec_NUMBER * dis[NUMBER] * Math.Cos(sum_yaw_angle[NUMBER] + use_past_slip[NUMBER]);              //ラジアン表記
                sum_prepoint_x += dec_NUMBER * dis[NUMBER] * Math.Sin(sum_yaw_angle[NUMBER] + use_past_slip[NUMBER]);              //ラジアン表記
                all_prepoint_z[NUMBER] = sum_prepoint_z;
                all_prepoint_x[NUMBER] = sum_prepoint_x;

                prepoint_z = all_prepoint_z[NUMBER];
                prepoint_x = all_prepoint_x[NUMBER];
                sum_yaw = sum_yaw_angle[NUMBER];
                //軌跡延長分の座標計算
                if(compen_mode == true)
                {
                    for (l = 1; Math.Sqrt((sum_prepoint_x * sum_prepoint_x) + (sum_prepoint_z * sum_prepoint_z)) < traj_length; l++)  //変更点
                    {

                        sum += traj_res * Math.Sin(slip) / lr;  //変更点f
                                                                //sum += yaw_angle[NUMBER];
                        
                        sum_prepoint_z += traj_res * Math.Cos(sum_yaw_angle[NUMBER + l-1] + slip);  //変更点
                        sum_prepoint_x += traj_res * Math.Sin(sum_yaw_angle[NUMBER + l-1] + slip);   //変更点
                        sum_yaw_angle[NUMBER + l] = sum;
                        all_prepoint_z[NUMBER + l] = sum_prepoint_z;
                        all_prepoint_x[NUMBER + l] = sum_prepoint_x;
                        all_yaw_angle[NUMBER + l] = sum_yaw_angle[NUMBER + l];

                        if (Math.Sqrt((sum_prepoint_x * sum_prepoint_x) + (sum_prepoint_z * sum_prepoint_z)) > vehicle_pos && model_count == false)
                        {
                            model_prepoint_x = sum_prepoint_x;
                            model_prepoint_z = sum_prepoint_z;
                            model_yaw_angle = sum_yaw_angle[NUMBER + l];
                            model_count = true;
                        }

                    }
                }
                
            }
            //速度が0のときの予測座標の計算
            else
            {
                if(compen_mode == true)
                {
                    for (l = 1; Math.Sqrt((sum_prepoint_x * sum_prepoint_x) + (sum_prepoint_z * sum_prepoint_z)) < traj_length; l++)  //変更点
                    {

                        sum += traj_res * Math.Sin(slip) / lr;   //変更点
                        
                        sum_prepoint_z += traj_res * Math.Cos(sum_yaw_angle[l-1] + slip);  //変更点
                        sum_prepoint_x += traj_res * Math.Sin(sum_yaw_angle[l-1] + slip); //変更点
                        sum_yaw_angle[l] = sum;
                        all_prepoint_z[l] = sum_prepoint_z;
                        all_prepoint_x[l] = sum_prepoint_x;
                        all_yaw_angle[l] = sum_yaw_angle[l];

                        if (Math.Sqrt((sum_prepoint_x * sum_prepoint_x) + (sum_prepoint_z * sum_prepoint_z)) > vehicle_pos && model_count == false)
                        {
                            model_prepoint_x = sum_prepoint_x;
                            model_prepoint_z = sum_prepoint_z;
                            model_yaw_angle = sum_yaw_angle[l];
                            model_count = true;
                        }
                    }
                }
                
                all_prepoint_z[l] = 0;
                all_prepoint_x[l] = 0;
                all_yaw_angle[l] = 0;
                prepoint_z = 0;
                prepoint_x = 0;
                sum_yaw = 0;

            }




            count = 0;

            for (j = 0; j < BUF_NUMBER; j++)
            {
                if (all_prepoint_z[j] != '\0')
                {
                    ++count; //配列の数をカウント
                }
                else { }
            }


            //車両概形の座標を計算
            double[] up_right = new double[3] { prepoint_x + center_side * Math.Cos(sum_yaw) + center_long * Math.Sin(sum_yaw), center_up, prepoint_z + center_long * Math.Cos(sum_yaw) - center_side * Math.Sin(sum_yaw) };
            double[] up_left = new double[3] { prepoint_x - (center_side * Math.Cos(sum_yaw) - center_long * Math.Sin(sum_yaw)), center_up, prepoint_z + center_long * Math.Cos(sum_yaw) + center_side * Math.Sin(sum_yaw) };
            double[] down_right = new double[3] { prepoint_x + center_side * Math.Cos(sum_yaw) + center_long * Math.Sin(sum_yaw), 0, prepoint_z + center_long * Math.Cos(sum_yaw) - center_side * Math.Sin(sum_yaw) };
            double[] down_left = new double[3] { prepoint_x - (center_side * Math.Cos(sum_yaw) - center_long * Math.Sin(sum_yaw)), 0, prepoint_z + center_long * Math.Cos(sum_yaw) + center_side * Math.Sin(sum_yaw) };

            v_up_right = new Vector3((float)up_right[0], (float)up_right[1], (float)up_right[2]);
            v_up_left = new Vector3((float)up_left[0], (float)up_left[1], (float)up_left[2]);
            v_down_left = new Vector3((float)down_left[0], (float)down_left[1], (float)down_left[2]);
            v_down_right = new Vector3((float)down_right[0], (float)down_right[1], (float)down_right[2]);


            Array.Copy(all_prepoint_x, send_prepoint_x, all_prepoint_x.Length);
            Array.Copy(all_prepoint_z, send_prepoint_z, all_prepoint_z.Length);
            Array.Copy(all_yaw_angle, send_yaw_angle, all_yaw_angle.Length);
            send_count = count;


            
        }
    }
}

