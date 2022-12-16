using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Predict_change : MonoBehaviour
{
    public Other_param other_param;
    GameObject gameobject;
    Display_change display_change;

    private bool predict_mode,pre_mode;
    // Start is called before the first frame update
    void Start()
    {
        pre_mode = other_param.predict_mode;
        gameobject = GameObject.Find("Main_Predict");
        display_change = new Display_change(pre_mode, predict_mode, gameobject);
        display_change.ini_process();
    }

    // Update is called once per frame
    void Update()
    {
        predict_mode = other_param.predict_mode;

        display_change = new Display_change(pre_mode, predict_mode, gameobject);
        display_change.change_process();
        pre_mode = predict_mode;
    }
}
