using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    //七色に変化させてアクセル、ブレーキの踏み込み量を表示する
    public class pedal_col : MonoBehaviour
    {

        LineRenderer line;


        // Start is called before the first frame update
        void Start()
        {
            GameObject image_object = GameObject.Find("Image");
            GameObject canvas_object = GameObject.Find("Canvas");
            Vector3 canvas_pos = canvas_object.GetComponent<RectTransform>().localPosition;
            Vector3 image_pos = image_object.GetComponent<RectTransform>().localPosition;
            //Debug.Log(canvas_pos.x);


            line = GetComponent<LineRenderer>();
            var positions = new Vector3[]{
        new Vector3(canvas_pos.x + image_pos.x -49.0f, canvas_pos.y + image_pos.y + 6.3f, 0),               // 開始点
        new Vector3(canvas_pos.x + image_pos.x -2.0f, canvas_pos.y + image_pos.y + 6.3f, 0),               // 終了点
        new Vector3(canvas_pos.x + image_pos.x -2.0f, canvas_pos.y + image_pos.y -18.5f, 0),
        new Vector3(canvas_pos.x + image_pos.x -49.0f, canvas_pos.y + image_pos.y -18.5f, 0),
    };
            line.positionCount = positions.Length;
            line.SetPositions(positions);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

