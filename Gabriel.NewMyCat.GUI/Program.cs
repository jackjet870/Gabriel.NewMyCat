using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gabriel.NewMyCat.Util;

namespace Gabriel.NewMyCat.GUI
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger.Initialize();
            MyCat.Instance.NewTransaction("XX", "XX险批单接口");
            //
            MyCat.Instance.LogEvent("是否重新审核", "");
            MyCat.Instance.LogEvent("是否是总社", "");
            MyCat.Instance.LogEvent("调用批单接口", "一般性修改的站内信");
            MyCat.Instance.LogEvent("调用批单接口", "一般性批改的接口");
            //
            MyCat.Instance.Complete();
        }
    }
}
