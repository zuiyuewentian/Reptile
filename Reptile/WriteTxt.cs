using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Reptile
{
    public class WriteTxt
    {
        /// <summary>
        /// 写入Txt文件
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="content">内容</param>
        public static void WriteNewTxt(string fileName, string content)
        {
            string basePath = System.AppDomain.CurrentDomain.BaseDirectory + fileName + ".txt";
            FileStream fs = null;
            if (File.Exists(basePath))
            {
                fs = new FileStream(basePath, FileMode.Append);
            }
            else
            {
                fs = new FileStream(basePath, FileMode.Create);
            }

            StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
            sw.WriteLine(content);
            sw.Close();
            fs.Close();
        }
    }
}
