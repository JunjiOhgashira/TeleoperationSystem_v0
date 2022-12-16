using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Predictive_display
{
    //車両後方右の進路軌跡を描画するクラス
    public class Back_right_traj : MonoBehaviour
    {

        public Predictive_calc.Est_parameter est_parameter;
        public Predict_param predict_param;
        public Vehicle_param vehicle_param;

        private int BUF_NUMBER;  //最大計算保持点数
        private double[] all_prepoint_x;
        private double[] all_prepoint_z;
        private double[] all_yaw_angle;
        private int count;
        private double center_side;  //車両幅の半分 mm
        private double center_long; //車両長さの半分 mm

        LineRenderer line;

        // Start is called before the first frame update
        void Start()
        {
            line = GetComponent<LineRenderer>();
            line.material = new Material(Shader.Find("Sprites/Default"));

            BUF_NUMBER = predict_param.BUF_NUMBER;
            center_side = vehicle_param.center_side;
            center_long = vehicle_param.center_long;

            all_prepoint_x = new double[BUF_NUMBER + 1];
            all_prepoint_z = new double[BUF_NUMBER + 1];
            all_yaw_angle = new double[BUF_NUMBER + 1];
        }

        // Update is called once per frame
        void Update()
        {
            //計算した重心の予測表示座標点を参照
            Array.Copy(est_parameter.send_prepoint_x, all_prepoint_x, est_parameter.send_prepoint_x.Length);
            Array.Copy(est_parameter.send_prepoint_z, all_prepoint_z, est_parameter.send_prepoint_z.Length);
            Array.Copy(est_parameter.send_yaw_angle, all_yaw_angle, est_parameter.send_yaw_angle.Length);
            count = est_parameter.send_count;

            double[,] pre_trj_right = new double[count, 3];
            Vector3 v_pre_trj_right = new Vector3();
            line.positionCount = count;

            //重心の予測表示座標点から車両後方右が描く進路軌跡を計算し仮想環境上に描画する
            for (int m = 0; m < count; m++)
            {
                pre_trj_right[m, 0] = all_prepoint_x[m] + center_side * Math.Cos(all_yaw_angle[m]) - center_long * Math.Sin(all_yaw_angle[m]);
                pre_trj_right[m, 1] = 0;
                pre_trj_right[m, 2] = all_prepoint_z[m] - center_long * Math.Cos(all_yaw_angle[m]) - center_side * Math.Sin(all_yaw_angle[m]);
                v_pre_trj_right.x = (float)pre_trj_right[m, 0];
                v_pre_trj_right.y = (float)pre_trj_right[m, 1];
                v_pre_trj_right.z = (float)pre_trj_right[m, 2];
                line.SetPosition(m, v_pre_trj_right);
            }
        }
    }
}

