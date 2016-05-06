using System;
using System.Globalization;
using System.IO;
using System.Text;
using Gabriel.NewMyCat.Config;

namespace Gabriel.NewMyCat.Util
{
    public class NewMyCatError
    {
        //Cat本身错误日志打点信息的存储完整路径
        private static readonly string CatErrorLogFullPath = ConfigInfoHelper.Instance.Info.CatErrorLogFullPath;
        private static StreamWriter _mWriter;
        public static void Initialize()
        {
            var logFile = CatErrorLogFullPath;
            Initialize(logFile);
        }
        /// <summary>
        ///   初始化
        /// </summary>
        /// <param name="logFile"> </param>
        public static void Initialize(string logFile)
        {
            try
            {
                if (!File.Exists(logFile))
                {
                    var directoryInfo = new FileInfo(logFile).Directory;
                    if (directoryInfo != null) directoryInfo.Create();
                }

                _mWriter = new StreamWriter(logFile, true);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error when openning log file: " + e.Message + " " + e.StackTrace + ".");
            }
        }
        internal static void Error(Exception ex)
        {
            var text = LogException(ex);
            Error(text);
            _mWriter?.Close();
        }
        private static void Error(string pattern, params object[] args)
        {
            Log("ERROR", pattern, args);
        }
        private static void Log(string severity, string pattern, params object[] args)
        {
            string timestamp = MilliSecondTimer.CurrentTimeMicrosToString(MilliSecondTimer.CurrentTimeMicros());
            string message = string.Format(pattern, args);
            string line = "[" + timestamp + "] [" + severity + "] " +
                          Environment.NewLine + "---------------------" +
                          message + "---------------------";

            if (_mWriter != null)
            {
                _mWriter.WriteLine(line);
                _mWriter.Flush();
            }
            else
            {
                Console.WriteLine(line);
            }

        }
        private static string LogException(Exception ex)
        {
            try
            {
                var newLine = System.Environment.NewLine;
                var sb = new StringBuilder();
                sb.Append(newLine + ".系统出现如下错误：" + newLine);
                sb.Append("发生时间：" + DateTime.Now.ToString(CultureInfo.InvariantCulture) + newLine);
                if (ex.TargetSite != null)
                {
                    var targetSite = ((System.Reflection.MemberInfo)(ex.TargetSite));
                    sb.Append("方法名称：" + targetSite.Name + newLine);
                    sb.Append("C#类名称：" + targetSite.ReflectedType.FullName + newLine);
                    sb.Append("程 序 集：" + ex.TargetSite.Module.Name + newLine);
                }
                if (ex.TargetSite == null)
                {
                    sb.Append("错误对象：" + ex.Source + newLine);
                }
                sb.Append("异常信息：" + ex.Message.Replace("\r\n", ";") + newLine);
                sb.Append("堆栈信息：" + newLine + ex.StackTrace.ToString(CultureInfo.InvariantCulture) + newLine);
                return sb.ToString();
            }
            catch (Exception exception)
            {
                return CannotLogEvent(exception);
            }

        }

        private static string CannotLogEvent(Exception exception)
        {
            var newLine = System.Environment.NewLine;
            var sb = new StringBuilder();
            sb.Append("系统出现如下错误：" + newLine);
            sb.Append("无法记录日志!" + newLine);
            return sb.ToString();
        }
    }
}