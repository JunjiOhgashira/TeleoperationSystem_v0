using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Valve.VR;

namespace Image_com
{
    //仮想空間上にあるHMDの視点を固定するためクラス
    public class Cam_Poscon : MonoBehaviour
    {
        Vector3 basePos = Vector3.zero;

        void Start()
        {
            basePos = transform.position;
        }

        void Update()
        {
            // VR.InputTracking から hmd の位置を取得
            var trackingPos = InputTracking.GetLocalPosition(XRNode.Head);
            //Debug.Log(trackingPos);

            var scale = transform.localScale;
            trackingPos = new Vector3(
                trackingPos.x * scale.x,
                trackingPos.y * scale.y,
                trackingPos.z * scale.z
            );

            // 回転
            trackingPos = transform.rotation * trackingPos;

            // 固定したい位置から hmd の位置を
            // 差し引いて実質 hmd の移動を無効化する
            transform.position = basePos - trackingPos;

        }
    }
}

