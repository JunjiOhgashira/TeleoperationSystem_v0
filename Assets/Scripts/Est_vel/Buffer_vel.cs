using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using static System.Console;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System;
using System.Linq;
using System.IO;
using System.Threading.Tasks;

public class Buffer_vel : MonoBehaviour
{


    public Udp_com.UdpReceiver udpreceiver;
    public Predictive_calc.Est_parameter est_parameter;
    public Est_velocity est_velocity;


    private UInt32 timestamp = new UInt32();
    private UInt32 timestamp_pre = new UInt32();
    private Int16 real_vel = new Int16();

    private double down_time = 0;

    Queue<double> buffer_vel = new Queue<double>();
    Queue<long> buffer_time = new Queue<long>();

    System.Diagnostics.Stopwatch sw;
    Thread Buffer_thread;
    // Start is called before the first frame update
    void Start()
    {
        down_time = est_parameter.down_time;
        sw = new System.Diagnostics.Stopwatch();
        sw.Start();


        // コルーチンを設定
        StartCoroutine(Buffer_velocity());


        Buffer_thread = new Thread(new ThreadStart(Buffer_exit));
        Buffer_thread.Start();
    }

    void OnApplicationQuit()
    {

        Buffer_thread.Abort();
    }

    //実速度の配列を作成
    private IEnumerator Buffer_velocity()
    {
        while (true)
        {
            timestamp = udpreceiver.timestamp;
            //実速度のタイムスタンプが変わるまで待機
            yield return new WaitUntil(() => timestamp != udpreceiver.timestamp);

            buffer_vel.Enqueue(udpreceiver.real_vel);
            buffer_time.Enqueue(sw.ElapsedMilliseconds);
        }
    }

    //実速度のバッファリング処理部分
    private void Buffer_exit()
    {
        while (true)
        {
            down_time = est_parameter.down_time;
            if (buffer_time.Count == 0)
            { 
            } 
            else if (down_time < sw.ElapsedMilliseconds - buffer_time.Peek())
            {
                est_velocity.real_vel2 = buffer_vel.Dequeue();
                long gabage = buffer_time.Dequeue();
                
            }
            else
            {  }
           
        }
    }
}
