using System;
using System.Collections.Generic;
using System.IO;
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
            CodeTimerHelper.Initialize();
            CodeTimerHelper.Time("CodeTimerHelper", 10, CodeTimerFun);
            Console.ReadKey();
        }

        private static void CodeTimerFun()
        {
            CodeTimerHelper.Time("NewMyCat", 100000, PidanEnterpriseExt);
        }

        #region 扩展版

        static void PidanEnterpriseExt()
        {
            MyCat.Instance.Define("XX-EXT","XX", "XX险批单接口", () =>
            {
                //
                MyCat.Instance.LogEvent("是否重新审核", "");
                CheckEnterpriseExt();
                MyCat.Instance.LogEvent("是否是总社", "");
                MyCat.Instance.LogEvent("调用批单接口", "一般性修改的站内信");
                MyCat.Instance.LogEvent("调用批单接口", "一般性批改的接口");
                //
            });
        }

        static void CheckEnterpriseExt()
        {
            MyCat.Instance.Define("CC-EXT", "XX", "企业审核", () =>
            {
                //
                MyCat.Instance.LogEvent("审核信息", "获取企业审核信息");
                ConnectDatabaseExt();
                //
                MyCat.Instance.LogEvent("重新审核", "判断企业是否需要重新审核");
                UpdateEnterpriseExt();
                //
            });
        }

        static void ConnectDatabaseExt()
        {
            MyCat.Instance.Define("DD-EXT", "XX", "数据库连接", () =>
            {
                //
                MyCat.Instance.LogEvent("连接数据库", "获取企业审核信息");
                //System.Threading.Thread.Sleep(1000);
                //throw new Exception("数据库连接失败！");
                //
            });
        }

        static int UpdateEnterpriseExt()
        {
            return MyCat.Instance.Define("II-EXT", "XX", "更新企业信息", () =>
            {
                //
                MyCat.Instance.LogEvent("更新企业信息", "企业信息");
                //System.Threading.Thread.Sleep(2000);
                //throw new Exception("更新企业信息失败！");
                //
                return 1;
            });
        }

        #endregion

    }
}
