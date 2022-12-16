using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI 
{
    //アクセルペダルの2Dモデルを描画する
    public class accelpedal_col : MonoBehaviour
    {

        LineRenderer line;
        GameObject image_object;
        GameObject canvas_object;
        GameObject camera_object;
        GameObject udp_object;

        Color start_col;
        Color end_col;

        public Udp_com.UdpReceiver udpreceiver;

        private float accel_pedal;
        public float x_axis = 0.0f;
        public float y_axis = 0.0f;
        public float z_axis = 0.0f;


        // Start is called before the first frame update
        void Start()
        {
            image_object = GameObject.Find("Image");
            canvas_object = GameObject.Find("Canvas");
            line = GetComponent<LineRenderer>();
            line.material = new Material(Shader.Find("Sprites/Default"));
            line.startColor = Color.blue;
            line.endColor = Color.blue;



        }

        // Update is called once per frame
        void FixedUpdate()
        {
            Vector3 image_pos = image_object.GetComponent<RectTransform>().localPosition;
            Vector3 canvas_pos = canvas_object.transform.position;
            //Debug.Log(image_pos.x);

            var q = Quaternion.Euler(45f, 0f, 0f);

            /*var positions = new Vector3[]{
            new Vector3((canvas_pos.x + image_pos.x +27.0f)*scale, (canvas_pos.y + image_pos.y + 21.5f)*scale, canvas_pos.z*scale),               // 開始点
            new Vector3((canvas_pos.x + image_pos.x +36.0f)*scale, (canvas_pos.y + image_pos.y + 21.5f)*scale, canvas_pos.z*scale),               // 終了点
            new Vector3((canvas_pos.x + image_pos.x +49.0f)*scale, (canvas_pos.y + image_pos.y -18.5f)*scale, canvas_pos.z*scale),
            new Vector3((canvas_pos.x + image_pos.x +49.0f)*scale, (canvas_pos.y + image_pos.y -37.0f)*scale, canvas_pos.z*scale),
            new Vector3((canvas_pos.x + image_pos.x +27.0f)*scale, (canvas_pos.y + image_pos.y -37.0f)*scale,canvas_pos.z*scale),
        };*/

            var positions = new Vector3[]{
        new Vector3(canvas_pos.x + 0.108f + x_axis, canvas_pos.y + 0.086f + y_axis, canvas_pos.z + z_axis),               // 開始点
        new Vector3(canvas_pos.x + 0.144f + x_axis, canvas_pos.y + 0.086f + y_axis, canvas_pos.z +z_axis),               // 終了点
        new Vector3(canvas_pos.x + 0.196f + x_axis, canvas_pos.y -0.074f + y_axis, canvas_pos.z + z_axis),
        new Vector3(canvas_pos.x + 0.196f + x_axis, canvas_pos.y -0.148f + y_axis, canvas_pos.z + z_axis),
        new Vector3(canvas_pos.x + 0.108f + x_axis, canvas_pos.y - 0.148f + y_axis, canvas_pos.z + z_axis), };
            for (int i = 0; i < positions.Length; i++)
            {
                positions[i] = q * positions[i];
            }

            accel_pedal = udpreceiver.accel_pedal;
            //Debug.Log(accel_pedal);
            if (0 < accel_pedal && accel_pedal <= 25)
            {
                start_col = new Color(0.0f, accel_pedal / 25.0f, 1.0f, 1.0f);
                end_col = start_col;
                line.startColor = start_col;
                line.endColor = end_col;
            }
            else if (accel_pedal > 25 && accel_pedal <= 50)
            {
                start_col = new Color(0.0f, 1.0f, 1.0f - ((accel_pedal - 25.0f) / 25.0f), 1.0f);
                end_col = start_col;
                line.startColor = start_col;
                line.endColor = end_col;
            }
            else if (accel_pedal > 50 && accel_pedal <= 75)
            {
                start_col = new Color((accel_pedal - 50.0f) / 25.0f, 1.0f, 0.0f, 1.0f);
                end_col = start_col;
                line.startColor = start_col;
                line.endColor = end_col;
            }
            else if (accel_pedal > 75 && accel_pedal <= 100)
            {
                start_col = new Color(1.0f, 1.0f - ((accel_pedal - 75.0f) / 25.0f), 0.0f, 1.0f);
                end_col = start_col;
                line.startColor = start_col;
                line.endColor = end_col;
            }

            //line.startWidth = 1.0f;
            line.positionCount = positions.Length;
            line.SetPositions(positions);
        }
    }

}

