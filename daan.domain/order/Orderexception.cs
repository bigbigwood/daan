using System;
namespace daan.domain
{
    /// <summary>
    ///	异常条码表
    /// </summary>
    [Serializable]
    public sealed class Orderexception : BaseDomain
    {
        #region Private Members
        private bool isChanged;
        private bool isDeleted;
        private double? orderexceptionid;
        private string exceptiontype;
        private string subbarcode;
        private string applyby;
        private DateTime? applydate;
        private string remark;
        private string approveby;
        private DateTime? approvedate;
        private string status;
        private DateTime? createdate;
        private string barcode; 
        private string disposeby;
        private DateTime? disposedate;
        private string suggestion;
        private string disposestate;
        private string labcode;

        
        #endregion

        #region Default ( Empty ) Class Constuctor
        /// <summary>
        /// default constructor
        /// </summary>
        public Orderexception()
        {
            orderexceptionid = null;
            exceptiontype = null;
            subbarcode = null;
            applyby = null;
            applydate = null;
            remark = null;
            approveby = null;
            approvedate = null;           
            status = null;
            createdate = DateTime.Now;
            barcode= null;
            disposeby= null;
            disposedate = null;
            suggestion= null;
            disposestate = "0";
            labcode = null;
        }
        #endregion // End of Default ( Empty ) Class Constuctor

        #region Public Properties

        /// <summary>主键，序列
        /// 
        /// </summary>	
        [LogInfo("主键，序列")]
        public double? Orderexceptionid
        {
            get { return orderexceptionid; }
            set { isChanged |= (orderexceptionid != value); orderexceptionid = value; }
        }

        /// <summary>三种值：DELAY/RERUN/CANCEL
        /// 
        /// </summary>	
        [LogInfo("三种值：DELAY/RERUN/CANCEL")]
        public string Exceptiontype
        {
            get { return exceptiontype; }
            set
            {
                if (value != null && value.Length > 40)
                    throw new ArgumentOutOfRangeException("Invalid value for Exceptiontype", value, value.ToString());

                isChanged |= (exceptiontype != value); exceptiontype = value;
            }
        }

        /// <summary>条码号
        /// 
        /// </summary>	
        [LogInfo("条码号")]
        public string Subbarcode
        {
            get { return subbarcode; }
            set
            {
                if (value != null && value.Length > 40)
                    throw new ArgumentOutOfRangeException("Invalid value for Subbarcode", value, value.ToString());

                isChanged |= (subbarcode != value); subbarcode = value;
            }
        }

        /// <summary>申请人
        /// 
        /// </summary>	
        [LogInfo("申请人")]
        public string Applyby
        {
            get { return applyby; }
            set { isChanged |= (applyby != value); applyby = value; }
        }

        /// <summary>申请时间
        /// 
        /// </summary>	
        [LogInfo("申请时间")]
        public DateTime? Applydate
        {
            get { return applydate; }
            set { isChanged |= (applydate != value); applydate = value; }
        }

        /// <summary>操作原因
        /// 
        /// </summary>	
        [LogInfo("操作原因")]
        public string Remark
        {
            get { return remark; }
            set
            {
                if (value != null && value.Length > 500)
                    throw new ArgumentOutOfRangeException("Invalid value for Remark", value, value.ToString());

                isChanged |= (remark != value); remark = value;
            }
        }

        /// <summary>审批人
        /// 
        /// </summary>	
        [LogInfo("审批人")]
        public string Approveby
        {
            get { return approveby; }
            set { isChanged |= (approveby != value); approveby = value; }
        }

        /// <summary>审批时间
        /// 
        /// </summary>	
        [LogInfo("")]
        public DateTime? Approvedate
        {
            get { return approvedate; }
            set { isChanged |= (approvedate != value); approvedate = value; }
        }       

        /// <summary>状态，对应表InitBasic
        /// 
        /// </summary>	
        [LogInfo("状态，对应表InitBasic")]
        public string Status
        {
            get { return status; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Invalid value for Status", value, value.ToString());

                isChanged |= (status != value); status = value;
            }
        }

        /// <summary>创建日期
        /// 
        /// </summary>	
        [LogInfo("创建日期")]
        public DateTime? CreateDate
        {
            get { return createdate; }
            set { isChanged |= (createdate != value); createdate = value; }
        }

        /// <summary>主条码号
        /// 
        /// </summary>
        public string Barcode
        {
            get { return barcode; }
            set { barcode = value; }
        }
  
        /// <summary>处理人
        /// 
        /// </summary>
        public string Disposeby
        {
            get { return disposeby; }
            set { disposeby = value; }
        }

        /// <summary>处理时间
        /// 
        /// </summary>
        public DateTime? Disposedate
        {
            get { return disposedate; }
            set { disposedate = value; }
        }

        /// <summary>处理意见
        /// 
        /// </summary>
        public string Suggestion
        {
            get { return suggestion; }
            set { suggestion = value; }
        }

        /// <summary>处理状态
        /// 
        /// </summary>
        public string Disposestate
        {
            get { return disposestate; }
            set { disposestate = value; }
        }

        /// <summary>分点标识
        /// 
        /// </summary>
        public string Labcode
        {
            get { return labcode; }
            set { labcode = value; }
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
