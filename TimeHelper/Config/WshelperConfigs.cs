namespace TimeHelper.Config
{
    /// <summary>
    /// ��̳����������
    /// </summary>
    public class WshelperConfigs
    {
        /// <summary>
        /// ��ȡ����������ʵ��
        /// </summary>
        /// <returns></returns>
        public static WshelperConfigInfo GetConfig()
        {
            return WshelperConfigFileManager.LoadConfig();
        }

        /// <summary>
        /// ����������ʵ��
        /// </summary>
        /// <returns></returns>
        public static bool SaveConfig(WshelperConfigInfo scheduleconfiginfo)
        {
            WshelperConfigFileManager scfm = new WshelperConfigFileManager();
            WshelperConfigFileManager.ConfigInfo = scheduleconfiginfo;
            return scfm.SaveConfig();
        }
    }
}
