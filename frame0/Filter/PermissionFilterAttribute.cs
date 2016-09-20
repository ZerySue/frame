using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace frame0.Filter
{
    /// <summary>
    /// 用户权限验证
    /// </summary>
    public class PermissionFilterAttribute:AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            //是否登陆 
            bool isLogin = HttpContext.Current.User.Identity.IsAuthenticated;//form表单认真状态
            //是否有页面权限
            // TODO
            if (!isLogin)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 获取当前访问地址
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            string controllerNama = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            string actionName = filterContext.ActionDescriptor.ActionName;
            ///获取当前访问页面 判断是否有权限
            base.OnAuthorization(filterContext);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            string cName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;

            string loaclPath = HttpContext.Current.Request.Url.LocalPath.ToLower();
            if(loaclPath!=("/Login/Index").ToLower())
            {

            }
            if (cName.ToLower() == "Login")
            {
                filterContext.Result = new RedirectResult("/Login/Index");
            }
            else
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
            
        }

    }
}