
using MyFrame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyFrame
{
    public class UserManager : IAUDRManager, IUserManager
    {
        public List<object> GetDaga(object dataInfo)
        {  
            List<object> list=new List<object>();
            var info = (UserModel)dataInfo;
            UserModel userInfo=new UserModel();
            userInfo.Account="510669408@qq.com";
            list.Add(userInfo);
            return list;
        }
        public bool AddData(object logInfo)
        {
            return false;
        }
        public bool UpdateData(object userInfo)
        {
            return false;
        }
        public bool DeleteData(object userInfo)
        {
            return false;
        }
    }
}