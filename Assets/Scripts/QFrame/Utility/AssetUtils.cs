using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// 处理资源辅助类
/// </summary>

namespace QFrame
{
    public static class AssetUtils
    {
        private static List<string> searchPaths = new List<string>();
        public static List<string> SearchPaths
        {
            get { return new List<string>(searchPaths); }
        }

        /// <summary>
        /// 添加文件查找路径
        /// </summary>
        /// <param name="path"></param>
        /// <param name="front"></param>
        /// <returns></returns>
        public static bool AddSearchPath(string path, bool front = false)
        {
            var index = searchPaths.IndexOf(path);

            if (index >= 0)
            {
                return false;
            }

            if (front)
            {
                searchPaths.Insert(0, path);
            }
            else
            {
                searchPaths.Add(path);
            }
            return true;
        }

        /// <summary>
        /// 清空Lua查找路径
        /// </summary>
        public static void ClearSearchPath()
        {
            searchPaths.Clear();
        }

        /// <summary>
        /// 移除指定查找路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool RemoveSearchPath(string path)
        {
            var index = searchPaths.IndexOf(path);

            if (index >= 0)
            {
                searchPaths.RemoveAt(index);
                return true;
            }

            return false;
        }

        /// <summary>
        /// 查找文件并获取绝对路径
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string FindFile(string fileName)
        {
            if (fileName == string.Empty)
            {
                return string.Empty;
            }

            string fullPath = string.Empty;
            for (var i = 0; i < searchPaths.Count; i++)
            {
                fullPath = searchPaths[i].Replace("?", fileName);
                if (File.Exists(fullPath) || Directory.Exists(fullPath))
                {
                    return fullPath;
                }
            }
            return string.Empty;
        }

    }
}

