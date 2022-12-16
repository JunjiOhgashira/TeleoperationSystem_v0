using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.IO;
using static System.Console;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Predictive_calc
{
    //予測表示の座標点の計算を実行するクラス
    public class Main_Predict : MonoBehaviour
    {
        public Est_parameter est_parameter;
        int count = 0;
        TimeSpan timeElapsed,time_count;
        Thread Receiver_thread;

        System.Diagnostics.Stopwatch sw;
        // Start is called before the first frame update
        void Start()
        {
            sw = new System.Diagnostics.Stopwatch();
            sw.Start(); //計測開始
            Receiver_thread = new Thread(new ThreadStart(Fixed_renewal));
            Receiver_thread.Start();
        }

        void OnApplicationQuit()
        {

            Receiver_thread.Abort();
        }

        // Update is called once per frame
        async void Fixed_renewal()
        {
            while (true)
            {
                
                time_count = sw.Elapsed;
                if (time_count.TotalMilliseconds - timeElapsed.TotalMilliseconds < 50) continue;
                timeElapsed = sw.Elapsed;

                //各種変数を参照する
                est_parameter.command_receiver();
                
                //遅延データ処理
                est_parameter.delay_proc();
                
                //ステアリング角、アクセルブレーキペダルの指令値から滑り角などを求める
                est_parameter.command_proc();

                //予測表示計算
                est_parameter.calc_predict();
                
            }
        }
    }
}




