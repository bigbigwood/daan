using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace daan.domain
{
    [Serializable]
    public sealed class Hpvinstruments
    {
        #region Private Members
        private bool _isChanged;
        private bool _isDeleted;
        private double? _hpvinstrumentsid;
        private double? _dictcustomerid;
        private double? _dicttestitemid;
        private string _instrumentsbarcode;
        private string _barcode;
        private DateTime? _instcreatedate;
        private DateTime? _barcodecreatedate;
        private string _instenterby;
        private string _barcodeenterby;
        private string _remark;
        private string _isactive;
        private string _testname;
        private string _labdeptname;
        private double? _price;
        private string _customername;
        #endregion

        #region Default ( Empty ) Class Constuctor
        /// <summary>
        /// default constructor
        /// </summary>
        public Hpvinstruments()
        {
            _hpvinstrumentsid = 0;
            _dictcustomerid = 0;
            _dicttestitemid = 0;
            _instrumentsbarcode = null;
            _barcode = null;
            _instcreatedate =DateTime.Now;
            _barcodecreatedate =null;
            _instenterby = null;
            _barcodeenterby = null;
            _remark = null;
            _isactive = null;
            _testname = null;
            _labdeptname = null;
            _price = 0;
            _customername = null; 
        }
        #endregion // End of Default ( Empty ) Class Constuctor

        #region Public Properties

        /// <summary>
        /// 主键，自动ID
        /// </summary>		
        public double? Hpvinstrumentsid
        {
            get { return _hpvinstrumentsid; }
            set { _isChanged |= (_hpvinstrumentsid != value); _hpvinstrumentsid = value; }
        }

        /// <summary>
        /// 客户ID
        /// </summary>		
        public double? Dictcustomerid
        {
            get { return _dictcustomerid; }
            set { _isChanged |= (_dictcustomerid != value); _dictcustomerid = value; }
        }

        /// <summary>
        /// 套餐ID
        /// </summary>		
        public double? Dicttestitemid
        {
            get { return _dicttestitemid; }
            set { _isChanged |= (_dicttestitemid != value); _dicttestitemid = value; }
        }

        /// <summary>
        /// 耗材条码号
        /// </summary>		
        public string Instrumentsbarcode
        {
            get { return _instrumentsbarcode;  }
            set
            {
                if (value != null && value.Length > 20)
                    throw new ArgumentOutOfRangeException("Invalid value for Instrumentsbarcode", value, value.ToString());

                _isChanged |= (_instrumentsbarcode != value); _instrumentsbarcode = value;
            }
        }

        /// <summary>
        /// 样本条码号
        /// </summary>		
        public string Barcode
        {
            get { return _barcode; }
            set
            {
                if (value != null && value.Length > 20)
                    throw new ArgumentOutOfRangeException("Invalid value for Barcode", value, value.ToString());

                _isChanged |= (_barcode != value); _barcode = value;
            }
        }

        /// <summary>
        /// 耗材激活时间
        /// </summary>		
        public DateTime? Instcreatedate
        {
            get { return _instcreatedate; }
            set { _isChanged |= (_instcreatedate != value); _instcreatedate = value; }
        }

        /// <summary>
        /// 录入样本条码时间
        /// </summary>		
        public DateTime? Barcodecreatedate
        {
            get { return _barcodecreatedate ; }
            set { _isChanged |= (_barcodecreatedate != value); _barcodecreatedate = value; }
        }

        /// <summary>
        /// 耗材激活人
        /// </summary>		
        public string Instenterby
        {
            get { return _instenterby; }
            set
            {
                if (value != null && value.Length > 20)
                    throw new ArgumentOutOfRangeException("Invalid value for Instenterby", value, value.ToString());

                _isChanged |= (_instenterby != value); _instenterby = value;
            }
        }

        /// <summary>
        /// 样本条码号激活人
        /// </summary>		
        public string Barcodeenterby
        {
            get { return _barcodeenterby; }
            set
            {
                if (value != null && value.Length > 20)
                    throw new ArgumentOutOfRangeException("Invalid value for Barcodeenterby", value, value.ToString());

                _isChanged |= (_barcodeenterby != value); _barcodeenterby = value;
            }
        }

        /// <summary>
        /// 备注
        /// </summary>		
        public string Remark
        {
            get { return _remark; }
            set
            {
                if (value != null && value.Length > 100)
                    throw new ArgumentOutOfRangeException("Invalid value for Remark", value, value.ToString());

                _isChanged |= (_remark != value); _remark = value;
            }
        }

        /// <summary>
        /// 是否可用0可用 1不可用
        /// </summary>		
        public string Isactive
        {
            get { return _isactive; }
            set
            {
                if (value != null && value.Length > 1)
                    throw new ArgumentOutOfRangeException("Invalid value for Isactive", value, value.ToString());

                _isChanged |= (_isactive != value); _isactive = value;
            }
        }

        /// <summary>
        /// 测试项中文名字
        /// </summary>	
        [LogInfo("测试项中文名字")]
        public string Testname
        {
            get { return _testname; }
            set
            {
                if (value != null && value.Length > 100)
                    throw new ArgumentOutOfRangeException("Invalid value for Testname", value, value.ToString());

                _isChanged |= (_testname != value); _testname = value;
            }
        }

        /// <summary>
        /// 科室名称
        /// </summary>	
        [LogInfo("科室名称")]
        public string Labdeptname
        {
            get { return _labdeptname; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Invalid value for Labdeptname", value, value.ToString());

                _isChanged |= (_labdeptname != value); _labdeptname = value;
            }
        }

        /// <summary>
        /// 价钱
        /// </summary>	
        [LogInfo("价钱")]
        public double? Price
        {
            get { return _price; }
            set { _isChanged |= (_price != value); _price = value; }
        }

        /// <summary>
        /// 客户名称
        /// </summary>		
        public string Customername
        {
            get { return _customername; }
            set
            {
                if (value != null && value.Length > 100)
                    throw new ArgumentOutOfRangeException("Invalid value for Customername", value, value.ToString());

                _isChanged |= (_customername != value); _customername = value;
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
