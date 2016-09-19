using MyFrame.Filter;
using MyFrame.LogManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace MyFrame.Common
{
    public class MessageQueueConfig
    {
        public static void RegisterExceptionLogQueue()
        {
            //从队列中读取错误信息写日志
            ThreadPool.QueueUserWorkItem(o =>
            {
                while (true)
                {
                    try
                    {
                        if (ExceptionHandleAttribute.ExceptionQueue.Count > 0)
                        {
                            Exception ex = ExceptionHandleAttribute.ExceptionQueue.Dequeue();
                            if(ex!=null)
                            {
                                //写日志
                                Log log = Log.CreateInstance();
                                log.WriteLog(ex.Message.ToString());
                            }
                        }
                        else
                        {
                            //
                            Thread.Sleep(1000);
                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionHandleAttribute.ExceptionQueue.Enqueue(ex);
                    }
                }
            },"");
        }
    }
}