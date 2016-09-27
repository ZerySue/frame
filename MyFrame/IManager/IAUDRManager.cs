using MyFrame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyFrame
{ 
    /// <summary>
    /// 增删改查的统一接口
    /// </summary>
    public interface IAUDRManager
    {
        List<object> GetDaga(object dataInfo);
        bool AddData(object logInfo);
        bool UpdateData(object dataInfo);
        bool DeleteData(object dataInfo);
    }
}