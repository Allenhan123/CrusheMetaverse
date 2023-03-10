using System.IO;
using UnityEngine;


namespace Utils
{
    public static class FileUtility
    {
        public static bool WriteText(string path, string content)
        {
            bool result = true;
            try
            {
                CreateFolder(path);
                using (FileStream stream = File.Create(path))
                {
                    var bytes = System.Text.Encoding.UTF8.GetBytes(content);
                    stream.Write(bytes, 0, bytes.Length);
                    stream.Flush();
                }
            }
            catch (System.Exception e)
            {
                Debug.Log("FileUtility.WriteText() Write text exception : " + e.Message);
                result = false;
            }

            return result;
        }


        public static bool WriteBytes(string path, byte[] content)
        {
            bool result = true;
            try
            {
                CreateFolder(path);
                using (FileStream stream = File.Create(path))
                {
                    stream.Write(content, 0, content.Length);
                    stream.Flush();
                }
            }
            catch (System.Exception e)
            {
                Debug.Log("FileUtility.WriteText() Write text exception : " + e.Message);
                result = false;
            }

            return result;
        }

        public static string CreateFolder(string path)
        {
            string dir = Path.GetDirectoryName(path);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            return dir;
        }

        /// <summary>
        /// �ļ��ж�ȡ�ı�
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string ReadText(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return string.Empty;
            }


            FileStream fs = null;
            StreamReader sr = null;
            string result = string.Empty;

            try
            {
                fs = new FileStream(filePath, FileMode.Open);
                sr = new StreamReader(fs);

                result = sr.ReadToEnd();
            }
            catch (System.Exception e)
            {
                Debug.LogErrorFormat("read file exception => {0}", e.ToString());
            }

            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }

                if (sr != null)
                {
                    sr.Close();
                }
            }

            return result;
        }

        public static byte[] ReadBytes(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return null;
            }

            FileStream fs = null;
            BinaryReader binaryReader = null;
            byte[] bytes = null;

            try
            {
                fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                binaryReader = new BinaryReader(fs);

                bytes = binaryReader.ReadBytes((int)fs.Length);
            }
            catch(System.Exception e)
            {
                Debug.LogErrorFormat("read file exception => {0}", e.ToString());
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }

                if (binaryReader != null)
                {
                    binaryReader.Close();
                }
            }


            return bytes;
        }

        /// <summary>
        /// �ļ��Ƿ����
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool Exists(string filePath)
        {
            return File.Exists(filePath);
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="filePath"></param>
        public static void DeleteFile(string filePath)
        {
            File.Delete(filePath);
        }
    }
}
