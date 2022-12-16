using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pedal_change : MonoBehaviour
{
    public Other_param other_param;
    GameObject image_gameobject,acc_gameobject,bra_gameobject;
    Display_change imagedisplay_change,accdisplay_change,bradisplay_change;

    private bool pedal_mode, pre_mode;
    // Start is called before the first frame update
    void Awake()
    {
        pre_mode = other_param.pedal_mode;

        image_gameobject = GameObject.Find("Image");
        imagedisplay_change = new Display_change(pre_mode, pedal_mode, image_gameobject);
        imagedisplay_change.ini_process();

        acc_gameobject = GameObject.Find("accelepedal");
        accdisplay_change = new Display_change(pre_mode,pedal_mode,acc_gameobject);
        accdisplay_change.ini_process();

        bra_gameobject = GameObject.Find("brake_pedal");
        bradisplay_change = new Display_change(pre_mode, pedal_mode, bra_gameobject);
        bradisplay_change.ini_process();
    }

    // Update is called once per frame
    void Update()
    {
        pedal_mode = other_param.pedal_mode;

        imagedisplay_change = new Display_change(pre_mode, pedal_mode, image_gameobject);
        imagedisplay_change.change_process();

        accdisplay_change = new Display_change(pre_mode, pedal_mode, acc_gameobject);
        accdisplay_change.change_process();

        bradisplay_change = new Display_change(pre_mode, pedal_mode, bra_gameobject);
        bradisplay_change.change_process();

        pre_mode = pedal_mode;
    }
}
