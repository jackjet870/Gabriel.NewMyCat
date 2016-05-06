using System;
using System.Xml.Serialization;

namespace Gabriel.NewMyCat.Config
{
    /// <summary>
    /// 项目配置信息--Model
    /// </summary>
    [Serializable]
    [XmlRoot("ArrayOfConfigInfo")]
    public class ArrayOfConfigInfo
    {
        [XmlElement(ElementName = "ConfigInfo_NewMyCat")]
        public ConfigInfo Info { get; set; }
    }
    /// <summary>
    /// 指定项目的配置信息
    /// </summary>
    public class ConfigInfo
    {
        private string catErrorLogName = "CatError.log";
        /// <summary>
        /// Cat本身错误日志打点信息的存储名称
        /// 相对路径(relativePath)
        /// </summary>
        public string CatErrorLogName
        {
            get { return catErrorLogName; }
            set { catErrorLogName = value; }
        }
        private string catErrorLogPath = PublicCon.BaseDirectory();
        /// <summary>
        /// Cat本身错误日志打点信息的存储路径
        /// 绝对路径(absolutePath)
        /// </summary>
        public string CatErrorLogPath
        {
            get { return catErrorLogPath; }
            set { catErrorLogPath = value; }
        }
        /// <summary>
        /// Cat本身错误日志打点信息的存储完整路径
        /// </summary>
        [XmlIgnore]
        public string CatErrorLogFullPath
        {
            get { return PublicCon.PathCombine(CatErrorLogPath, CatErrorLogName); }
        }

       

        private string log4NetConfigFileName = "log4Net.Config";
        /// <summary>
        /// log4Net配置文件的名称
        /// 相对路径(relativePath)
        /// </summary>
        public string Log4NetConfigFileName
        {
            get
            {
                return log4NetConfigFileName;
            }

            set
            {
                log4NetConfigFileName = value;
            }
        }

        private string log4NetConfigFilePath = PublicCon.BaseDirectory();
        /// <summary>
        /// log4Net配置文件的路径
        /// 绝对路径(absolutePath)
        /// </summary>
        public string Log4NetConfigFilePath
        {
            get
            {
                return log4NetConfigFilePath;
            }

            set
            {
                log4NetConfigFilePath = value;
            }
        }
        /// <summary>
        /// log4Net配置文件的存储完整路径
        /// </summary>
        [XmlIgnore]
        public string Log4NetConfigFileFullPath
        {
            get { return PublicCon.PathCombine(Log4NetConfigFilePath, Log4NetConfigFileName); }
        }
        /// <summary>
        /// log4Net-Logger节点
        /// </summary>
        public string Log4ConfigLoggerName { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string Project { get; set; }
        /// <summary>
        /// 项目编码
        /// </summary>
        public string ProjectCode { get; set; }

    }
}