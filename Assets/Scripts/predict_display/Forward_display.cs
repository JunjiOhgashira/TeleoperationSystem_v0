using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Predictive_display
{
    //青フレームを描画する
    public class Forward_display : MonoBehaviour
    {
        public Predictive_calc.Est_parameter est_parameter;

        LineRenderer line;

        Vector3 v_up_right;
        Vector3 v_up_left;
        Vector3 v_down_left;
        Vector3 v_down_right;


        // Start is called before the first frame update
        void Start()
        {
            line = GetComponent<LineRenderer>();
            line.material = new Material(Shader.Find("Sprites/Default"));
            line.positionCount = 4;

        }

        // Update is called once per frame
        void Update()
        {
            //計算されていた青フレームを参照し描画する
            v_up_right = est_parameter.v_up_right;
            v_up_left = est_parameter.v_up_left;
            v_down_left = est_parameter.v_down_left;
            v_down_right = est_parameter.v_down_right;

            line.SetPosition(0, v_up_right);
            line.SetPosition(1, v_up_left);
            line.SetPosition(2, v_down_left);
            line.SetPosition(3, v_down_right);
        }
    }
}

