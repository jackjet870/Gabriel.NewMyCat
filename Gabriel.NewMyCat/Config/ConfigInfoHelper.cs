using System;

namespace Gabriel.NewMyCat.Config
{
    public sealed class ConfigInfoHelper
    {
        private static readonly Lazy<ConfigInfoHelper> lazy =
            new Lazy<ConfigInfoHelper>(() => new ConfigInfoHelper());

        public static ConfigInfoHelper Instance
        {
            get { return lazy.Value; }
        }

        private ConfigInfoHelper()
        {
            var xml = PublicCon.LoadConfigInfo();
            var arrayOfConfigInfoObj = SerializeHelper.FromXml<ArrayOfConfigInfo>(xml);
            //设置默认的配置信息 
            Info = arrayOfConfigInfoObj.Info ?? new ConfigInfo();
        }
        /// <summary>
        /// 当前项目的配置信息
        /// </summary>
        public ConfigInfo Info;
    }
}