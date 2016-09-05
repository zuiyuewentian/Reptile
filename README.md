Reptile,采用C#+jumony开发的一个简单爬虫。
该程序以爬取伯乐在线（http://www.jobbole.com/）用户信息为实例，并对爬取的用户做数据分析。

##使用准备：
使用Nuget程序包管理工具下载 jumony安装。

##Class类介绍：
JumonyHelper：在该爬虫程序中主要用于在HTML代码中获取选择要求的数据，方法很多，具体参看jumony官网。
MemberEntity：爬取的数据结构
MemberHelper：添加数据帮助类
ReptileHelper：爬取有效URL并加入下载列表。
ReptilePageHelper：根据有效下载列表下载具体数据
WriteTxt：写入Txt文本
XMLHelper：读取写入XML数据文件

##注意：
1. 开启多线程爬取时应该以使用电脑的效率最高为目的，而不应该随意开启太多线程。
2. 请选择合适的入口，以连接本站页面多为好。
3. 请勿滥用爬虫程序爬取网站，以免造成对网站形成流量攻击。
4. 未引用代理IP，有些网站会对恶意爬取进行封IP处理。

