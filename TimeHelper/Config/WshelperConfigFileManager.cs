using System;
using System.IO;
using System.Web;

namespace TimeHelper.Config
{
    /// <summary>
    /// 计划任务设置管理类
    /// </summary>
    class WshelperConfigFileManager : DefaultConfigFileManager
    {
        private static WshelperConfigInfo m_configinfo;

        /// <summary>
        /// 文件修改时间
        /// </summary>
        private static DateTime m_fileoldchange;

        /// <summary>
        /// 初始化文件修改时间和对象实例
        /// </summary>
        static WshelperConfigFileManager()
        {
            m_fileoldchange = File.GetLastWriteTime(ConfigFilePath);
            m_configinfo = (WshelperConfigInfo)DeserializeInfo(ConfigFilePath, typeof(WshelperConfigInfo));
        }

        public new static IConfigInfo ConfigInfo
        {
            get { return m_configinfo; }
            set { m_configinfo = (WshelperConfigInfo)value; }
        }

        /// <summary>
        /// 配置文件所在路径
        /// </summary>
        public static string filename = null;

        /// <summary>
        /// 获取配置文件所在路径
        /// </summary>
        public new static string ConfigFilePath
        {
            get
            {
                if (filename == null)
                {
                    filename = HttpContext.Current.Request.MapPath("~/timehelper.config");
                }

                return filename;
            }
        }

        /// <summary>
        /// 返回配置类实例
        /// </summary>
        /// <returns></returns>
        public static WshelperConfigInfo LoadConfig()
        {
           ConfigInfo = LoadConfig(ref m_fileoldchange, ConfigFilePath, ConfigInfo, true);
            return ConfigInfo as WshelperConfigInfo;
        }

        /// <summary>
        /// 保存配置类实例
        /// </summary>
        /// <returns></returns>
        public override bool SaveConfig()
        {
            return base.SaveConfig(ConfigFilePath, ConfigInfo);
        }
    }
}
