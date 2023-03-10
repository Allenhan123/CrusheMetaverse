using System;
using QFrame;

namespace QFrame
{

    /// <summary>
    /// JSON 相关的实用函数。
    /// </summary>
    public static partial class Json
    {
        private static IJsonHelper s_JsonHelper = new JsonHelper();

        /// <summary>
        /// 设置 JSON 辅助器。
        /// </summary>
        /// <param name="jsonHelper">要设置的 JSON 辅助器。</param>
        public static void SetJsonHelper(IJsonHelper jsonHelper)
        {
            s_JsonHelper = jsonHelper;
        }

        /// <summary>
        /// 将对象序列化为 JSON 字符串。
        /// </summary>
        /// <param name="obj">要序列化的对象。</param>
        /// <param name="prettyPrint">是否格式化输出。</param>
        /// <returns>序列化后的 JSON 字符串。</returns>
        public static string ToJson(object obj)
        {
            if (s_JsonHelper == null)
            {
                throw new SystemException("JSON helper is invalid.");
            }

            return s_JsonHelper.ToJson(obj, false);
        }


        /// <summary>
        /// 将 JSON 字符串反序列化为对象。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="json">要反序列化的 JSON 字符串。</param>
        /// <returns>反序列化后的对象。</returns>
        public static T ToObject<T>(string json)
        {
            if (s_JsonHelper == null)
            {
                throw new SystemException("JSON helper is invalid.");
            }

            return s_JsonHelper.ToObject<T>(json);
        }

        /// <summary>
        /// 将 JSON 字符串反序列化为对象。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <param name="json">要反序列化的 JSON 字符串。</param>
        /// <returns>反序列化后的对象。</returns>
        public static object ToObject(Type objectType, string json)
        {
            if (s_JsonHelper == null)
            {
                throw new SystemException("JSON helper is invalid.");
            }

            if (objectType == null)
            {
                throw new SystemException("Object type is invalid.");
            }

            return s_JsonHelper.ToObject(objectType, json);
        }

    }

}
