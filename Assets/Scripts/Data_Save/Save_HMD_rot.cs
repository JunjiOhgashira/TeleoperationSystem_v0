using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Valve.VR;
using System.IO;
using System;

namespace Data_Save
{
    //HMDの回転角の保存に関する処理クラス
    class Save_HMD_rot : Save_abstract
    {

        StreamWriter sw;
        FileInfo fi;


        private string filename,fileName;
        private Vector3 HMD_rotation;


        //  コンストラクタ
        public Save_HMD_rot() : base("HMDの回転")
        {
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
            fi = new FileInfo(Application.dataPath + "/Experiment_data/HMD_rot/" + fileName + ".csv");
            sw = fi.AppendText();
            sw.Write("時間[s]");
            sw.Write(" ");
            sw.Write("x軸周り[deg]");
            sw.Write(" ");
            sw.Write("y軸周り[deg]");
            sw.Write(" ");
            sw.WriteLine("z軸周り[deg]");
        }
        //ファイル書き込み
        public override void file_write()
        {
            // VR.InputTracking から hmd の位置を取得
            var trackingRot = InputTracking.GetLocalRotation(XRNode.Head);
            HMD_rotation = trackingRot.eulerAngles;

            sw.Write(Time.time);
            sw.Write(" ");
            sw.Write(HMD_rotation.x);
            sw.Write(" ");
            sw.Write(HMD_rotation.y);
            sw.Write(" ");
            sw.WriteLine(HMD_rotation.z);
            sw.Flush();
        }

        //ファイルクローズ
        public override void file_close()
        {
            sw.Close();
            Debug.Log("HMD_rot_Save Completed");
        }
    }
}
