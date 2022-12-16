using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steering_change : MonoBehaviour
{

    public Other_param other_param;
    GameObject gameobject;
    Display_change display_change;

    private bool Steering_mode, pre_mode;
    // Start is called before the first frame update
    void Start()
    {
        pre_mode = other_param.Steering_mode;
        gameobject = GameObject.Find("steering _wheel");
        display_change = new Display_change(pre_mode, Steering_mode, gameobject);
        display_change.ini_process();
    }

    // Update is called once per frame
    void Update()
    {
        Steering_mode = other_param.Steering_mode;

        display_change = new Display_change(pre_mode, Steering_mode, gameobject);

        display_change.change_process();
        pre_mode = Steering_mode;
    }
}
