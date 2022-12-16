using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Velocity_change : MonoBehaviour
{
    public Other_param other_param;
    GameObject gameobject,gameobject1;
    Display_change display_change,display_change1;

    private bool Velocity_mode, pre_mode;

    // Start is called before the first frame update
    void Start()
    {
        pre_mode = other_param.Velocity_mode;
        gameobject = GameObject.Find("velocity_dis");
        gameobject1 = GameObject.Find("Text");
        display_change = new Display_change(pre_mode, Velocity_mode, gameobject);
        display_change1 = new Display_change(pre_mode, Velocity_mode, gameobject1);
        display_change.ini_process();
        display_change1.ini_process();
    }

    // Update is called once per frame
    void Update()
    {
        Velocity_mode = other_param.Velocity_mode;

        display_change = new Display_change(pre_mode, Velocity_mode, gameobject);
        display_change1 = new Display_change(pre_mode, Velocity_mode, gameobject1);
        display_change.change_process();
        display_change1.change_process();

        pre_mode = Velocity_mode;
    }
}
