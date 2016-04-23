using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace daan.domain
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class ProjectControl : BaseDomain
    {

        private string  _PID = string.Empty;
        /// <summary>
        /// 主键ID
        /// </summary>
        public string PID
        {
            get { return _PID; }
            set
            {
                _PID = value;
            }
        }

        private string _OldUniquecode = string.Empty;
        /// <summary>
        /// OldUniquecode
        /// </summary>
        public string OldUniquecode
        {
            get { return _OldUniquecode; }
            set { _OldUniquecode = value; }
        }

        private string _NewUniquecode = string.Empty;
        /// <summary>
        /// NewUniquecode
        /// </summary>
        public string NewUniquecode
        {
            get
            {
                return _NewUniquecode;
            }
            set {
                _NewUniquecode = value;
            }
        }

        private string _TestName = string.Empty;
        /// <summary>
        /// 测试名称
        /// </summary>
        public string TestName
        {
            get {
                return _TestName;
            }
            set
            {
                _TestName = value;
            }
        }
        private string _CreateTime = string.Empty;

        public string CreateTime
        {
            get {
                return _CreateTime;
            }
            set
            {
                _CreateTime = value;
            }
        }
    }
}
