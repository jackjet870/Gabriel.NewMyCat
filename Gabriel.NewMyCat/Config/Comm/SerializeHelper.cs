using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Gabriel.NewMyCat.Config
{
    /// <summary>
    /// 序列化--C#内置的序列化方法
    /// </summary>
    internal class SerializeHelper
    {
        #region XML

        /// <summary>
        /// XML序列化
        /// </summary>
        /// <param name="item">对象</param>
        public static string ToXml<T>(T item)
        {
            XmlSerializer serializer = new XmlSerializer(item.GetType());
            StringBuilder sb = new StringBuilder();
            using (XmlWriter writer = XmlWriter.Create(sb))
            {
                serializer.Serialize(writer, item);
                return sb.ToString();
            }
        }

        /// <summary>
        /// XML反序列化
        /// </summary>
        /// <param name="str">字符串序列</param>
        public static T FromXml<T>(string str)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (XmlReader reader = new XmlTextReader(new StringReader(str)))
            {
                return (T)serializer.Deserialize(reader);
            }
        }

        #endregion
    }
}