using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Gabriel.NewMyCat.Message;
using Gabriel.NewMyCat.Util;

namespace Gabriel.NewMyCat
{
    public class MyCat
    {
        private static readonly Lazy<MyCat> lazy = new Lazy<MyCat>(() => new MyCat());

        public static MyCat Instance
        {
            get { return lazy.Value; }
        }

        private MyCat()
        {
        }

        private readonly MessageManager _manager = new MessageManager();

        /// <summary>
        /// 创建监测业务事务
        /// 访问属性-public=>private
        /// </summary>
        /// <param name="type">事务的类型</param>
        /// <param name="category">事务的分类</param>
        /// <param name="name">事务名称</param>
        private void NewTransaction(string type, string category, string name)
        {
            var ctx = _manager.GetContext();
            var t = new Transaction(type, category, name);
            t.Depth++;
            if (ctx.Transaction == null)
            {
                ctx.Transaction = t;
                return;
            }
            ctx.Transaction.Depth++;
            //
            var countE = ctx.Transaction.Evens.Count;
            if (countE == 0)
            {
                throw new Exception("创建监测业务事务异常，ctx.Transaction.Evens.Count=0");
            }
            var rootEven = ctx.Transaction.Evens[countE - 1];
            AddTransction(rootEven, t);
        }

        private void AddTransction(Even root, Transaction node)
        {
            var countT = root.Transactions.Count;
            if (countT == 0)
            {
                root.NewTransaction(node);
                return;
            }
            if (countT > 0)
            {
                var currentT = root.Transactions[countT - 1];
                if (currentT.Depth == 0)
                {
                    root.NewTransaction(node);
                    return;
                }
                var countE = currentT.Evens.Count;
                if (countE == 0)
                {
                    root.NewTransaction(node);
                    return;
                }
                var currentE = currentT.Evens[countE - 1];
                AddTransction(currentE, node);
                currentT.Depth++;
            }
        }

        /// <summary>
        /// 创建详细业务事件
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        public void LogEvent(string name, string description)
        {
            var ctx = _manager.GetContext();
            var e = new Even(name, description);
            AddEvent(ctx.Transaction, e);
        }
        /// <summary>
        /// 创建业务异常事件
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        public void ExceptionEvent(string name, string description)
        {
            var ctx = _manager.GetContext();
            if (ctx.IsException)
            {

                return;
            }
            ctx.IsException = true;
            ctx.Exception = description;
            var e = new Even(name, description) {IsException = true};
            AddEvent(ctx.Transaction, e);
        }
        /// <summary>
        /// 创建业务开始事件
        /// </summary>
        public void BeginEvent()
        {
            var ctx = _manager.GetContext();
            var e = new Even("业务开始", "") { BeginOrEnd = true };
            AddEvent(ctx.Transaction, e);
        }
        /// <summary>
        /// 创建业务结束事件
        /// </summary>
        public void EndEvent()
        {
            var ctx = _manager.GetContext();
            var e = new Even("业务结束", "") { BeginOrEnd = true };
            AddEvent(ctx.Transaction, e);
        }
        private void AddEvent(Transaction root, Even node)
        {
            var countE = root.Evens.Count;
            if (countE == 0)
            {
                root.NewEven(node);
                return;
            }
            if (countE > 0)
            {
                var currentE = root.Evens[countE - 1];
                var countT = currentE.Transactions.Count;
                if (countT == 0)
                {
                    root.NewEven(node);
                    return;
                }
                var currentT = currentE.Transactions[countT - 1];
                if (currentT.Depth == 0)
                {
                    root.NewEven(node);
                    return;
                }
                AddEvent(currentT, node);
            }
        }
        /// <summary>
        /// 业务事务-完成
        /// 访问属性-public=>private
        /// </summary>
        private void Complete()
        {
            var ctx = _manager.GetContext();
            CompleteTransaction(ctx.Transaction);
            if (ctx.Transaction.Depth == 0)
            {
                ctx.EndTime = MilliSecondTimer.CurrentTime();
                #region 时间间隔(毫秒计算)
                ctx.TimeSpanInMilliseconds = MilliSecondTimer.TimeSpanInMilliseconds(ctx.BeginTime, ctx.EndTime);
                #endregion
                var log = MessagePrint.PlainTextMessage(ctx);
                //LOG Exception
                LogHelper.Instance.Info(log);
                //Logger.Info(log);
                //
                ctx.Transaction = null;
                //
                _manager.Dispose();
            }
        }

        private void CompleteTransaction(Transaction root)
        {
            root.Depth--;
            var depth = root.Depth;
            if (depth == 0)
            {
                #region 时间间隔(毫秒计算)
                root.TimeSpanInMilliseconds = 
                    MilliSecondTimer.TimeSpanInMilliseconds(root.Time,MilliSecondTimer.CurrentTime());
                #endregion
                return;
            }
            //
            var countE = root.Evens.Count;
            if (countE == 0)
            {
                return;
            }
            var rootEven = root.Evens[countE - 1];
            var countT = rootEven.Transactions.Count;
            if (countT == 0)
            {
                return;
            }
            var currentT = rootEven.Transactions[countT - 1];
            CompleteTransaction(currentT);
        }

        #region MyCatExtensions

        /// <summary>
        /// 日志打点
        /// </summary>
        /// <param name="type">事务的类型-相当于类名(class name)</param>
        /// <param name="category">事务的分类-相当于方法名(method name)</param>
        /// <param name="name">事务名称</param>
        /// <param name="work"></param>
        public void Define(string type, string category, string name, Action work)
        {
            MyCat.Instance.NewTransaction(type, category, name);
            MyCat.Instance.BeginEvent();
            try
            {
                work();
                MyCat.Instance.EndEvent();
                MyCat.Instance.Complete();
            }
            catch (Exception ex)
            {
                MyCat.Instance.ExceptionEvent("异常事件", ex.Message);
                MyCat.Instance.EndEvent();
                MyCat.Instance.Complete();
                throw;
            }
        }

        /// <summary>
        /// 日志打点
        /// </summary>
        /// <typeparam name="TReturnType"></typeparam>
        /// <param name="type">事务的类型-相当于类名(class name)</param>
        /// <param name="category">事务的分类-相当于方法名(method name)</param>
        /// <param name="name">事务名称</param>
        /// <param name="work"></param>
        /// <returns></returns>
        public TReturnType Define<TReturnType>(string type, string category, string name, Func<TReturnType> work)
        {
            MyCat.Instance.NewTransaction(type, category, name);
            MyCat.Instance.BeginEvent();
            try
            {
                var result = work();
                MyCat.Instance.EndEvent();
                MyCat.Instance.Complete();
                return result;
            }
            catch (Exception ex)
            {
                MyCat.Instance.ExceptionEvent("异常事件", ex.Message);
                MyCat.Instance.EndEvent();
                MyCat.Instance.Complete();
                throw;
            }
        }

        #endregion
    }
}