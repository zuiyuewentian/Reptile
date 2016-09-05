using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reptile
{
    public class MemberEntity
    {
        /// <summary>
        ///  Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 名字
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string url { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string image { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public string sex { get; set; }

        /// <summary>
        /// 声望
        /// </summary>
        public string reputation { get; set; }

        /// <summary>
        /// 勋章
        /// </summary>
        public string medal { get; set; }

        /// <summary>
        /// 积分
        /// </summary>
        public string point { get; set; }

        /// <summary>
        /// 介绍
        /// </summary>
        public string profile { get; set; }

        /// <summary>
        /// 关注
        /// </summary>
        public string following { get; set; }

        /// <summary>
        /// 粉丝
        /// </summary>
        public string follower { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        public string city { get; set; }

        /// <summary>
        /// 注册时间
        /// </summary>
        public string Date { get; set; }
    }
}
