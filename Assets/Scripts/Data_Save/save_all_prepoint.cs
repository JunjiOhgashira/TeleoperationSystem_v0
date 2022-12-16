using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

//予測表示の座標点の保存クラス（デバッグ用）
public class save_all_prepoint : MonoBehaviour
{

    StreamWriter sw;
    FileInfo fi;
    private string fileName;

    public Predictive_calc.Est_parameter est_parameter;

    // Start is called before the first frame update
    void Start()
    {
        DateTime now = DateTime.Now;
        fileName = now.Year.ToString() + "_" + now.Month.ToString() + "_" + now.Day.ToString() + "__" + now.Hour.ToString() + "_" + now.Minute.ToString() + "_" + now.Second.ToString();
        fi = new FileInfo(Application.dataPath + "/Experiment_data/" + fileName + ".csv");
        sw = fi.AppendText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDestroy()
    {
        int i = 0;
        for(i = 0;i < est_parameter.send_prepoint_x.Length; i++)
        {
            sw.Write(est_parameter.send_prepoint_x[i]);
            sw.Write(" ");
            sw.WriteLine(est_parameter.send_prepoint_z[i]);

        }
        sw.Close();
        Debug.Log("souda Completed");
    }
}
