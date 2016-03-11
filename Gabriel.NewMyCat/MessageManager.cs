using Gabriel.NewMyCat.Util;

namespace Gabriel.NewMyCat
{
    public class MessageManager
    {
        private readonly CatThreadLocal<Context> _mContext = new CatThreadLocal<Context>();
        internal Context GetContext()
        {
            return _mContext.Value ?? (_mContext.Value = new Context());
        }

        internal void Dispose()
        {
            _mContext.Dispose();
        }
    }
}