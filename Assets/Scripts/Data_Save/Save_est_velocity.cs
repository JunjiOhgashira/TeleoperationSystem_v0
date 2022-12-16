using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

namespace Data_Save
{
    //推定速度の保存に関する処理クラス
    class Save_est_velocity : Save_abstract
    {
        StreamWriter sw;
        FileInfo fi;
        private double velocity;
        private double real_vel;
        private double real_vel1;
        private string filename, fileName;


        //  コンストラクタ
        public Save_est_velocity() : base("指令値からの推定速度")
        {
        }

        public double Velocity
        {
            set { velocity = value; }
        }

        public double Real_vel
        {
            set { real_vel = value; }
        }

        public double Real_vel1
        {
            set { real_vel1 = value; }
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
            fi = new FileInfo(Application.dataPath + "/Experiment_data/Est_velocity/" + fileName + ".csv");
            sw = fi.AppendText();

        }

        //ファイル書き込み
        public override void file_write()
        {
            

            sw.Write(Time.time);
            sw.Write(" ");
            sw.Write(velocity* 3.6);
            sw.Write(" ");
            sw.Write(real_vel * 3.6);
            sw.Write(" ");
            sw.WriteLine(real_vel1 * 3.6);
            sw.Flush();
        }

        //ファイルクローズ
        public override void file_close()
        {
            sw.Close();
            Debug.Log("Velocity_Save Completed");
        }
    }
}

