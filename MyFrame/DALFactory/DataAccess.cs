using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyFrame.DALFactory
{
    /// <summary>
    /// 抽象工厂模式创建DAL-(利用工厂模式+泛型机制+反射机制+缓存机制,实现动态创建不同的数据层对象接口) 。
    /// 可以在这个DataAccess类里创建所有DAL类
    /// </summary>
    public sealed class DataAccess
    {
        private static readonly string path = "MyFrame";//ConfigurationManager.AppSettings["DAL"];
        /// <summary>
        /// 采用泛型创建对象或从缓存中获取
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <returns></returns>
        public static T CreateObject<T>()
        {
            //此处采用XML，把对应关系做好更灵活
            string typeName = "." + typeof(T).Name;
            object obj;
            //从缓存中读取数据
            obj = DataCache.GetCache(typeName);
            if (obj == null)
            {
                //反射创建 
                obj = Assembly.Load(path).CreateInstance(path + typeName);
                DataCache.SetCache(typeName, obj);
            }
            return (T)obj;
        }
    }
}
