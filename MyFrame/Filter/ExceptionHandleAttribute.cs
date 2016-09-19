using MyFrame.LogManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;

namespace MyFrame.Filter
{
    /// <summary>
    /// 异常过滤器
    /// </summary>
    public class ExceptionHandleAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Response == null)
            {
                Log log = Log.CreateInstance();
                log.WriteLog(actionExecutedContext.Exception.ToString());
            }
            base.OnException(actionExecutedContext);
        }
    }
}