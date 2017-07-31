//  NetTool.cs
//  Nilsen
//  2015-04-09

//#define NetOrder
using System;
using System.Net.Sockets;
using System.Net;

namespace NetWrapper.Network.Tool
{
    /// <summary>
    /// 日志类型
    /// </summary>
    public enum LogerType
    {
        OFF = -1,
        ERROR,
        WARN,
        INFO,
        DEBUG,
    };

    /// <summary>
    /// log的回调函数
    /// </summary>
    /// <param name="type">log类型</param>
    /// <param name="format">信息</param>
    public delegate void LogCallBackFunc(LogerType type, string format);

    /// <summary>
    /// 写文件类
    /// </summary>
    public class WriteFiles
    {
        /// <summary>
        /// 唯一的实例
        /// </summary>
        static readonly WriteFiles sInstance = new WriteFiles();

        /// <summary>
        /// log回调函数
        /// </summary>
        public LogCallBackFunc LogCallBack;

        public LogerType Level;

        // = LogCallBackFunc;
        //public static void LogCallBackFunc(LogerType type, string format)
        //{
        //    UnityEngine.Debug.Log("LogerType: " + type + " " + format);
        //}

        static public WriteFiles WritFile
        {
            get { return sInstance; }
        }

        public WriteFiles()
        {
            LogCallBack = UnityLog;
            Level = LogerType.INFO;
        }

        public void UseConsoleLog()
        {
            LogCallBack = ConsoleLog;
        }

        /// <summary>
        /// 输出log
        /// </summary>
        /// <param name="type">log类型</param>
        /// <param name="format">信息</param>
        public void Log(LogerType type, string format)
        {
            //Directory.CreateDirectory(@"Log");
            //FileStream fs = new FileStream(@"Log\Log.txt", FileMode.Append, FileAccess.Write, FileShare.Write);
            //StreamWriter streamWriter = new StreamWriter(fs);
            //streamWriter.WriteLine("[" + DateTime.Now.ToString() + "]" + ":" + format);
            //streamWriter.Flush();
            //streamWriter.Close();
            //fs.Close();
            if (type > Level)
                return;

            if (this.LogCallBack != null)
                LogCallBack(type, "[" + DateTime.Now.ToString() + "]:" + format);
        }

        public void Log(Exception e)
        {
            Log(LogerType.ERROR, e.ToString());
        }

        private void UnityLog(LogerType type, string format)
        {
            switch (type)
            {
                case LogerType.WARN:
                    {
                        UnityEngine.Debug.LogWarning("[" + DateTime.Now.ToString() + "]:" + format);
                        break;
                    }
                case LogerType.ERROR:
                    {
                        UnityEngine.Debug.LogError("[" + DateTime.Now.ToString() + "]:" + format);
                        break;
                    }
                default:
                    {
                        UnityEngine.Debug.Log("[" + DateTime.Now.ToString() + "]:" + format);
                        break;
                    }
            }
        }

        private void ConsoleLog(LogerType type, string format)
        {
            switch (type)
            {
                case LogerType.WARN:
                    {
                        Console.WriteLine("[" + DateTime.Now.ToString() + "] [warn]:" + format);
                        break;
                    }
                case LogerType.ERROR:
                    {
                        Console.WriteLine("[" + DateTime.Now.ToString() + "] [error]:" + format);
                        break;
                    }
                default:
                    {
                        Console.WriteLine("[" + DateTime.Now.ToString() + "] [info]:" + format);
                        break;
                    }
            }
        }
    }

    public class NetUtil
    {
        static public IPAddress GetIPAddress(string address)
        {
            IPAddress ipAddress = null;
            bool flag = IPAddress.TryParse(address, out ipAddress);
            IPAddress result;
            if (flag)
            {
                result = ipAddress;
            }
            else
            {
                UnityEngine.Debug.Log("org address: " + address);
                IPHostEntry hostEntry = Dns.GetHostEntry(address);
                IPAddress[] addressList = hostEntry.AddressList;
                IPAddress[] array = addressList;
                for (int i = 0; i < array.Length; i++)
                {
                    IPAddress iPAddress2 = array[i];
                    UnityEngine.Debug.Log("AddressFamily: " + iPAddress2.AddressFamily.ToString());
                    UnityEngine.Debug.Log("Address: " + iPAddress2.ToString());
                    bool flag2 = iPAddress2.AddressFamily == AddressFamily.InterNetworkV6;
                    if (flag2)
                    {
                        result = iPAddress2;
                        return result;
                    }
                    bool flag3 = ipAddress == null && iPAddress2.AddressFamily == AddressFamily.InterNetwork;
                    if (flag3)
                    {
                        ipAddress = iPAddress2;
                    }
                }
                result = ipAddress;
            }
            return result;
        }
        static public int NetworkToHostOrder(int data)
        {
#if NetOrder
            return System.Net.IPAddress.NetworkToHostOrder(data);
#else
            return data;
#endif
        }
        static public int HostToNetworkOrder(int data)
        {
#if NetOrder
            return System.Net.IPAddress.HostToNetworkOrder(data); ;
#else
            return data;
#endif
        }
    }
}
