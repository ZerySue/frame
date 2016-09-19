using MyFrame.LogManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Script.Serialization;

namespace MyFrame.Helper
{
    /// <summary>
    /// 通用方法
    /// </summary>
    public class DataManager
    {
        /// <summary>
        /// 将对象序列化成json  
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public  string ModelToJson(Object obj)
        {
            String str = "";
            if (obj is String || obj is Char)
            {
                str = obj.ToString();
            }
            else
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                str = serializer.Serialize(obj);
            }
            return str;
        }
        /// <summary>
        /// json反序列成对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strJson"></param>
        /// <returns></returns>
        public T ScriptDeserialize<T>(string strJson)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Deserialize<T>(strJson);
        }
        /// <summary>
        /// 反序列化成数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strJson"></param>
        /// <returns></returns>
        public List<T> JSONStringToList<T>(string strJson)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<T> objList = serializer.Deserialize<List<T>>(strJson);
            return objList;
        }
        /// <summary>
        /// 反序列化成datatable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strJson"></param>
        /// <returns></returns>
        public DataTable JSONStringToDataTable<T>(string strJson)
        {
            DataTable dt = new DataTable();
            if (strJson.IndexOf("[") > -1)//如果大于则strJson存放了多个model对象
            {
                strJson = strJson.Remove(strJson.Length - 1, 1).Remove(0, 1).Replace("},{", "};{");
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string[] items = strJson.Split(';');

            foreach (PropertyInfo property in typeof(T).GetProperties())//通过反射获得T类型的所有属性
            {
                DataColumn col = new DataColumn(property.Name, property.PropertyType);
                dt.Columns.Add(col);
            }
            //循环 一个一个的反序列化
            for (int i = 0; i < items.Length; i++)
            {
                DataRow dr = dt.NewRow();
                //反序列化为一个T类型对象
                T temp = serializer.Deserialize<T>(items[i]);
                foreach (PropertyInfo property in typeof(T).GetProperties())
                {
                    dr[property.Name] = property.GetValue(temp, null);
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }
        /// <summary>
        /// 获取枚举值上的Description特性的说明
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="obj">枚举值</param>
        /// <returns>特性的说明</returns>
        public static string GetEnumDescription<T>(T obj)
        {
            try
            {
                var type = obj.GetType();
                FieldInfo field = type.GetField(Enum.GetName(type, obj));
                DescriptionAttribute descAttr = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (descAttr == null)
                {
                    return string.Empty;
                }

                return descAttr.Description;
            }
            catch(Exception ex)
            {
                Log log = Log.CreateInstance();
                log.WriteLog(ex.Message.ToString());
                return "";
            }
        }
        /// <summary>
        /// 泛型 通过反射跟据描述信息 实现 实体转换  T中属性描述信息跟Mname一致 则赋值
        /// </summary>
        /// <typeparam name="T">带有描述信息的实体对象</typeparam>
        /// <typeparam name="M"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public List<M> ObjToObj<T,M>(List<T> list) where M:new() 
        {
            try
            {
                var plist = new List<M>();
                PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);
                PropertyInfo[] propertiesP = typeof(M).GetProperties(BindingFlags.Instance | BindingFlags.Public);
                if (properties.Length < 0)
                {
                    return null;
                }
                for (int index = 0; index < list.Count; index++)
                {
                    M secondProduct = new M();
                    foreach (PropertyInfo item in properties)
                    {
                        var descriptionObj = ((DescriptionAttribute)Attribute.GetCustomAttribute(item, typeof(DescriptionAttribute)));
                        if (descriptionObj == null)
                        {
                            continue;
                        }
                        var des = descriptionObj.Description;
                        foreach (PropertyInfo p in propertiesP)
                        {
                            if (p.Name.ToString() == des)
                            {
                                p.SetValue(secondProduct, item.GetValue(list[index], null));
                                break;
                            }
                        }
                    }
                    plist.Add(secondProduct);
                }
                return plist;
            }
            catch (Exception ex)
            {
              //  WriteLog("ConvertProcedureModelToSecondKillProduct:" + ex.Message.ToString());
                Log log = Log.CreateInstance();
                log.WriteLog(ex.ToString());
                return null;
            }
        }
        /// <summary>
        /// datatable转成model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public  IList<T> TableToModel<T>(DataTable dt) where T:new () 
        {
            // 定义集合    
            IList<T> ts = new List<T>();

            // 获得此模型的类型   
            Type type = typeof(T);
            string tempName = "";

            foreach (DataRow dr in dt.Rows)
            {
                T t = new T();
                // 获得此模型的公共属性      
                PropertyInfo[] propertys = t.GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    tempName = pi.Name;  // 检查DataTable是否包含此列    

                    if (dt.Columns.Contains(tempName))
                    {
                        // 判断此属性是否有Setter      
                        if (!pi.CanWrite) continue;

                        object value = dr[tempName];
                        if (value != DBNull.Value)
                            pi.SetValue(t, value, null);
                    }
                }
                ts.Add(t);
            }
            return ts;
        }     

       
    }
    public static class ListToTable
    {
        /// <summary>
        /// list转换成table
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static DataTable ListToDataTable<T>(this IList<T> data, string tableName)
        {
            DataTable table = new DataTable(tableName);
            try
            {
                //special handling for value types and string
                if (typeof(T).IsValueType || typeof(T).Equals(typeof(string)))
                {

                    DataColumn dc = new DataColumn("Value");
                    table.Columns.Add(dc);
                    foreach (T item in data)
                    {
                        DataRow dr = table.NewRow();
                        dr[0] = item;
                        table.Rows.Add(dr);
                    }
                }
                else
                {
                    PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
                    foreach (PropertyDescriptor prop in properties)
                    {
                        table.Columns.Add(prop.Name,
                        Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                    }
                    foreach (T item in data)
                    {
                        DataRow row = table.NewRow();
                        foreach (PropertyDescriptor prop in properties)
                        {
                            try
                            {
                                row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                            }
                            catch (Exception ex)
                            {
                                row[prop.Name] = DBNull.Value;
                            }
                        }
                        table.Rows.Add(row);
                    }
                }
                return table;
            }
            catch (Exception ex)
            {
                Log log = Log.CreateInstance();
                log.WriteLog(ex.Message.ToString());
                return null;
            }
        }
    }
}