using Ivony.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Reptile
{
    public class JumonyHelper
    {
        private Ivony.Html.Parser.JumonyParser jumony;

        public IHtmlDocument doc;

        public JumonyHelper(string url)
        {
            jumony = new Ivony.Html.Parser.JumonyParser();
            byte[] downData;
            try
            {
                WebClient web = new WebClient();
               // web.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/27.0.1453.94 Safari/537.36");
                // todo  代理
                downData = web.DownloadData(url);
            }
            catch (Exception ex)
            {
                return;
            }
            //处理网站的编码格式
            string html = Encoding.UTF8.GetString(downData);
            doc = new Ivony.Html.Parser.JumonyParser().Parse(html);
        }

        /// <summary>
        /// 获取同一个Class的内容
        /// </summary>
        /// <param name="cssKey">css样式关键字</param>
        /// <returns></returns>
        public List<string> GetClassTxt(string cssKey)
        {
            try
            {
                List<string> returnValue = new List<string>();
                if (!doc.Exists(cssKey))
                    return null;
                var values = doc.Find(cssKey);
                if (values == null)
                    return null;
                foreach (var item in values)
                {
                    returnValue.Add(item.InnerText());
                }
                return returnValue;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 获取元素Id的内容
        /// </summary>
        /// <param name="cssKeyId">css样式id</param>
        /// <returns></returns>
        public string GetElementByIdTxt(string cssKeyId)
        {
            try
            {
                if (!doc.Exists(cssKeyId))
                    return null;
                var values = doc.GetElementById(cssKeyId);
                if (values == null)
                    return null;
                return values.InnerText();
            }
            catch
            {
                return null;
            }
        }
    }
}
