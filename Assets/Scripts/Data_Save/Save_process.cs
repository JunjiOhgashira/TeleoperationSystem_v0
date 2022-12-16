using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

namespace Data_Save
{
    //Saveの実行クラス
    public class Save_process : MonoBehaviour
    {
        public Csv_param csv_param;
        public Predictive_calc.Est_parameter est_parameter;
        public Est_velocity est_velocity;
        public Udp_com.UdpReceiver udpreceiver;

        private string filename,fileName;
        private bool HMD_rot_mode,est_velocity_mode,steering_rot_mode;
        private Vector3 HMD_rotation;

        StreamWriter sw;
        FileInfo fi;

        Save_HMD_rot save_HMD_rot = new Save_HMD_rot();
        Save_est_velocity save_est_velocity = new Save_est_velocity();
        Save_est_Rudder_angle save_est_rudder_angle = new Save_est_Rudder_angle();

        // Start is called before the first frame update
        void Start()
        {
            filename = csv_param.fileName;
            HMD_rot_mode = csv_param.HMD_rot_mode;
            est_velocity_mode = csv_param.est_velocity_mode;
            steering_rot_mode = csv_param.steering_rot_mode;

            DateTime now = DateTime.Now;

            if(HMD_rot_mode == true)
            {
                save_HMD_rot.Filename = filename;
                save_HMD_rot.file_open();
            }
            if(est_velocity_mode == true)
            {
                save_est_velocity.Filename = filename;
                save_est_velocity.file_open();
            }
            if(steering_rot_mode == true)
            {
                save_est_rudder_angle.Filename = filename;
                save_est_rudder_angle.file_open();
            }
            


        }

        // Update is called once per frame
        void Update()
        {
            if (HMD_rot_mode == true)
            {
                save_HMD_rot.file_write();
            }
            if(est_velocity_mode == true)
            {
                save_est_velocity.Velocity = est_parameter.velocity;
                save_est_velocity.Real_vel = udpreceiver.real_vel;
                save_est_velocity.Real_vel1 = est_velocity.real_vel;
                save_est_velocity.file_write();
            }
            if(steering_rot_mode == true)
            {
                save_est_rudder_angle.Ru_angle = est_parameter.handle;
                save_est_rudder_angle.file_write();
            }
        }

        void OnApplicationQuit()
        {
            if(HMD_rot_mode == true)
            {
                save_HMD_rot.file_close();
            }
            if(est_velocity_mode == true)
            {
                save_est_velocity.file_close();
            }
            if(steering_rot_mode == true)
            {
                save_est_rudder_angle.file_close();
            }
        }
    }
}

