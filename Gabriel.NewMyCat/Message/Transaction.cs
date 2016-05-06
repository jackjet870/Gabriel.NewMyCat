using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gabriel.NewMyCat.Util;

namespace Gabriel.NewMyCat.Message
{
    public class Transaction : Message
    {
        /// <summary>
        /// 创建事务
        /// </summary>
        /// <param name="type">事务的类型</param>
        /// <param name="category">事务的分类</param>
        /// <param name="name">事务名称</param>
        public Transaction(string type,string category, string name)
        {
            // TODO: Complete member initialization
            this.Type = type;
            this.Category = category;
            this.Name = name;
            this.Time = MilliSecondTimer.CurrentTime();
        }
        /// <summary>
        /// 事务中嵌套的事件集合
        /// </summary>
        public readonly List<Even> Evens = new List<Even>();
        /// <summary>
        /// 创建事件
        /// </summary>
        /// <param name="e"></param>
        public void NewEven(Even e)
        {
            Evens.Add(e);
            #region 时间间隔(毫秒计算)
            var count = Evens.Count;
            if (count > 2)
            {
                var preItem = Evens[count - 2];
                var curItem = Evens[count - 1];
                preItem.TimeSpanInMilliseconds = MilliSecondTimer.TimeSpanInMilliseconds(preItem.Time, curItem.Time);
            }
            #endregion

        }
        /// <summary>
        /// 事务的类型
        /// 相当于类名(class name)
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 事务的分类
        /// 相当于方法名(method name)
        /// </summary>
        public string Category { get; set; }
        /// <summary>
        /// 事务的名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 操作执行时间（HH:mm:ss ffff）
        /// DateTime.Now.ToString("HH:mm:ss ffff");
        /// </summary>
        public DateTime Time { get; set; }
        /// <summary>
        /// 时间间隔(毫秒计算)
        /// </summary>
        public double TimeSpanInMilliseconds { get; set; }
        /// <summary>
        /// 深度--嵌套操作计数
        /// </summary>
        public int Depth { get; set; }

    }
}
