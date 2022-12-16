using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Predictive_display
{
    //車両の2Dモデルを描画するクラス（使ってない）
    public class vehicle_model : MonoBehaviour
    {

        public Predictive_calc.Est_parameter est_parameter;
        public Predict_param predict_param;
        public Vehicle_param vehicle_param;


        private double center_side;  //車両幅の半分 mm
        private double center_long; //車両長さの半分 mm
        private int BUF_NUMBER;  //最大計算保持点数

        private double model_prepoint_x;
        private double model_prepoint_z;
        private double model_yaw_angle;


        private Vector3 v_left_forward;
        private Vector3 v_right_forward;
        private Vector3 v_right_backward;
        private Vector3 v_left_backward;

        Vector3 pos;
        // Start is called before the first frame update
        void Start()
        {
            Vector3 pos = this.transform.position;

            BUF_NUMBER = predict_param.BUF_NUMBER;
            center_side = vehicle_param.center_side;
            center_long = vehicle_param.center_long;

        }

        // Update is called once per frame
        void Update()
        {
            //車両モデルの位置を参照し描画する
            model_prepoint_x = est_parameter.model_prepoint_x;
            model_prepoint_z = est_parameter.model_prepoint_z;
            model_yaw_angle = est_parameter.model_yaw_angle;

            pos.x = (float)model_prepoint_x;
            pos.z = (float)model_prepoint_z;

            this.transform.position = pos;

            Quaternion rot = Quaternion.Euler(0, (float)(model_yaw_angle * 180 / Math.PI), 0);
            this.transform.rotation = rot;
        }
    }
}

