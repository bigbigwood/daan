using System;

namespace daan.domain
{

    [Serializable]
    public class CacheInfo
    {

        private string authorizationcode;
        /// <summary>
        /// Guid|SID
        /// </summary>
        public string AuthorizationCode
        {
            get { return authorizationcode; }
            set { authorizationcode = value; }
        }

        private string userid;
        /// <summary>
        /// 登录ID
        /// </summary>
        public string UserId
        {
            get { return userid; }
            set { userid = value; }
        }
        private string usercode;
        /// <summary>
        /// 登录码
        /// </summary>
        public string UserCode
        {
            get { return usercode; }
            set { usercode = value; }
        }
        private string username;
        /// <summary>
        /// 登录名
        /// </summary>
        public string UserName
        {
            get { return username; }
            set { username = value; }
        }
        private string password;
        /// <summary>
        /// 登录密码
        /// </summary>
        public string PassWord
        {
            get { return password; }
            set { password = value; }
        }
        //private string _operator;
        ///// <summary>
        ///// 操作人
        ///// </summary>
        //public string Operator
        //{
        //    get { return _operator; }
        //    set { _operator = value; }
        //}
        //private string sitecode;
        ///// <summary>
        ///// 分点
        ///// </summary>
        //public string SiteCode
        //{
        //    get { return sitecode; }
        //    set { sitecode = value; }
        //}

        public CacheInfo()
        {
            authorizationcode = string.Empty;
            username = string.Empty;
            password = string.Empty;
            //_operator = string.Empty;
            //sitecode = string.Empty;
            userid = string.Empty;
            usercode = string.Empty;
        }


    }
}
