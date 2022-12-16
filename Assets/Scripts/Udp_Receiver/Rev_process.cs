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
    //受信関係の処理を記述してるクラス
    public class Rev_process
    {
        public Other_param other_param;
        //ポート番号,受信データ格納
        private int LOCA_LPORT;
        private static UdpClient udp;
        private byte[] data;

        //UDP通信の共通データ
        private UInt32 timestamp = new UInt32();
        public byte data_id = new byte();

        //通信遅延データ
        private UInt16 RTT = new UInt16();

        //コントローラーデータ
        private byte accel_pedal = new byte();
        private byte brake_pedal = new byte();
        private Int16 steering = new Int16();

        //gps関係
        private Int32 gps_lat = new Int32();
        private Int32 gps_lon = new Int32();
        private UInt16 gps_dir = new UInt16();
        private UInt16 slvin_time = new UInt16();
        private Int16 real_vel = new Int16();
        private Int16 yaw_rate = new Int16();
        private bool est_vel_mode = false;
        //画像データ取得関係
        private UInt16 slvout_time = new UInt16();
        private byte mode = new byte();
        int[] img_count = new int[0];
        byte[] src;
        public byte[] image;
        private int con_num = 0;

        
        //各受信データのゲッター、セッター
        public UInt32 Timestamp
        {
            get { return this.timestamp; }
            private set { this.timestamp = value; }
        }


        public UInt16 Rtt
        {
            get { return this.RTT; }
            private set { this.RTT = value; }
        }

        public byte Accel_pedal
        {
            get { return this.accel_pedal; }
            private set { this.accel_pedal = value; }
        }

        public byte Brake_pedal
        {
            get { return this.brake_pedal; }
            private set { this.brake_pedal = value; }
        }

        public Int16 Steering
        {
            get { return this.steering; }
            private set { this.steering = value; }
        }

        public byte[] Image
        {
            get { return this.image; }
            private set { this.image = value; }
        }

        public UInt16 Slvout_time
        {
            get { return this.slvout_time; }
            private set { this.slvout_time = value; }
        }

        public Int32 Gps_lat
        {
            get { return this.gps_lat; }
            private set { this.gps_lat = value; }
        }

        public Int32 Gps_lon
        {
            get { return this.gps_lon; }
            private set { this.gps_lon = value; }
        }

        public UInt16 Gps_dir
        {
            get { return this.gps_dir; }
            private set { this.gps_dir = value; }
        }

        public UInt16 Slvin_time
        {
            get { return this.slvin_time; }
            private set { this.slvin_time = value; }
        }

        public Int16 Real_vel
        {
            get { return this.real_vel; }
            private set { this.real_vel = value; }
        }

        public Int16 Yaw_rate
        {
            get { return this.yaw_rate; }
            private set { this.yaw_rate = value; }
        }

        void Awake()
        {
            est_vel_mode = other_param.Est_vel_mode;
        }
        //ポート番号セット
        public Rev_process(int LOCA_LPORT)
        {
            this.LOCA_LPORT = LOCA_LPORT;
            udp = new UdpClient(LOCA_LPORT);
        }
        //通信接続とタイムスタンプ、idの取得
        public void connect_and_receive()
        {
            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
            data = udp.Receive(ref remoteEP);
            timestamp = BitConverter.ToUInt32(data, 0);
            data_id = data[4];

        }

        //遅延データ取得
        public void delay_receive()
        {
            RTT = BitConverter.ToUInt16(data, 5);
        }

        //コントローラデータ取得
        public void control_receive()
        {
            accel_pedal = data[5];
            brake_pedal = data[6];
            steering = BitConverter.ToInt16(data, 7);
        }

        //緯度、経度関係データ取得
        public void gps_data()
        {
            gps_lat = BitConverter.ToInt32(data, 5);
            gps_lon = BitConverter.ToInt32(data, 9);
            gps_dir = BitConverter.ToUInt16(data, 13);
            slvin_time = BitConverter.ToUInt16(data, 15);
            real_vel = BitConverter.ToInt16(data, 17);
            yaw_rate = BitConverter.ToInt16(data, 19);
            

        }
        //画像データの復元と取得
        public bool image_receive()
        {
            slvout_time = BitConverter.ToUInt16(data, 5);
            mode = data[7];
            int number = (int)(data[8] - 1);
            int com_div = (int)data[9];
            int send = (int)BitConverter.ToUInt32(data, 10);

            if (img_count.All(i => i == 0))
            {
                img_count = new int[com_div];
                src = new byte[0];
                if (number != 0)
                {
                    return true;
                }
            }
            else
            {
                if (!(con_num == (number - 1)))
                {
                    img_count = new int[com_div];
                    src = new byte[0];
                    return true;
                }
            }

            img_count[number] = 1;
            con_num = number;
            Array.Resize(ref src, src.Length + send);
            Buffer.BlockCopy(data, 14, src, number * 64000, send);


            //最後の分割番号がきたら、画像をimage変数にコピー
            if (com_div == (number + 1))
            {

                image = new byte[src.Length];
                Buffer.BlockCopy(src, 0, image, 0, src.Length);
                img_count = new int[com_div];
                src = new byte[0];
                return false;
            }
            //最後以外の分割番号だったら、再度データを受信し取得する
            return true;
        }
    }
}
