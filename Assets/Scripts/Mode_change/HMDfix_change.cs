using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HMDfix_change : MonoBehaviour
{
    public Other_param other_param;
    GameObject gameobject;

    private bool predict_mode, pre_mode;
    // Start is called before the first frame update
    void Start()
    {
        pre_mode = other_param.HMDfix_mode;
        gameobject = GameObject.Find("[CameraRig]");

        if (pre_mode == true)
        {
            gameobject.GetComponent<Image_com.Cam_Poscon>().enabled = true;
        }
        else
        {
            gameobject.GetComponent<Image_com.Cam_Poscon>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

        predict_mode = other_param.HMDfix_mode;

        if (pre_mode != predict_mode)
        {
            if (predict_mode == true)
            {
                gameobject.GetComponent<Image_com.Cam_Poscon>().enabled = true;


            }
            else
            {
                gameobject.GetComponent<Image_com.Cam_Poscon>().enabled = false;

            }

        }
        pre_mode = predict_mode;
    }
}
