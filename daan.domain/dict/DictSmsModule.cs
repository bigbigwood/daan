using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace daan.domain
{
    [Serializable]
    public class DictSmsModule: BaseDomain
    {
        private int dictsmsmoduleid;
        private string smstitle;
        private string smscontent;     

        #region Default ( Empty ) Class DictModule
		/// <summary>
		/// default constructor
		/// </summary>
        public DictSmsModule()
		{
			dictsmsmoduleid = 0;
            smstitle = null;
            smscontent = null;  
		}
		#endregion // End of Default ( Empty ) Class Constuctor
       /// <summary>
       /// ID
       /// </summary>
        public int DictSmsModuleid
        {
            get { return this.dictsmsmoduleid; }
            set { this.dictsmsmoduleid = value; }
        }
        /// <summary>
        /// 短信标题
        /// </summary>
        public string SmsTitle
        {
            get { return this.smstitle; }
            set { this.smstitle = value; }
        }
        /// <summary>
        /// 短信内容
        /// </summary>
        public string SmsContent
        {
            get { return this.smscontent; }
            set { this.smscontent = value; }
        }
    }
}
