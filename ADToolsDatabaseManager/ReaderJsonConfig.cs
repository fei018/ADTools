using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace ADToolsDatabaseManager
{
    public class ReaderJsonConfig
    {
        /// <summary>
        /// App Path
        /// </summary>
        public string AppPath => AppDomain.CurrentDomain.BaseDirectory;

        /// <summary>
        /// 获取 appconfig.json 配置文件
        /// </summary>
        public string AppConfigJson => Path.Combine(AppPath, "appsettings.json");

        /// <summary>
        /// Daabase ConnectionString
        /// </summary>
        public string ConnectionString => GetConfigJsonValue("ConnectionStrings", "ADToolsDb");

        /// <summary>
        /// 获取 配置文件节点的值
        /// </summary>
        public string GetConfigJsonValue(string node, params string[] subNode)
        {
            try
            {
                using var file = File.OpenRead(AppConfigJson);
                var json = JsonDocument.Parse(file);
                var root = json.RootElement;

                JsonElement element = root.GetProperty(node);
                if (subNode == null || subNode.Length <= 0)
                {
                    return element.GetString();
                }

                for (int i = 0; i < subNode.Length; i++)
                {
                    element = element.GetProperty(propertyName: subNode[i]);
                }
                var conn = element.GetString();

                return conn;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
