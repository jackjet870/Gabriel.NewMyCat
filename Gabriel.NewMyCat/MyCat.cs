using System;
using System.Collections.Generic;
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
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        public void NewTransaction(string type, string name)
        {
            var ctx = _manager.GetContext();
            var t = new Transaction(type, name);
            t.Depth++;
            if (ctx.Transaction == null)
            {
                ctx.Transaction = t;
                return;
            }
            ctx.Transaction.Depth++;
            //
            var countE = ctx.Transaction.Evens.Count;
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

        public void Complete()
        {
            var ctx = _manager.GetContext();
            var n = 0;
            CompleteTransaction(ctx.Transaction);
            if (ctx.Transaction.Depth == 0)
            {
                ctx.EndTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss ffff");
                var log = MessagePrint.PlainTextMessage(ctx);
                Logger.Info(log);
                //
                ctx.Transaction = null;
                _manager.Dispose();
            }
        }

        private void CompleteTransaction(Transaction root)
        {
            root.Depth--;
            var depth = root.Depth;
            if (depth == 0)
            {
                return;
            }
            //
            var countE = root.Evens.Count;
            var rootEven = root.Evens[countE - 1];
            var countT = rootEven.Transactions.Count;
            var currentT = rootEven.Transactions[countT - 1];
            CompleteTransaction(currentT);
        }


    }
}