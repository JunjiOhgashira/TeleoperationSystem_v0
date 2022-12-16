using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Display_change
{
    private bool pre_mode, mode;
    GameObject gameobject;
    public Display_change(bool pre_mode,bool mode,GameObject gameobject)
    {
        this.pre_mode = pre_mode;
        this.mode = mode;
        this.gameobject = gameobject;
    }

    public void ini_process()
    {
        if(pre_mode == true)
        {
            gameobject.SetActive(true);
        }
        else
        {
            gameobject.SetActive(false);
        }
    }

    public void re_ini_process()
    {
        if (pre_mode == true)
        {
            gameobject.SetActive(false);
        }
        else
        {
            gameobject.SetActive(true);
        }
    }

    public void change_process()
    {
        if(pre_mode != mode)
        {
            if(mode == true)
            {
                gameobject.SetActive(true);
                
            }
            else
            {
                gameobject.SetActive(false);
                
            }

            
        }
        
    }

    public void re_change_process()
    {

        if (pre_mode != mode)
        {
            if (mode == true)
            {
                gameobject.SetActive(false);

            }
            else
            {
                gameobject.SetActive(true);

            }


        }

    }
}
