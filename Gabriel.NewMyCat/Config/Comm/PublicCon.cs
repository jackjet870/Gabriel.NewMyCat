using System;
using System.Configuration;
using System.IO;
using System.Text;

namespace Gabriel.NewMyCat.Config
{
    /// <summary>
    /// 公共数据
    /// </summary>
    internal class PublicCon
    {
        /// <summary>
        /// 当前项目的基本路径
        /// </summary>
        /// <returns></returns>
        public static string BaseDirectory()
        {
            return System.AppDomain.CurrentDomain.BaseDirectory;
        }
        /// <summary>
        /// 配置信息文件的存储路径
        /// app.config-配置节点
        /// <appSettings>
        /// <add key = "ConfigInfoPath" value="Config\ConfigInfo.xml" />
        /// </appSettings>
        /// </summary>
        /// <returns></returns>
        public static string ConfigInfoPath()
        {
            return ConfigurationManager.AppSettings["ConfigInfoPath"];
        }
        /// <summary>
        /// 加载配置信息
        /// </summary>
        /// <returns></returns>
        public static string LoadConfigInfo()
        {
            var path = BaseDirectory();
            var fileName = ConfigInfoPath();
            var filePath = Path.Combine(path, fileName);
            return Read(filePath, Encoding.UTF8);
        }
        /// <summary>
        /// 文件读取
        /// </summary>
        /// <returns></returns>
        public static string Read(string filePath, Encoding encoding)
        {
            if (!File.Exists(filePath))
            {
                throw new Exception(string.Format(
                    "项目配置信息文件没有找到！检查配置节点{{appSettings：ConfigInfoPath}}是否正确。{0}FilePath:{1}",
                    System.Environment.NewLine, filePath));
            }
            return File.ReadAllText(filePath, encoding);
        }
        /// <summary>
        /// 路径合并
        /// </summary>
        /// <param name="path">绝对路径</param>
        /// <param name="fileName">相对路径</param>
        /// <returns></returns>
        public static string PathCombine(string path, string fileName)
        {
            path = path.TrimEnd('\\') + '\\';
            fileName = fileName.TrimStart('\\');
            return Path.Combine(path, fileName);
        }
    }
}