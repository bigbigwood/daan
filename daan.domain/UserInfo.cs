using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using daan.domain;

namespace daan.domain
{
    [Serializable]
    public class UserInfo
    {
        public string AuthorizationCode = "";                    //权限系统 GUID码
        public  string userCode = "";                            //登录名
        public double userId = 0;                                   //用户ID为 -1 则是 admin 用户
        public  string userName = "";                             //用户名
        //public string loginIp;                                   //登录计算机IP
        public DateTime loginTime;                                  //登录时间  
        public double? dictlabid;                                  //分点id
        /// <summary>
        ///分点ID拼接字符串
        /// </summary>
        public string joinLabidstr;

        public double? dictlabdeptid;                              //实验室ID

        /// <summary>
        ///实验室ID拼接字符串
        /// </summary>
        public string joinDeptstr;                  
        public  Initsyssetting sysSetting;                        //保存当前系统参数表
        public Initlocalsetting initlocalsetting;                        //保存当前系统参数表
    }
}
