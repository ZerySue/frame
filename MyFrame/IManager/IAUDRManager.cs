using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyFrame.Manager
{
    public interface IAUDRManager
    {    /// <summary>
        /// 增删改查的统一接口
        /// </summary>

        List<object> GetDaga(object dataInfo);
        bool AddData(object logInfo);
        bool UpdateData(object dataInfo);
        bool DeleteData(object dataInfo);

    }
}
