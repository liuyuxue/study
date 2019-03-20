
using CommonLib;
using CommonLib.Tools;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetLib
{
    //当串口数据发送不频繁,发送间隔较大时,采用本串口类.
    //算法依据: 多数串口协议都是 head+len+data+check?+tail,其中除data位以外,其他位长度都是一个byte
    //若协议不遵循此格式,可重写串口类,但本思路仍可借鉴
    public class BaseSerialDriver2
    {

        #region 
        int BaudRate = 57600;
        int ReadTimeout = 500;
        int WriteTimeout = 1000;
        int RetrySendTimes = 3;
        byte Head;//包头   
        byte Tail;  //包尾 = 0x03;        
        bool NeedVerify;//是否有校验位
        int SendWaitTime = 111;//发送后的短暂间隔时间,然后开始接收

        static object locker_send = new object();
        SerialPort serialPort;
        #endregion

        //public delegate void SendSerialDataHandle(byte[] bs);
        //public event SendSerialDataHandle SendSerialDataEvent;

        //public delegate void ReceiveSerialDataHandle(BaseSericalCmdResult data);
        //public event ReceiveSerialDataHandle ReceiveSerialDataEvent;

        //public delegate void OpenSerialHandle(string com);
        //public event OpenSerialHandle OpenSerialEvent;

        //public delegate void CloseSerialHandle();
        //public event CloseSerialHandle CloseSerialEvent;

        //public delegate void SerialDataOutTimeHandle(byte[] bs_send);
        //public event SerialDataOutTimeHandle SerialDataOutTimeEvent;

        public bool IsConnected
        {
            get
            {
                if (serialPort == null)
                    return false;
                return serialPort.IsOpen;
            }
        }

        public void InitPara(int BaudRate, int ReadTimeout, int WriteTimeout, byte Head, byte Tail, bool NeedVerify)
        {
            this.BaudRate = BaudRate;
            this.ReadTimeout = ReadTimeout;
            this.WriteTimeout = WriteTimeout;

            this.Head = Head;
            this.Tail = Tail;
            this.NeedVerify = NeedVerify;
        }

        public bool OpenSeriesPort(string com)
        {
            if (serialPort == null)
            {
                serialPort = new SerialPort();
                serialPort.ReadTimeout = ReadTimeout;
                serialPort.WriteTimeout = WriteTimeout;
                serialPort.BaudRate = BaudRate;
            }
            serialPort.PortName = com;//这玩意会变,所以不能放上面的括号里

            if (!serialPort.IsOpen)
            {
                serialPort.Open();
                if (!serialPort.IsOpen)
                {
                    return false;
                }   
            }
            return true;
        }


        public bool CloseSeriesPort()
        {
            if (serialPort == null || !serialPort.IsOpen)
                return false;

            if (serialPort != null && serialPort.IsOpen)
            {
                serialPort.Close();
             //   CloseSerialEvent?.Invoke();
            }
            return true;
        }

        /// <summary>
        /// 有些特殊协议,在收到数据后,需要过滤掉转义符 
        /// </summary>
        protected virtual byte[] RemoveEscapeCharacter(byte[] bytes)
        {
            return bytes;
        }

        /// <summary>
        /// 有些特殊协议,在发送数据前,要求加上转义符
        /// </summary>
        protected virtual byte[] AddEscapeCharacter(byte[] bytes_send)
        {
            return bytes_send;
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        public BaseSericalCmdResult Send(byte[] bytes_send)
        {           
            if (!serialPort.IsOpen)
                return null;

            var bytes_send2 = AddEscapeCharacter(bytes_send);
         // string test_hex = SerialCalc.BytesToString(bytes_send2, 16);//这行code是为测试提供方便
            for (int i = 0; i < RetrySendTimes; i++)
            {
                serialPort.DiscardInBuffer();
                serialPort.DiscardOutBuffer();
                serialPort.Write(bytes_send2, 0, bytes_send2.Length);
               // SendSerialDataEvent?.Invoke(bytes_send2);
                System.Threading.Thread.Sleep(SendWaitTime);
                try
                {
                    int readLength = serialPort.BytesToRead;
                    byte[] reces = new byte[readLength];
                    serialPort.Read(reces, 0, readLength);

                    var reces2 = RemoveEscapeCharacter(reces);
                    BaseSericalCmdResult r = new BaseSericalCmdResult() { Bytes = reces2 };
                    
                    return r;
                }
                catch (Exception ex)// 读超时触发异常
                {
                    if (i >= 2)
                        return null;
                    else
                        continue;
                }
               
            }
            return null;
       
        }


        public BaseSericalCmdResult SendMessage(string hexCmd)
        {
            string hexCmd2 = hexCmd.Replace(" ", "");
            var bs = ByteCalcHelper.StrToByteArray(hexCmd2);
            return SendMessage(bs);
        }

        public BaseSericalCmdResult SendMessage(byte[] bytes_send)
        {
            lock (locker_send)
            {
                return Send(bytes_send);
            }
        }


    }
}
