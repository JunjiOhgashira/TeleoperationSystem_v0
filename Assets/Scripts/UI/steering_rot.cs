using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UI
{
    //ステアリングの3Dモデルを使用してステア角の表示をする
    public class steering_rot : MonoBehaviour
    {

        private float steering;
        public Udp_com.UdpReceiver udpreceiver;
        GameObject obj;
        Transform myTransform;
        Quaternion rot, rot_ini, rot_end;
        Vector3 steer_angle = new Vector3(0.0f, 1.0f, 0.21255656167f);
        Vector3 ini = new Vector3(0.0f, -1.0f, 1.0f);
        void Start()
        {
            obj = GameObject.Find("Udp_Receiver");
            udpreceiver = obj.GetComponent<Udp_com.UdpReceiver>();
            myTransform = this.transform;
            rot_ini = Quaternion.AngleAxis(303, Vector3.right);

        }

        // Update is called once per frame
        void FixedUpdate()
        {
            steering = (float)udpreceiver.steering;
            rot = Quaternion.AngleAxis(-steering * 0.05f, ini);
            //rot_end = Quaternion.AngleAxis(258, transform.right);
            myTransform.rotation = rot * rot_ini;
        }
    }
}

