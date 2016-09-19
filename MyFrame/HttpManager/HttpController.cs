using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyFrame
{
    public class HttpController
    {
        /// <summary>
        /// 使用 httphelper类通过url获取html
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string GetHtmlByHttpHelper(string url)
        {
            try
            {
                //创建Httphelper对象
                HttpHelper http = new HttpHelper();
                //创建Httphelper参数对象
                HttpItem item = new HttpItem()
                {
                    URL = url,//URL     必需项    
                    Method = "get",//URL     可选项 默认为Get   
                    ContentType = "text/html",//返回类型    可选项有默认值   
                    // ProxyIp = iplist[index],
                    Allowautoredirect = true

                    //ContentType = "application/x-www-form-urlencoded",//返回类型    可选项有默认值   
                };
                //请求的返回值对象
                HttpResult result = http.GetHtml(item);
                //获取请请求的Html
                string html = result.Html;
                return html;
            }
            catch (Exception ex)
            {
               
                return "";
            }
        }
    }
}