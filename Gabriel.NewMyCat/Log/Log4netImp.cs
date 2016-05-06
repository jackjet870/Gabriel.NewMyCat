using System;
using System.IO;
using log4net.Config;

namespace Gabriel.NewMyCat.Log
{
    public class Log4NetImp:ILog
    {
        private string _log4ConfigFileFullPath;
        private string _log4ConfigLoggerName;
        readonly log4net.ILog _logger;

        public Log4NetImp(string log4ConfigFileFullPath,string log4ConfigLoggerName)
        {
            _log4ConfigFileFullPath = log4ConfigFileFullPath;
            _log4ConfigLoggerName = log4ConfigLoggerName;
            //Log4Net--初始化
            if (!File.Exists(_log4ConfigFileFullPath))
            {
                throw new Exception(string.Format(
                    "Log4Net配置信息文件没有找到！检查配置节点{{ConfigInfo_NewMyCat：Log4NetConfigFileName}}是否正确。{0}FilePath:{1}",
                    System.Environment.NewLine, _log4ConfigFileFullPath));
            }
            //
            var logCfg = new FileInfo(this._log4ConfigFileFullPath);
            XmlConfigurator.ConfigureAndWatch(logCfg);
            //
            if (string.IsNullOrWhiteSpace(log4ConfigLoggerName))
            {
                throw new Exception(string.Format("在项目配置文件中检查配置节点{{ConfigInfo_NewMyCat：Log4ConfigLoggerName}}不能为空。"));
            }
            //
            var repository = log4net.LogManager.GetRepository();
            if (repository.GetLogger(_log4ConfigLoggerName) == null)
            {
                throw new Exception(string.Format("在log4net日志配置文件（{0}）中检查配置节点<logger name=\"{1}\" additivity=\"false\">不存在。", log4ConfigFileFullPath,log4ConfigLoggerName));
            }           
            _logger = log4net.LogManager.GetLogger(_log4ConfigLoggerName);
        }

        public void Info(string pattern)
        {
            _logger.Info(pattern);
        }
    }
}