using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    //ブレーキペダルの2Dモデルを表示する
    public class brakepedal_col : MonoBehaviour
    {

        LineRenderer line;
        GameObject image_object;
        GameObject canvas_object;
        GameObject camera_object;
        GameObject udp_object;

        Color start_col;
        Color end_col;

        public Udp_com.UdpReceiver udpreceiver;

        private float brake_pedal;
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
            var q = Quaternion.Euler(45f, 0f, 0f);

            var positions = new Vector3[]{
        new Vector3(canvas_pos.x - 0.196f + x_axis, canvas_pos.y + 0.0252f + y_axis, canvas_pos.z + z_axis),               // 開始点
        new Vector3(canvas_pos.x - 0.008f + x_axis, canvas_pos.y + 0.0252f + y_axis, canvas_pos.z +z_axis),               // 終了点
        new Vector3(canvas_pos.x - 0.008f + x_axis, canvas_pos.y -0.074f + y_axis, canvas_pos.z + z_axis),
        new Vector3(canvas_pos.x - 0.196f + x_axis, canvas_pos.y -0.074f + y_axis, canvas_pos.z + z_axis),};
            for (int i = 0; i < positions.Length; i++)
            {
                positions[i] = q * positions[i];
            }

            brake_pedal = udpreceiver.brake_pedal;
            if (0 < brake_pedal && brake_pedal <= 25)
            {
                start_col = new Color(0.0f, brake_pedal / 25.0f, 1.0f, 1.0f);
                end_col = start_col;
                line.startColor = start_col;
                line.endColor = end_col;
            }
            else if (brake_pedal > 25 && brake_pedal <= 50)
            {
                start_col = new Color(0.0f, 1.0f, 1.0f - ((brake_pedal - 25.0f) / 25.0f), 1.0f);
                end_col = start_col;
                line.startColor = start_col;
                line.endColor = end_col;
            }
            else if (brake_pedal > 50 && brake_pedal <= 75)
            {
                start_col = new Color((brake_pedal - 50.0f) / 25.0f, 1.0f, 0.0f, 1.0f);
                end_col = start_col;
                line.startColor = start_col;
                line.endColor = end_col;
            }
            else if (brake_pedal > 75 && brake_pedal <= 100)
            {
                start_col = new Color(1.0f, 1.0f - ((brake_pedal - 75.0f) / 25.0f), 0.0f, 1.0f);
                end_col = start_col;
                line.startColor = start_col;
                line.endColor = end_col;
            }
            line.positionCount = positions.Length;
            line.SetPositions(positions);
        }
    }

}
