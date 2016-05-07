using System;

namespace daan.domain
{
    [Serializable]
    public sealed class Orderreportdata:BaseDomain
    {
        #region Private Members
        private bool isChanged;
        private bool isDeleted;
        private double? orderreportdataid;
        private string ordernum;
        private string reportdata;
        private DateTime? createdate;
        #endregion

        #region Default ( Empty ) Class Constuctor
		/// <summary>
		/// default constructor
		/// </summary>
        public Orderreportdata()
		{
            orderreportdataid = null; 
			ordernum = null;
            reportdata = null;
			createdate = new DateTime(); 
		}
		#endregion // End of Default ( Empty ) Class Constuctor

        #region Public Properties
        /// <summary>
        /// 主键
        /// </summary>	
        [LogInfo("主键")]
        public double? Orderreportdataid
        {
            get { return orderreportdataid; }
            set { isChanged |= (orderreportdataid != value); orderreportdataid = value; }
        }

        /// <summary>
        /// 体检流水号
        /// </summary>	
        [LogInfo("体检流水号")]
        public string Ordernum
        {
            get { return ordernum; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Invalid value for Ordernum", value, value.ToString());

                isChanged |= (ordernum != value); ordernum = value;
            }
        }

        /// <summary>
        /// 体检报告数据
        /// </summary>	
        [LogInfo("体检报告数据")]
        public string ReportData
        {
            get { return reportdata; }
            //set
            //{
            //    if (value != null && value.Length > 50)
            //        throw new ArgumentOutOfRangeException("Invalid value for ReportData", value, value.ToString());

            //    isChanged |= (reportdata != value); reportdata = value;
            //}
            set { isChanged |= (reportdata != value); reportdata = value; }
        }

        /// <summary>
        /// 创建时间
        /// </summary>	
        [LogInfo("创建时间")]
        public DateTime? Createdate
        {
            get { return createdate; }
            set { isChanged |= (createdate != value); createdate = value; }
        }

        /// <summary>
        /// Returns whether or not the object has changed it's values.
        /// </summary>
        public bool IsChanged
        {
            get { return isChanged; }
        }

        /// <summary>
        /// Returns whether or not the object has changed it's values.
        /// </summary>
        public bool IsDeleted
        {
            get { return isDeleted; }
        }
        #endregion

        #region Public Functions
        /// <summary>
        /// mark the item as deleted
        /// </summary>
        public void MarkAsDeleted()
        {
            isDeleted = true;
            isChanged = true;
        }
        #endregion
    }
}
