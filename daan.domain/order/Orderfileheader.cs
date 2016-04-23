using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace daan.domain
{
    [Serializable]
    public sealed class Orderfileheader : BaseDomain
    {
        #region Private Members
        private bool _isChanged;
        private bool _isDeleted;
        private double? _orderfileheaderid;
        private double? _dictlabid;
        private double? _dictcustormer;
        private double? _enterby;
        private DateTime? _createdate;
        private string _filename;
        private double? _status;
        private string _province;
        private string _city;
        private string _county;

        private string isunifiedpost;
        private string postaddress;
        private string recipient;
        private string contactnumber;
        #endregion

        #region Default ( Empty ) Class Constuctor
        /// <summary>
        /// default constructor
        /// </summary>
        public Orderfileheader()
        {
            _orderfileheaderid = 0;
            _dictlabid = 0;
            _dictcustormer = 0;
            _enterby = 0;
            _createdate = null;
            _filename = null;
            _status = 0;
            _province = null;
            _city = null;
            _county = null;
            isunifiedpost = "0";
            postaddress = null;
            recipient = null;
            contactnumber = null;
        }
        #endregion // End of Default ( Empty ) Class Constuctor

        #region Public Properties

        /// <summary>
        /// 主键，自动ID
        /// </summary>		
        public double? Orderfileheaderid
        {
            get { return _orderfileheaderid; }
            set { _isChanged |= (_orderfileheaderid != value); _orderfileheaderid = value; }
        }

        /// <summary>
        /// 分点实验室
        /// </summary>		
        public double? Dictlabid
        {
            get { return _dictlabid; }
            set { _isChanged |= (_dictlabid != value); _dictlabid = value; }
        }

        /// <summary>
        /// 单位
        /// </summary>		
        public double? Dictcustormer
        {
            get { return _dictcustormer; }
            set { _isChanged |= (_dictcustormer != value); _dictcustormer = value; }
        }
        /// <summary>
        /// 录单人
        /// </summary>		
        public double? Enterby
        {
            get { return _enterby; }
            set { _isChanged |= (_enterby != value); _enterby = value; }
        }
        /// <summary>
        /// 上传日期
        /// </summary>
        public DateTime? Createdate
        {
            get { return _createdate; }
            set { _isChanged |= (_createdate != value); _createdate = value; }
        }

        /// <summary>
        /// 文件名
        /// </summary>		
        public string Filename
        {
            get { return _filename; }
            set
            {
                if (value != null && value.Length > 500)
                    throw new ArgumentOutOfRangeException("Invalid value for Filename", value, value.ToString());

                _isChanged |= (_filename != value); _filename = value;
            }
        }
        /// <summary>
        /// 0未上传1上传完成   扫描状态
        /// </summary>
        public double? Status
        {
            get { return _status; }
            set { _isChanged |= (_status != value); _status = value; }
        }
        /// <summary>
        /// 省
        /// </summary>
        public string Province
        {
            get { return _province; }
            set
            {
                _isChanged |= (_province != value); _province = value;
            }
        }
        /// <summary>
        /// 市
        /// </summary>
        public string City
        {
            get { return _city; }
            set
            {
                _isChanged |= (_city != value); _city = value;
            }
        }
        /// <summary>
        /// 县
        /// </summary>
        public string County
        {
            get { return _county; }
            set { _isChanged |= (_county != value); _county = value; }
        }

        /// <summary>
        /// 是否统一寄送报告
        /// </summary>
        public string IsUnifiedpost
        {
            get { return isunifiedpost; }
            set { isChanged |= (isunifiedpost != value); isunifiedpost = value; }
        }

        /// <summary>
        /// 报告邮寄地址
        /// </summary>
        public string PostAddress
        {
            get { return postaddress; }
            set
            {
                isChanged |= (postaddress != value); postaddress = value;
            }
        }
        /// <summary>
        /// 收件人
        /// </summary>
        public string Recipient
        {
            get { return recipient; }
            set
            {
                isChanged |= (recipient != value); recipient = value;
            }
        }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string ContactNumber
        {
            get { return contactnumber; }
            set
            {
                isChanged |= (contactnumber != value); contactnumber = value;
            }
        }

        /// <summary>
        /// Returns whether or not the object has changed it's values.
        /// </summary>
        public bool IsChanged
        {
            get { return _isChanged; }
        }

        /// <summary>
        /// Returns whether or not the object has changed it's values.
        /// </summary>
        public bool IsDeleted
        {
            get { return _isDeleted; }
        }

        #endregion


        #region Public Functions

        /// <summary>
        /// mark the item as deleted
        /// </summary>
        public void MarkAsDeleted()
        {
            _isDeleted = true;
            _isChanged = true;
        }

        #endregion
    }
}
