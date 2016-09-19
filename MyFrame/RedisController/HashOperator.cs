
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyFrame.RedisControl
{
    public class HashOperator : RedisOperatorBase
    {
        public HashOperator() : base() { }

         /// <summary>
          /// 判断某个数据是否已经被缓存
          /// </summary>
          public bool Exist<T>(string hashId, string key)
          {
              return Redis.HashContainsEntry(hashId, key);
          }
        /// <summary>
          /// 存储数据到hash表
          /// </summary>
          public bool Set<T>(string hashId, string key, T t)
          {
              var value = JsonSerializer.SerializeToString<T>(t);
              return Redis.SetEntryInHash(hashId, key, value);
          }
        /// <summary>
        /// 设置 键值并设置过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expireIn"></param>
          public void SetKeyValue(string key, string value, TimeSpan expireIn)
          {
             Redis.SetValue(key,value,expireIn);
          }
         /// <summary>
          /// 移除hash中的某值
          /// </summary>
          public bool Remove(string hashId, string key)
          {
              return Redis.RemoveEntryFromHash(hashId, key);
          }
         /// <summary>
         /// 移除整个hash
          /// </summary>
          public bool Remove(string key)
          {
              return Redis.Remove(key);
          }
          /// <summary>
          /// 从hash表获取数据
          /// </summary>
          public T Get<T>(string hashId, string key)
          {
              string value = Redis.GetValueFromHash(hashId, key);
              return JsonSerializer.DeserializeFromString<T>(value);
          }
        /// <summary>
        /// 根据键获取值
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public string  GetValueByKey(string key)
        {
            var itm = Redis.GetValue(key);
            return itm;
        }
           /// <summary>
         /// 获取整个hash的数据
         /// </summary>
         public List<T> GetAll<T>(string hashId)
         {
             var result = new List<T>();
             var list = Redis.GetHashValues(hashId);
             if (list != null && list.Count > 0)
             {
                 list.ForEach(x =>
                 {
                     var value = JsonSerializer.DeserializeFromString<T>(x);
                     result.Add(value);
                 });
             }
             return result;
         }
         /// <summary>
         /// 设置缓存过期
         /// </summary>
         public bool SetExpire(string key, DateTime datetime)
         {
            if(Redis.ExpireEntryAt(key, datetime))
            {
                return true; 
            }
            else { return false; }
         }
         /// <summary>
         /// 设置缓存过期时间段内有效
         /// </summary>
         public bool SetExpireIn(string key )
         {
             try
             {
                 if (Redis.ExpireEntryIn(key, DateTime.Now.AddSeconds(15) - DateTime.Now))
                 {
                     return true;
                 }
                 else { return false; }
             }
             catch (Exception ex)
             {
                 return false;
             }
         }
    }
}