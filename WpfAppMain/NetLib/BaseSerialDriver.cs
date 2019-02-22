using CommonLib;
using CommonLib.Tools;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace NetLib
{

    public class BaseSericalCmdResult
    {
        public byte[] Bytes { get; set; }

        string hexStr = null;
        public string HexStr
        {
            get
            {
                if (hexStr != null)
                    return hexStr;
                hexStr = ByteCalcHelper.BytesToString(Bytes, 16);
                return hexStr;
            }
        }

        public string CmdType { get; set; }
    }

    //当串口数据发送频繁,可能产生粘包时,本串口类实现了拆包组包的功能.
    //算法依据: 多数串口协议都是 head+len+data+check?+tail,其中除data位以外,其他位长度都是一个byte
    //若协议不遵循此格式,可重写串口类,但本思路仍可借鉴
    public class BaseSerialDriver
    {
        public void InitPara(int BaudRate, int ReadTimeout, int WriteTimeout, byte Head, byte Tail, bool NeedVerify)
        {
            this.BaudRate = BaudRate;
            this.ReadTimeout = ReadTimeout;
            this.WriteTimeout = WriteTimeout;

            this.Head = Head;
            this.Tail = Tail;
            this.NeedVerify = NeedVerify;
        }

        int BaudRate = 57600;
        int ReadTimeout = 500;
        int WriteTimeout = 1000;

        byte Head;//包头   
        byte Tail;  //包尾 = 0x03;        
        bool NeedVerify;//是否有校验位

        SerialPort serialPort;
        byte[] lostbts; //上次丢失数据
        ConcurrentQueue<byte[]> byteQueue = new ConcurrentQueue<byte[]>();//收到的数据全存这里

        public delegate void SendSerialDataHandle(byte[] bs);
        public event SendSerialDataHandle SendSerialDataEvent;

        public delegate void ReceiveSerialDataHandle(BaseSericalCmdResult data);
        public event ReceiveSerialDataHandle ReceiveSerialDataEvent;

        public delegate void OpenSerialHandle(string com);
        public event OpenSerialHandle OpenSerialEvent;

        public delegate void CloseSerialHandle();
        public event CloseSerialHandle CloseSerialEvent;

        public bool IsConnected
        {
            get
            {
                if (serialPort == null)
                    return false;
                return serialPort.IsOpen;
            }
        }

        public void OpenSeriesPort(string com)
        {
            if (serialPort == null)
            {
                serialPort = new SerialPort();
                serialPort.PortName = com;
                serialPort.BaudRate = BaudRate;
                serialPort.ReadTimeout = ReadTimeout;
                serialPort.WriteTimeout = WriteTimeout;
            }

            if (!serialPort.IsOpen)
            {
                serialPort.Open();
                if (!serialPort.IsOpen)
                {
                    return;
                }

                //CmdNum = 1;
                byteQueue = new ConcurrentQueue<byte[]>();
                lostbts = null;
                OpenSerialEvent?.Invoke(com);
            }

            serialPort.DataReceived -= SerialDataReceived;
            serialPort.DataReceived += SerialDataReceived;

            Task.Factory.StartNew(() =>
            { 
                while (IsConnected)
                {
                    DataPaly();
                }
            });
        }


        /// <summary>
        /// 关闭串口
        /// </summary>
        /// <returns></returns>
        public void CloseSeriesPort()
        {
            if (serialPort == null || !serialPort.IsOpen)
                return;

            lostbts = null;
            try
            {
                if (serialPort != null && serialPort.IsOpen)
                {
                    serialPort.DataReceived -= SerialDataReceived;
                    serialPort.Close();
                    CloseSerialEvent?.Invoke();
                }
            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }
        }

        private void SerialDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                SerialPort comm = sender as SerialPort;
                int n = comm.BytesToRead;
                byte[] buf = new byte[n];
                comm.Read(buf, 0, n);
                byteQueue.Enqueue(buf);

            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }
        }

        private void DataPaly()
        {
            if (byteQueue.Count == 0)
                return;

            byte[] buf;
            bool b = byteQueue.TryDequeue(out buf);
            if (!b)
                return;

            byte[] Newbts;
            if (lostbts != null)
            {
                Newbts = new byte[lostbts.Length + buf.Length];
                Array.Copy(lostbts, Newbts, lostbts.Length);
                Array.Copy(buf, 0, Newbts, lostbts.Length, buf.Length);
                lostbts = null;
            }
            else
            {
                Newbts = new byte[buf.Length];
                Array.Copy(buf, Newbts, buf.Length);
            }
            //协议读出，判断完整性
            int lastIndex;
            for (int i = 0; i < Newbts.Length; i++)
            {
                if (Newbts[i] == this.Head)//开头
                {
                    if (Newbts.Length >= i + 3)
                    {
                        var len_data = Newbts[i + 1];
                        int len_check = 0;
                        int len_full = len_data + 3;
                        if (NeedVerify)
                        {
                            len_check = 1;
                            len_full= len_data + 4;
                        }
                        lastIndex = (i + 1 + len_data + len_check + 1);//依据:  head+len+data+check+tail
                        if (Newbts.Length > lastIndex && Newbts[lastIndex] == this.Tail)
                        {
                            byte[] fullBytes = new byte[len_full];//完整报文len              
                            Array.Copy(Newbts, i, fullBytes, 0, fullBytes.Length);
                            i += fullBytes.Length - 1;

                            var check1 = ByteCalcHelper.CalcXOr(fullBytes, 1, 1 + len_data);
                            var check2 = fullBytes[fullBytes.Length - 2];
                            if (check1 == check2)
                            {
                                var r = new BaseSericalCmdResult() { Bytes = fullBytes };
                                ReceiveSerialDataEvent?.Invoke(r);
                            }
                        }
                        else
                        {
                            lostbts = new byte[Newbts.Length - i];
                            Array.Copy(Newbts, i, lostbts, 0, Newbts.Length - i);
                            break;
                        }
                    }
                    else
                    {
                        lostbts = new byte[Newbts.Length - i];
                        Array.Copy(Newbts, i, lostbts, 0, Newbts.Length - i);
                        break;
                    }
                }
            }
        }

       
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public bool SendMessage(byte[] bytes)
        {
            try
            {
                if (!serialPort.IsOpen)
                    return false;

                serialPort.Write(bytes, 0, bytes.Length);
                SendSerialDataEvent?.Invoke(bytes);
                return true;
            }
            catch (Exception ee)
            {
                return false;
            }
        }

        public bool SendMessage(string hexCmd)
        {
            string hexCmd2 = hexCmd.Replace(" ", "");
            var bs = ByteCalcHelper.StrToByteArray(hexCmd2);
            bool b = SendMessage(bs);
            return b;
        }


    }


}
