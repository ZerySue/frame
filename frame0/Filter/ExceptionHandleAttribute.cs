using frame0.LogManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Mvc;

namespace frame0.Filter
{
    /// <summary>
    /// 异常过滤器
    /// </summary>
    public class ExceptionHandleAttribute : HandleErrorAttribute
    {
        public static Queue<Exception> ExceptionQueue = new Queue<Exception>();
      
        public override void OnException(ExceptionContext actionExecutedContext)
        {

            //将异常信息入队
            ExceptionQueue.Enqueue(actionExecutedContext.Exception);
            //
            //Log log = Log.CreateInstance();
            //log.WriteLog(actionExecutedContext.Exception.ToString());
           // actionExecutedContext.HttpContext.Response.Redirect("~/Admin/CommonError.html");

            base.OnException(actionExecutedContext);
        }
    }
}