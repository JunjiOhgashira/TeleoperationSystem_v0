using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inner_change : MonoBehaviour
{
    public Other_param other_param;
    GameObject line_gameobject;
    GameObject model_gameobject;
    Display_change display_change,display_change1;

    private bool Inner_mode, pre_mode;
    // Start is called before the first frame update
    void Awake()
    {
        pre_mode = other_param.compen_mode;
        line_gameobject = GameObject.Find("liner_display");
        model_gameobject = GameObject.Find("vehicle_model");
        display_change = new Display_change(pre_mode, Inner_mode, line_gameobject);
        display_change.re_ini_process();
        display_change1 = new Display_change(pre_mode, Inner_mode, model_gameobject);
        display_change1.ini_process();
    }

    // Update is called once per frame
    void Update()
    {
        
        Inner_mode = other_param.display_mode;

        display_change = new Display_change(pre_mode, Inner_mode, line_gameobject);
        display_change1= new Display_change(pre_mode, Inner_mode, model_gameobject);
        
        display_change.re_change_process();
        display_change1.change_process();

        pre_mode = Inner_mode;


    }
}
