using System;
using Gabriel.NewMyCat.Util;

namespace Gabriel.NewMyCat
{
    #region Nested type: Context

    public class Context
    {
        public Context()
        {
            this.IsException = false;
            this.BeginTime = MilliSecondTimer.CurrentTime();
        }
        /// <summary>
        /// 事件--根
        /// </summary>
        public Message.Transaction Transaction { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string Project { get; set; }
        /// <summary>
        /// 项目编码
        /// </summary>
        public string ProjectCode { get; set; }
        /// <summary>
        /// 是否发生异常
        /// </summary>
        public bool IsException { get; set; }
        /// <summary>
        /// 事件异常信息
        /// </summary>
        public string Exception { get; set; }
        /// <summary>
        /// 执行的开始时间
        /// DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss ffff");
        /// </summary>
        public DateTime BeginTime { get; set; }
        /// <summary>
        /// 执行的结事时间
        /// DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss ffff");
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 时间间隔(毫秒计算)
        /// </summary>
        public double TimeSpanInMilliseconds { get; set; }
    }

    #endregion
}