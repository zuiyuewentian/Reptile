using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Reptile
{
    class Program
    {
        static ReptileHelper reptileHelper;

        static ReptilePageHelper reptilePageHelper;
        static void Main(string[] args)
        {
            //初始入口
            string firstUrl = "http://www.jobbole.com/members/skl569552591/";
            string secUrl = "http://group.jobbole.com/";
            string thUrl = "http://group.jobbole.com/23425/?utm_source=blog.jobbole.com&utm_medium=sidebar-group-topic";
            List<string> urls = new List<string>() { firstUrl, secUrl, thUrl };
            //爬取有效URL，并存入NEWURL.XML文件
            reptileHelper = new ReptileHelper(urls);
            reptileHelper.ReptileStart();
            //解析有效数据，并存入jobbole.XML文件
            reptilePageHelper = new ReptilePageHelper();
            reptilePageHelper.ReptileStart(reptileHelper.NeedUrlList);
            //统计状态
            Thread loadThread = new Thread(TrackReptile);
            loadThread.Name = "CountURL";
            loadThread.Start();
            Console.WriteLine("开始时间：" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss ffff"));
        }

        /// <summary>
        /// 跟踪爬虫状态
        /// </summary>
        public static void TrackReptile()
        {
            while (true)
            {
                int allCount = reptileHelper.ReptileUrl.Count();
                int count = reptileHelper.NeedUrlList.Count();
                int haveCount = reptilePageHelper.VisitedUrl.Count();
                Console.WriteLine("已爬的网页：" + allCount.ToString());
                Console.WriteLine("需要的网页：" + count.ToString());
                Console.WriteLine("已经爬的数据：" + haveCount.ToString());
                Console.WriteLine("时间：" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss ffff"));
                Thread.Sleep(1000);

                if (reptilePageHelper.IsEndReptile)
                    reptilePageHelper.ReptileStart(reptileHelper.NeedUrlList);

                if (reptilePageHelper.IsEndReptile && reptilePageHelper.IsEndReptile)
                {
                    Console.WriteLine("END：" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss ffff"));
                    break;
                }
            }
        }

    }
}
