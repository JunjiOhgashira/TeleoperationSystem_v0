using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Image_com
{
    //全方位カメラの位置によって仮想空間上の視点位置を調整するクラス
    public class Camera_pos : MonoBehaviour
    {

        public Vehicle_param vehicle_param;


        // Start is called before the first frame update
        void Start()
        {
            Vector3 pos = gameObject.transform.position;

            pos.y = (float)vehicle_param.camera_pos_Y;
            pos.z = (float)vehicle_param.camera_pos_Z;

            gameObject.transform.position = pos;
        }

    }
}

