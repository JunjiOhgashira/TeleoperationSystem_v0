using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace UI
{
    //速度UIを表示するクラス
    public class velocity_dis : MonoBehaviour
    {

        public GameObject velocity_obj = null; // Textオブジェクト
        public Predictive_calc.Est_parameter est_parameter;
        private double velocity;
        private string velocity_3digit;
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            //Debug.Log(main_predict.velocity);
            velocity = Math.Round(est_parameter.velocity * 3.6, 1, MidpointRounding.AwayFromZero);  //少数第2位で四捨五入
            velocity_3digit = velocity.ToString("f1");
            Text velocity_text = velocity_obj.GetComponent<Text>();
            velocity_text.color = new Color(1.0f, 1.0f, 0.0f, 1.0f);
            velocity_text.text = velocity_3digit + "km/h";
        }
    }
}

