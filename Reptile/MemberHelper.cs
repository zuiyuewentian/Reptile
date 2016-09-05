using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reptile
{
    public class MemberHelper
    {
        public static List<MemberEntity> Member;

        public MemberHelper()
        {
            Member = new List<MemberEntity>();
        }

        public bool AddMember(MemberEntity member)
        {
            if (IsMember(member.url))
            {
                return false;
            }
            else
            {
                Member.Add(member);
                return true;
            }
        }

        public bool IsMember(string url)
        {
            if (Member.Where(s => s.url == url).Count() > 0)
                return false;
            else
                return true;
        }

    }
}
