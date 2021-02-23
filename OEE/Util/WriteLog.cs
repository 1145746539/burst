using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


namespace OEE.Util
{
    /// <summary>
    /// 日志记录的工具类
    /// </summary>
    class WriteLog
    {
        /// <summary>
        /// 日志保存路径
        /// </summary>
        private readonly static string filePath = @"D:\equipment\log";

        /// <summary>
        /// 运行日志
        /// </summary>
        private readonly static string fPath = @"D:\equipment\running";
        /// <summary>
        /// 日志名
        /// </summary>
        private static string fileName;

        /// <summary>
        /// 写入日志：path可null
        /// </summary>
        /// <param name="path"> 路径为空时候，写入默认路径</param>
        /// <param name="logInfo"> 日志信息</param>
        public static void WriteOrCreateLog(string path, string logInfo)
        {
            
            try
            {
                DateTime dt = DateTime.Now;
                fileName = dt.ToString("yyyyMMdd") + ".txt";
                string filePathEnd = path ?? fPath;  //path为空时返回filePath , 不为空返回本身
                if (!Directory.Exists(filePathEnd))
                {
                    Directory.CreateDirectory(filePathEnd);
                }
                String log = filePathEnd + "\\" + fileName;
                using(StreamWriter textWriter = new StreamWriter(log, true))
                {
                    textWriter.WriteLine(dt.ToString("yyyy-MM-dd HH:mm:ss") + "\t" + logInfo);
                }
                
            }
            catch (Exception ex)
            {
                ex.Message.ToString();

            }

        }

     
    }

}
