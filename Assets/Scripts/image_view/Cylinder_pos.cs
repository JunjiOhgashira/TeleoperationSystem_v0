using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Image_com
{
    //カメラ位置によって円筒の位置を調整するクラス
    public class Cylinder_pos : MonoBehaviour
    {

        public Vehicle_param vehicle_param;


        // Start is called before the first frame update
        void Start()
        {
            Vector3 pos = gameObject.transform.position;
            Vector3 scale = gameObject.transform.localScale;
            //Debug.Log(scale.z);
            pos.y = (float)(vehicle_param.camera_pos_Y + scale.z*0.113);
            pos.z = (float)vehicle_param.camera_pos_Z;

            gameObject.transform.position = pos;
        }

    }
}

