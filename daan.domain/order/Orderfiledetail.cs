using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace daan.domain
{
    [Serializable]
    public sealed class Orderfiledetail : BaseDomain
    {
        #region Private Members
        private bool _isChanged;
        private bool _isDeleted;
        private double? _orderfiledetailid;
        private double? _orderfileheaderid;
        private string _barcode;
        private double? _status;
        private string _reason;
        private DateTime? _createdate;
        private string _realname;
        private string _mobile;
        private string _idnumber;
        #endregion

        #region Default ( Empty ) Class Constuctor
        /// <summary>
        /// default constructor
        /// </summary>
        public Orderfiledetail()
        {
            _orderfiledetailid = 0;
            _orderfileheaderid = 0;
            _barcode = null;
            _status = 0;
            _reason = null;
            _createdate = null;
            _realname = null;
            _mobile = null;
            _idnumber = null;
        }
        #endregion // End of Default ( Empty ) Class Constuctor

        #region Public Properties

        /// <summary>
        /// 子表id
        /// </summary>		
        public double? Orderfiledetailid
        {
            get { return _orderfiledetailid; }
            set { _isChanged |= (_orderfiledetailid != value); _orderfiledetailid = value; }
        }

        /// <summary>
        /// 头表id
        /// </summary>		
        public double? Orderfileheaderid
        {
            get { return _orderfileheaderid; }
            set { _isChanged |= (_orderfileheaderid != value); _orderfileheaderid = value; }
        }


        /// <summary>
        /// 条码号
        /// </summary>		
        public string Barcode
        {
            get { return _barcode; }
            set
            {
                if (value != null && value.Length > 20)
                    throw new ArgumentOutOfRangeException("Invalid value for Barcode", value, value.ToString());

                _isChanged |= (_barcode != value); _barcode = value.ToString();
            }
        }
        /// <summary>
        /// 0上传失败1上传成功
        /// </summary>
        public double? Status
        {
            get { return _status; }
            set { _isChanged |= (_status != value); _status = value; }
        }
        /// <summary>
        /// 失败原因
        /// </summary>
        public string Reason
        {
            get { return _reason; }
            set
            {
                if (value != null && value.Length > 200)
                    throw new ArgumentOutOfRangeException("Invalid value for Reason", value, value.ToString());

                _isChanged |= (_reason != value); _reason = value;
            }
        }
        /// <summary>
        /// 创建时间
        /// </summary>		
        public DateTime? Createdate
        {
            get { return _createdate; }
            set { _isChanged |= (_createdate != value); _createdate = value; }
        }

        /// <summary>
        /// 姓名
        /// </summary>		
        public string Realname
        {
            get { return _realname; }
            set
            {
                if (value != null && value.Length > 20)
                    throw new ArgumentOutOfRangeException("Invalid value for Realname", value, value.ToString());

                _isChanged |= (_realname != value); _realname = value.ToString();
            }
        }

        /// <summary>
        /// 手机号
        /// </summary>		
        public string Mobile
        {
            get { return _mobile; }
            set
            {
                if (value != null && value.Length > 20)
                    throw new ArgumentOutOfRangeException("Invalid value for Mobile", value, value.ToString());

                _isChanged |= (_mobile != value); _mobile = value.ToString();
            }
        }
        /// <summary>
        /// 身份证号码
        /// </summary>		
        public string Idnumber
        {
            get { return _idnumber; }
            set
            {
                if (value != null && value.Length > 20)
                    throw new ArgumentOutOfRangeException("Invalid value for Idnumber", value, value.ToString());

                _isChanged |= (_idnumber != value); _idnumber = value.ToString();
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
