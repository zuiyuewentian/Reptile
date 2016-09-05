using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace Reptile
{
    public class XMLHelper
    {
        string ServerPath = System.AppDomain.CurrentDomain.BaseDirectory;
        protected XmlDocument objXmlDoc = new XmlDocument();
        public XMLHelper(string fileName)
        {
            ServerPath = ServerPath + fileName + ".xml";
        }

        public void CreateXml()
        {
            try
            {
                if (!File.Exists(ServerPath))
                {
                    CreatXmlFile(ServerPath);
                }
                objXmlDoc.Load(ServerPath);

            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        private void CreatXmlFile(string file)
        {
            XmlTextWriter writer = new
            XmlTextWriter(file, Encoding.UTF8);
            // start writing!
            writer.WriteStartDocument();
            writer.WriteStartElement("jobbole");
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();
        }

        /// <summary>
        /// 保存文件
        /// </summary>
        public void Save()
        {
            //保存文件。 
            try
            {
                objXmlDoc.Save(ServerPath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            objXmlDoc = null;
        }

        public void Insert(MemberEntity member)
        {
            XmlNode rootElement = objXmlDoc.SelectSingleNode("jobbole");
            XmlElement node = objXmlDoc.CreateElement("member");//创建一个<Node>节点 
            node.SetAttribute("Id", member.Id.ToString() == null ? "" : member.Id.ToString());
            node.SetAttribute("name", member.name == null ? "" : member.name);
            node.SetAttribute("image", member.image == null ? "" : member.image);
            node.SetAttribute("medal", member.medal == null ? "" : member.medal);
            node.SetAttribute("point", member.point == null ? "" : member.point);
            node.SetAttribute("profile", member.profile == null ? "" : member.profile);
            node.SetAttribute("reputation", member.reputation == null ? "" : member.reputation);
            node.SetAttribute("sex", member.sex == null ? "" : member.sex);
            node.SetAttribute("url", member.url == null ? "" : member.url);
            node.SetAttribute("city", member.city == null ? "" : member.city);
            node.SetAttribute("Date", member.Date == null ? "" : member.Date);
            node.SetAttribute("follower", member.follower == null ? "" : member.follower);
            node.SetAttribute("following", member.following == null ? "" : member.following);
            rootElement.AppendChild(node);
        }

        public void InsertUrl(int id, string url)
        {
            XmlNode rootElement = objXmlDoc.SelectSingleNode("jobbole");
            XmlElement node = objXmlDoc.CreateElement("member");//创建一个<Node>节点 
            node.SetAttribute("Id", id.ToString());
            node.SetAttribute("URL", url);
            rootElement.AppendChild(node);
        }

        public List<string> SelectAllUrl()
        {
            List<string> urls = new List<string>();
            XmlNode rootNode = objXmlDoc.SelectSingleNode("jobbole");
            foreach (XmlNode item in rootNode.ChildNodes)
            {
                urls.Add(item.Attributes["URL"].Value.ToString());
            }
            return urls;
        }

        public List<MemberEntity> SelectAllMember()
        {
            List<MemberEntity> members = new List<MemberEntity>();
            XmlNode rootNode = objXmlDoc.SelectSingleNode("jobbole");
            foreach (XmlNode item in rootNode.ChildNodes)
            {
                MemberEntity member = new MemberEntity();
                if (String.IsNullOrEmpty(item.Attributes["Id"].Value.ToString()))
                    member.Id = 0;
                else
                    member.Id = Convert.ToInt32(item.Attributes["Id"].Value.ToString());
                member.name = item.Attributes["name"].Value.ToString();
                member.image = item.Attributes["image"].Value.ToString();
                member.medal = item.Attributes["medal"].Value.ToString();
                member.point = item.Attributes["point"].Value.ToString();
                member.profile = item.Attributes["profile"].Value.ToString();
                member.reputation = item.Attributes["reputation"].Value.ToString();
                member.sex = item.Attributes["sex"].Value.ToString();
                member.url = item.Attributes["url"].Value.ToString();
                member.city = item.Attributes["city"].Value.ToString();
                member.Date = item.Attributes["Date"].Value.ToString();
                member.follower = item.Attributes["follower"].Value.ToString();
                member.following = item.Attributes["following"].Value.ToString();
                members.Add(member);
            }
            return members;
        }
    }
}
