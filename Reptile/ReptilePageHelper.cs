using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ivony.Html;
using System.Threading;

namespace Reptile
{
    public class ReptilePageHelper
    {
        /// <summary>
        /// 线程数量
        /// </summary>
        public int ThreadCount = 3;

        /// <summary>
        /// 线程
        /// </summary>
        public Thread[] ReptileThread;

        /// <summary>
        /// 已经访问过的URL
        /// </summary>
        public List<string> VisitedUrl;

        private static readonly object ReptileObj = new object();
        private static readonly object ReadObj = new object();

        /// <summary>
        /// 是否结束爬虫
        /// </summary>
        public bool IsEndReptile = false;

        public MemberHelper memberHelper;

        public ReptilePageHelper()
        {
            VisitedUrl = new List<string>();

            memberHelper = new MemberHelper();
        }

        /// <summary>
        /// 开始爬取
        /// </summary>
        public void ReptileStart(List<string> ReptileUrl)
        {
            IsEndReptile = false;

            if (ReptileUrl == null)
            {
                IsEndReptile = true;
                return;
            }

            if (ReptileUrl.Count <= 0)
            {
                IsEndReptile = true;
                return;
            }
            ReptileThread = new Thread[ThreadCount];
            for (int thread = 0; thread < ThreadCount; thread++)
            {
                ReptileThread[thread] = new Thread(new ParameterizedThreadStart(Reptile));
                ReptileThread[thread].Name = "myThread" + thread;
                ReptileThread[thread].IsBackground = true;
                ReptileThread[thread].Start(ReptileUrl);
            }
        }

        int startIndex = 0;
        public void Reptile(object ReptileUrl)
        {
            List<string> RepUrl = (List<string>)ReptileUrl as List<string>;
            int count = RepUrl.Count();
            while (true)
            {
                if (RepUrl.Count() <= VisitedUrl.Count())
                {
                    IsEndReptile = true;
                    break;
                }

                for (int i = startIndex; i < count; i++)
                {
                    string url = RepUrl[i];
                    if (VisitedUrl.Contains(url))
                        continue;

                    lock (ReadObj)
                    {
                        VisitedUrl.Add(url);
                    }
                    LoadUser(url);
                }
                startIndex = count;
            }
        }

        int UserIndex = 1;
        public void LoadUser(string userUrl)
        {
            MemberEntity member = new MemberEntity();
            member.url = userUrl;
            try
            {
                JumonyHelper jumonyHelper = new JumonyHelper(userUrl);
                var nameValue = jumonyHelper.doc.FindFirst(".profile-title");
                string name = nameValue.InnerText();
                member.name = name;
                var profiles = jumonyHelper.doc.Find(".profile-points > li");
                foreach (var item in profiles)
                {
                    string value = item.InnerText();
                    if (value.Contains("\r\n"))
                    {
                        value = value.Replace("\r\n", "|");
                        string[] pros = value.Split('|');
                        if (pros[1] == "声望")
                        {
                            member.reputation = pros[0];
                        }
                        else if (pros[1] == "勋章")
                        {
                            member.medal = pros[0];
                        }
                        else if (pros[1] == "积分")
                        {
                            member.point = pros[0];
                        }
                    }
                }
                var profile = jumonyHelper.doc.FindFirst(".profile-bio");
                member.profile = profile.InnerText();
                var follows = jumonyHelper.doc.Find(".profile-follow");
                foreach (var item in follows)
                {
                    string value = item.InnerText();
                    if (!String.IsNullOrEmpty(value))
                    {
                        if (value.Contains("关注"))
                        {
                            string following = value.Split('（')[1].Split('）')[0];
                            member.following = following;
                        }
                        else if (value.Contains("粉丝"))
                        {
                            string follower = value.Split('（')[1].Split('）')[0];
                            member.follower = follower;
                        }
                    }
                }
                var infos = jumonyHelper.doc.Find(".member-info > span");
                foreach (var item in infos)
                {
                    string value = item.InnerText();
                    if (!String.IsNullOrEmpty(value))
                    {
                        if (value.Contains("注册"))
                        {
                            string date = value.Split('：')[1];
                            member.Date = date;
                        }
                        else if (value.Contains("城市"))
                        {
                            string city = value.Split('：')[1];
                            member.city = city;
                        }
                    }
                }

                var image = jumonyHelper.doc.FindFirst(".profile-img > a > img");
                string imageUrl = image.Attribute("src").Value();
                member.image = imageUrl;

                if (jumonyHelper.doc.Exists("i[title]"))
                {
                    var sexHtml = jumonyHelper.doc.FindFirst("i[title]");
                    string sex = sexHtml.Attribute("title").Value();
                    member.sex = sex;
                }
                else
                {
                    member.sex = "";
                }
                member.Id = UserIndex;
                lock (ReptileObj)
                {
                    memberHelper.AddMember(member);
                    insertXML(member);
                    UserIndex++;
                }
            }
            catch (Exception ex)
            {
                WriteTxt.WriteNewTxt("ERRORLOG", "++++错误数据+++" + ex.Message);
            }
        }

        private void insertXML(MemberEntity member)
        {
            XMLHelper xmlHelper = new XMLHelper("jobbole");
            xmlHelper.CreateXml();
            xmlHelper.Insert(member);
            xmlHelper.Save();
        }
    }
}
