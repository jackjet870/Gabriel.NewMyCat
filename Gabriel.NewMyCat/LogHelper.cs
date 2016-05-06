using System;
using Gabriel.NewMyCat.Config;
using Gabriel.NewMyCat.Log;
using Gabriel.NewMyCat.Util;

namespace Gabriel.NewMyCat
{
    public sealed class LogHelper
    {
        private static readonly Lazy<LogHelper> lazy =
            new Lazy<LogHelper>(() => new LogHelper());

        public static LogHelper Instance { get { return lazy.Value; } }

        private LogHelper()
        {
            var log4ConfigFileFullPath = ConfigInfoHelper.Instance.Info.Log4NetConfigFileFullPath;
            var log4ConfigLoggerName = ConfigInfoHelper.Instance.Info.Log4ConfigLoggerName;
            //
            _log =new Log4NetImp(log4ConfigFileFullPath, log4ConfigLoggerName);
        }
        private ILog _log;

        public void Info(string pattern)
        {
            try
            {
                _log.Info(pattern);
            }
            catch (Exception ex)
            {
                NewMyCatError.Initialize();
                NewMyCatError.Error(ex);
            }
            
        }
    }
}