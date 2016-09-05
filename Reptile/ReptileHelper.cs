using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Ivony.Html;

namespace Reptile
{
    public class ReptileHelper
    {
        ///==============配置信息2016年9月2日11:50:22===============//
        /// <summary>
        /// 所属网站 
        /// </summary>
        private const string BASEURL = "jobbole.com";

        /// <summary>
        /// 最终需要的URL规则关键字 
        /// </summary>
        private const string NEEDURL = "jobbole.com/members";

        /// <summary>
        /// 线程数量
        /// </summary>
        public int ThreadCount = 1;

        /// <summary>
        /// 爬虫的深度要求不能大于BASEURLDEPTH
        /// </summary>
        public const int BASEURLDEPTH = 4;

        /// <summary>
        /// 所需要的URL深度要求=NEEDURLDEPTH
        /// </summary>
        public const int NEEDURLDEPTH = 3;

        ///======================================//

        /// <summary>
        /// 线程
        /// </summary>
        public Thread[] ThreadList;

        /// <summary>
        /// 初始爬的页面
        /// </summary>
        public List<string> PageStartURL;

        /// <summary>
        /// 爬到的所有URL
        /// </summary>
        public List<string> ReptileUrl;

        /// <summary>
        /// 已经访问过的URL
        /// </summary>
        public List<string> VisitedUrl;

        /// <summary>
        /// 需要的URL
        /// </summary>
        public List<string> NeedUrlList;

        /// <summary>
        /// Reptile锁   读写URl使用
        /// </summary>
        private static readonly object ReptileObj = new object();
        private static readonly object VisitedObj = new object();
        private static readonly object NeedObj = new object();

        /// <summary>
        /// 是否结束爬虫
        /// </summary>
        public bool IsEndReptile = false;

        /// <summary>
        ///  url 的数量=线程数量
        /// </summary>
        /// <param name="url"></param>
        public ReptileHelper(List<string> url)
        {
            if (url == null)
                return;

            if (url.Count < ThreadCount)
                return;

            ReptileUrl = new List<string>();
            VisitedUrl = new List<string>();
            PageStartURL = new List<string>();
            NeedUrlList = new List<string>();

            PageStartURL.AddRange(url);
        }

        /// <summary>
        /// 开始爬取
        /// </summary>
        public void ReptileStart()
        {
            ThreadList = new Thread[ThreadCount];
            for (int thread = 0; thread < ThreadCount; thread++)
            {
                ThreadList[thread] = new Thread(new ParameterizedThreadStart(Reptile));
                ThreadList[thread].Name = "myThread" + thread;
                ThreadList[thread].IsBackground = true;
                ThreadList[thread].Start(PageStartURL[thread]);
            }
        }

        public void Reptile(object url)
        {
            //初始网址抓取
            string repUrl = url.ToString();
            ReptileURL(repUrl);
            lock (ReptileObj)
            {
                ReptileUrl.Add(repUrl);
            }
            lock (VisitedObj)
            {
                VisitedUrl.Add(repUrl);
            }
            //循环抓取
            int startIndex = 0;
            while (true)
            {
                int reptileUrlCount = ReptileUrl.Count();
                if (reptileUrlCount <= 0)
                    break;
                if (reptileUrlCount <= VisitedUrl.Count())
                {
                    IsEndReptile = true;
                    break;
                }
                for (int i = startIndex; i < reptileUrlCount; i++)
                {
                    string rurl = ReptileUrl[i];
                    // 去除重复爬网页
                    if (VisitedUrl.Contains(rurl))
                        continue;
                    ReptileURL(rurl);
                    lock (VisitedObj)
                    {
                        VisitedUrl.Add(rurl);
                    }
                }
                startIndex = reptileUrlCount;
            }
        }

        /// <summary>
        /// NeedUrl ID
        /// </summary>
        int MemberIndex = 1;
        /// <summary>
        /// 爬取网页中a标签的链接地址
        /// </summary>
        /// <param name="url"></param>
        public void ReptileURL(string url)
        {
            if (!IsBaseUrl(url))
                return;
            JumonyHelper jumonyHelper = new JumonyHelper(url);
            if (jumonyHelper.doc == null)
                return;
            var urlList = jumonyHelper.doc.Find("a[href]");
            if (urlList == null)
                return;
            foreach (var item in urlList)
            {
                string itemUrl = item.Attribute("href").Value();
                if (!ReptileUrl.Contains(itemUrl))
                {
                    lock (ReptileObj)
                    {
                        ReptileUrl.Add(itemUrl);
                    }
                    if (IsNeedUrl(itemUrl))
                    {
                        itemUrl = itemUrl.TrimEnd('/');
                        if (!NeedUrlList.Contains(itemUrl))
                        {
                            lock (NeedObj)
                            {
                                NeedUrlList.Add(itemUrl);
                                insertURLXML(MemberIndex, itemUrl);
                                MemberIndex++;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 是否是需要的网页
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public bool IsNeedUrl(string url)
        {
            if (url == null)
                return false;

            if (!url.Contains("http://") && !(url.Contains("https://")))
                return false;

            if (url.Contains(NEEDURL))
            {
                if (GetUrlDepth(url) == NEEDURLDEPTH)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        /// <summary>
        /// 是否是需要的网页
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public bool IsBaseUrl(string url)
        {
            if (url == null)
                return false;

            if (!url.Contains("http://") && !(url.Contains("https://")))
                return false;

            if (GetUrlDepth(url) > BASEURLDEPTH)
                return false;

            if (url.Contains(BASEURL))
                return true;
            else
                return false;
        }


        /// <summary>
        /// 计算URL深度
        /// </summary>
        /// <returns></returns>
        public int GetUrlDepth(string url)
        {
            if (url.Contains("http://"))
                url = url.Substring(7, url.Length - 7);
            else if (url.Contains("https://"))
                url = url.Substring(8, url.Length - 8);
            else
                return 0;

            if (String.IsNullOrEmpty(url))
                return 0;
            url = url.TrimEnd('/');
            if (url.Contains('/'))
            {
                int depth = url.Split('/').Count();
                return depth;
            }
            else
            {
                return 1;
            }
        }

        /// <summary>
        /// 插入URLXML数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="url"></param>
        private void insertURLXML(int id, string url)
        {
            XMLHelper xmlHelper = new XMLHelper("NEWURL");
            xmlHelper.CreateXml();
            xmlHelper.InsertUrl(id, url);
            xmlHelper.Save();
        }
    }
}
