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

namespace Udp_com
{
    //三菱PCからのデータを受信プログラムの実行クラス
    public class UdpReceiver : MonoBehaviour
    {

        public Udp_param udp_param;
        public Other_param other_param;

        [SerializeField]
        private int LOCA_LPORT;

        //各受信データ変数宣言
        byte data_id = new byte();
        public UInt16 RTT = new UInt16();
        public byte accel_pedal = new byte();
        public byte brake_pedal = new byte();
        public Int16 steering = new Int16();
        public byte[] image;
        public UInt16 slvout_time;

        //gps関係変数宣言
        public Int32 gps_lat = new Int32();
        public Int32 gps_lon = new Int32();
        public UInt16 gps_dir = new UInt16();
        public UInt16 slvin_time = new UInt16();
        public Int16 real_vel = new Int16();
        public Int16 yaw_rate = new Int16();
        public UInt32 timestamp = new UInt32();

        private bool est_vel_mode = false;

        Thread Receiver_thread;


        // Start is called before the first frame update
        void Start()
        {
            
            Receiver_thread = new Thread(new ThreadStart(Udp_Receiver));
            Receiver_thread.Start();

            LOCA_LPORT = udp_param.LOCA_LPORT;
            est_vel_mode = other_param.Est_vel_mode;
        }

        void OnApplicationQuit()
        {

            Receiver_thread.Abort();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void Udp_Receiver()
        {
            //受信部分
            Rev_process rev_process = new Rev_process(LOCA_LPORT);
            while (true)
            {
                try
                {
                    //通信接続を行いタイムスタンプ、データidの取得
                    rev_process.connect_and_receive();
                    data_id = rev_process.data_id;

                    //それぞれのデータidごとの処理
                    if (data_id == 0x04)
                    {
                        //RTT取得
                        rev_process.delay_receive();
                        RTT = rev_process.Rtt;

                    }
                    else if (data_id == 0x02)
                    {
                        //コントロールデータ取得、代入
                        rev_process.control_receive();
                        accel_pedal = rev_process.Accel_pedal;
                        brake_pedal = rev_process.Brake_pedal;
                        steering = (Int16)(rev_process.Steering);

                    }
                    else if (data_id == 0x01)
                    {

                        //カメラ画像の取得
                        if (rev_process.image_receive())
                        {
                            continue;
                        }
                        else
                        {
                            image = rev_process.Image;
                            slvout_time = rev_process.Slvout_time;
                           
                        }


                    }
                    else if(data_id == 0x08)
                    {
                        //gpsデータなどの取得
                        rev_process.gps_data();
                        gps_lat = rev_process.Gps_lat;
                        gps_lon = rev_process.Gps_lon;
                        gps_dir = rev_process.Gps_dir;
                        slvin_time = rev_process.Slvin_time;
                        real_vel = rev_process.Real_vel;
                        yaw_rate = rev_process.Yaw_rate;
                        timestamp = rev_process.Timestamp;

                    }
                    else
                    {

                        Console.WriteLine("data_idが間違っています");
                    }

                }
                catch (SocketException)
                {
                }

            }

        }
    }

   
}

