namespace TimeHelper.Config
{
    /// <summary>
    /// 论坛基本设置类
    /// </summary>
    public class WshelperConfigs
    {
        /// <summary>
        /// 获取进程配置类实例
        /// </summary>
        /// <returns></returns>
        public static WshelperConfigInfo GetConfig()
        {
            return WshelperConfigFileManager.LoadConfig();
        }

        /// <summary>
        /// 保存配置类实例
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
