using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace frame0.LogManager
{
    /// <summary>
    /// 单例模式 写日志 
    /// </summary>
    public class Log
    {
        private volatile static Log _instance = null;
        private static readonly object _lockHelper = new object();
        private Log()
        {
        }
        /// <summary>
        /// 避免资源争夺  单例模式
        /// </summary>
        /// <returns></returns>
        public static Log CreateInstance()
        {
            if (_instance == null)
            {
                lock (_lockHelper)
                {
                    if (_instance == null)
                    {
                        _instance = new Log();
                    }
                }
            }
            return _instance;
        }
        public void WriteLog(string txt)
        {
            try
            {
                string path = HttpRuntime.AppDomainAppPath.ToString() + @"\log\" + DateTime.Now.ToString("yyyy-MM-dd") + @"\";

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);

                }
                path += DateTime.Now.ToString("yyyyMMdd") + "-" + DateTime.Now.ToString("HH") + ".txt";

                if (!File.Exists(path))
                {
                    using (FileStream fscreate = new FileStream(path, FileMode.Create))
                    {
                        fscreate.Close();
                    }
                }
                using (FileStream fs = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                {
                    using (StreamWriter sw = new StreamWriter(fs, Encoding.Default))
                    {
                        sw.Write(DateTime.Now.ToString("HH:mm:ss") + " " + txt + "\r\n");
                        sw.Close();
                    }
                    fs.Close();
                }
            }

            catch (Exception ex)
            {
                WriteLog("程序发生异常（WriteLog）。详情：" + ex.Message);

            }

        }
    }
}