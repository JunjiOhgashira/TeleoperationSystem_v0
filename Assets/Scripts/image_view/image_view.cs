using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Image_com
{
    //全方位カメラ画像を円筒に描画するクラス
    public class image_view : MonoBehaviour
    {

        public Udp_com.UdpReceiver udpreceiver;

        private byte[] image;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            image = udpreceiver.image;
            Texture2D texture = new Texture2D(0, 0);
            texture.LoadImage(image);
            texture.Apply();
            Destroy(GetComponent<Renderer>().material.mainTexture);
            GetComponent<Renderer>().material.mainTexture = texture;

        }
    }
}

