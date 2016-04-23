using System;
using System.IO;
using System.Web;

namespace TimeHelper.Config
{
    /// <summary>
    /// �ƻ��������ù�����
    /// </summary>
    class WshelperConfigFileManager : DefaultConfigFileManager
    {
        private static WshelperConfigInfo m_configinfo;

        /// <summary>
        /// �ļ��޸�ʱ��
        /// </summary>
        private static DateTime m_fileoldchange;

        /// <summary>
        /// ��ʼ���ļ��޸�ʱ��Ͷ���ʵ��
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
        /// �����ļ�����·��
        /// </summary>
        public static string filename = null;

        /// <summary>
        /// ��ȡ�����ļ�����·��
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
        /// ����������ʵ��
        /// </summary>
        /// <returns></returns>
        public static WshelperConfigInfo LoadConfig()
        {
           ConfigInfo = LoadConfig(ref m_fileoldchange, ConfigFilePath, ConfigInfo, true);
            return ConfigInfo as WshelperConfigInfo;
        }

        /// <summary>
        /// ����������ʵ��
        /// </summary>
        /// <returns></returns>
        public override bool SaveConfig()
        {
            return base.SaveConfig(ConfigFilePath, ConfigInfo);
        }
    }
}
