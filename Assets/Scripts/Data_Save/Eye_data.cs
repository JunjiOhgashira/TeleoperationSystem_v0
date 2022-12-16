using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;


namespace ViveSR
{
    namespace anipal
    {
        namespace Eye
        {
            //アイトラッキングのデータを保存するクラス（未完成？）
            public class Eye_data : MonoBehaviour
            {
                public Csv_param csv_param;
                private string filename, fileName;
                private bool eye_mode;
                StreamWriter sw;
                FileInfo fi;

                EyeData eye;

                //origin：起点，direction：レイの方向　x,y,z軸
                //両目の視線格納変数
                Vector3 CombineGazeRayorigin;
                Vector3 CombineGazeRaydirection;
                // Start is called before the first frame update
                void Start()
                {
                    filename = csv_param.fileName;
                    eye_mode = csv_param.eye_mode;


                    if (eye_mode == true)
                    {
                        DateTime now = DateTime.Now;
                        fileName = filename + now.Year.ToString() + "_" + now.Month.ToString() + "_" + now.Day.ToString() + "__" + now.Hour.ToString() + "_" + now.Minute.ToString() + "_" + now.Second.ToString();
                        fi = new FileInfo(Application.dataPath + "/Experiment_data/eye/" + fileName + ".csv");
                        sw = fi.AppendText();
                        sw.Write("時間[s]");
                        sw.Write(" ");
                        sw.Write("起点x");
                        sw.Write(" ");
                        sw.Write("起点y");
                        sw.Write(" ");
                        sw.Write("起点z");
                        sw.Write(" ");
                        sw.Write("ベクトルx");
                        sw.Write(" ");
                        sw.Write("ベクトルy");
                        sw.Write(" ");
                        sw.WriteLine("ベクトルz");
                    }

                }

                // Update is called once per frame
                void Update()
                {
                    //⓪取得呼び出し-----------------------------
                    SRanipal_Eye_API.GetEyeData(ref eye);

                    //③視線情報--------------------（目をつぶると検知されない）
                    //両目の視線情報が有効なら視線情報を表示origin：起点，direction：レイの方向

                    if (eye_mode == true)
                    {
                        if (SRanipal_Eye.GetGazeRay(GazeIndex.COMBINE, out CombineGazeRayorigin, out CombineGazeRaydirection, eye))
                        {
                            sw.Write(Time.time);
                            sw.Write(" ");
                            sw.Write(CombineGazeRayorigin.x);
                            sw.Write(" ");
                            sw.Write(CombineGazeRayorigin.y);
                            sw.Write(" ");
                            sw.Write(CombineGazeRayorigin.z);
                            sw.Write(" ");
                            sw.Write(CombineGazeRaydirection.x);
                            sw.Write(" ");
                            sw.Write(CombineGazeRaydirection.y);
                            sw.Write(" ");
                            sw.WriteLine(CombineGazeRaydirection.z);
                            sw.Flush();

                        }
                    }


                }

                void OnApplicationQuit()
                {
                    if (eye_mode == true)
                    {
                        sw.Close();
                        Debug.Log("eye_Save Completed");
                    }
                }
            }
        }
    }
}