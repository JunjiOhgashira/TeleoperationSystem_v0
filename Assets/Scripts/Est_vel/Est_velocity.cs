using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

//遅れている実速度からリアルタイムの推定速度を算出するための処理を記述したクラス
public class Est_velocity : MonoBehaviour
{
    public Predict_param predict_param;
    public Vehicle_param vehicle_param;
    public Predictive_calc.Est_parameter est_parameter;
    public Other_param other_param;

    private double thorttle_pedal = 0;
    private double brake_pedal = 0;
    internal double real_vel = 0;
    internal double real_vel1 = 0;
    internal double real_vel2 = 0;
    private double real_acc = 0;
    private int Number = 0;
    private double dv = 0;
    private double vel = 0;
    private double vel_pre = 0;

    private double K_pdl = 3.0;
    private double K_brk = 2.5;
    private double acc_max = 3;
    private double acc_min = -5.0;

    private double plus_acc = 0;
    private double minus_acc = 0;

    private double dir_acc = 0;


    private double ddy = 0;
    private double dy = 0;
    private double y = 0;
    private double y1 = 0;

    private double dy2 = 0;
    private double dy3 = 0;
    private double y2 = 0;
    private double y3 = 0;

    private double zeta = 3;
    private double w = 7;
    private double K = 0.16;
    private double K1 = 0.6;


    private double down_time = 0;
    private double sampling_time;
    private bool est_vel_mode = false;

    Queue<double> buffer_vel = new Queue<double>();
    Queue<long> buffer_time = new Queue<long>();
    Queue<double> queue_vel = new Queue<double>();
    Queue<double> queue_diracc = new Queue<double>();
    private int que_size = 20;
    private int que_accsize = 90;

    System.Diagnostics.Stopwatch sw;
    
    private void Awake()
    {
        
        K_pdl = predict_param.K_pdl;
        K_brk = predict_param.K_brk;
        acc_max = predict_param.acc_max;
        acc_min = predict_param.acc_min;
        sampling_time = predict_param.timeOut;
        est_vel_mode = other_param.Est_vel_mode;

        sw = new System.Diagnostics.Stopwatch();
        sw.Start();
    }

    private void OnApplicationQuit()
    {
        sw.Stop();
    }
        public void constructor()
    {
        thorttle_pedal = est_parameter.accelpedal_rev;
        brake_pedal = est_parameter.brakepedal_rev;
        Number = est_parameter.NUMBER;
    }

    public void constructor_pre()
    {
        down_time = est_parameter.down_time;
        real_vel1 = est_parameter.real_vel1;
    }

    //ペダル入力量から指示加速度の計算
    public void dir_acc_calc()
    {
        //Debug.Log(Time.time);
        plus_acc = K_pdl * thorttle_pedal;
        minus_acc = (K_brk * brake_pedal) * (-vel);
        dir_acc = plus_acc + minus_acc;

        dir_acc = Math.Min(dir_acc, acc_max);
        dir_acc = Math.Max(dir_acc, acc_min);

        if (que_accsize > queue_diracc.Count)
        {
            queue_diracc.Enqueue(dir_acc);
        }
        else if (que_accsize <= queue_diracc.Count)
        {
            double dast = queue_diracc.Dequeue();
            queue_diracc.Enqueue(dir_acc);
        }
    }

    //アクセルペダル踏んだ時の推定速度
    public double est_plus_vel(long time_ms)
    {

        double[] diracc_array = queue_diracc.ToArray();
        zeta = 3;
        w = 7;
        K = 0.183;
        double gain = 1;


        int rise_time = 500;
        double t = 0;

        ddy = 0;
        dy = 0;
        y1 = 0;

            y = real_acc;
            vel = real_vel;

        
        int i = 0;

        
            for (i = 0; i < Number; i++)
            {
                //速度0のときに発生する無駄時間
                if (t >= rise_time - time_ms)
                {

                    ddy = -2 * zeta * w * dy - w * w * y + w * w * (diracc_array[diracc_array.Length - 1 - i] - K);
                    dy = dy + ddy * sampling_time;
                    y = y + dy * sampling_time;
                }
                vel = vel + sampling_time * y;
                t = t + sampling_time * 1000;
            
                if (vel < 0)
                    vel = 0;
            }


        return vel;
    }

    //ブレーキペダルを踏んだ時の推定速度
    public double est_minus_vel()
    {
        double[] diracc_array = queue_diracc.ToArray();
        zeta = 0.8;
        w = 2;
        K = 0.62;
        K1 = 0.6;


        ddy = 0;
        dy = 0;

            y = real_acc;
            vel = real_vel;


        int i = 0;

            for (i = 0; i < Number; i++)
            {
                ddy = -2 * zeta * w * dy - w * w * y + K1 * w * w * (diracc_array[diracc_array.Length - 1 - i] - K);
                dy = dy + ddy * sampling_time;
                y = y + dy * sampling_time;

                vel = vel + sampling_time * y;

                if (vel < 0)
                    vel = 0;

            }

        
        return vel;
    }

    //ペダルを踏まなかったときの推定速度
    public double est_inertia()
    {

        zeta = 2.64;
        w = 0.3;
        double u = -2;
        dy = 0;

            y = real_acc;
            vel = real_vel;

        int i= 0;

            for(i = 0; i < Number; i++)
            {
                dy = -2 * zeta * w * y - w * w * vel + w * w * u;
                y = y + dy * sampling_time;
                vel = vel + y * sampling_time;

                if (vel < 0)
                    vel = 0;
            }


            return vel;
    }

    //受信データからの実速度への変換と加速度の計算
    public void real_vel_proc()
    {
        real_vel = real_vel2 * 0.0078125 * 0.2777778;
        if (que_size > queue_vel.Count)
        {
            queue_vel.Enqueue(real_vel);
        }else if (que_size <= queue_vel.Count)
        {
            double dast = queue_vel.Dequeue();
            queue_vel.Enqueue(real_vel);
        }

        double[] vel_array = queue_vel.ToArray();
        //double vel_sum = vel_array.Sum();
        real_acc = (vel_array[vel_array.Length - 1] - vel_array[0]) / (sampling_time * queue_vel.Count);
        //Debug.Log(real_vel2);
    }

    //速度バッファリング
    public void Buffer_vel()
    {
        buffer_vel.Enqueue(real_vel1);
        buffer_time.Enqueue(sw.ElapsedMilliseconds);
        //Debug.Log(sw.ElapsedMilliseconds * 0.001);
        if(down_time < sw.ElapsedMilliseconds - buffer_time.Peek())
        {
            real_vel2 = buffer_vel.Dequeue();
            long gabage = buffer_time.Dequeue();
        }
        else { }
    }
}
