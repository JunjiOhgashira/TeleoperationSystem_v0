using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

namespace Data_Save
{
    //ステア角の保存に関する処理クラス
    class Save_est_Rudder_angle : Save_abstract
    {
        StreamWriter sw;
        FileInfo fi;
        private double ru_angle;

        private string filename, fileName;


        //  コンストラクタ
        public Save_est_Rudder_angle() : base("推定舵角")
        {
        }

        public double Ru_angle
        {
            set { ru_angle = value; }
        }
        public override string Filename
        {
            set { filename = value; }
            get { return filename; }
        }

        //  ファイルオープン
        public override void file_open()
        {
            DateTime now = DateTime.Now;
            fileName = filename + now.Year.ToString() + "_" + now.Month.ToString() + "_" + now.Day.ToString() + "__" + now.Hour.ToString() + "_" + now.Minute.ToString() + "_" + now.Second.ToString();
            fi = new FileInfo(Application.dataPath + "/Experiment_data/Rudder_angle/" + fileName + ".csv");
            sw = fi.AppendText();
            sw.Write("時間[s]");
            sw.Write(" ");
            sw.WriteLine("タイヤの舵角[deg]");
        }

        //ファイル書き込み
        public override void file_write()
        {
            sw.Write(Time.time);
            sw.Write(" ");
            sw.WriteLine(ru_angle);
            sw.Flush();
        }

        //ファイルクローズ
        public override void file_close()
        {
            sw.Close();
            Debug.Log("angle_Save Completed");
        }
    }
}