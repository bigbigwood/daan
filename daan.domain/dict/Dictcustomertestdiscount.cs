/*
insert license info here
*/
using System;

namespace daan.domain
{
	/// <summary>
	///	Generated by MyGeneration using the IBatis Object Mapping template
	/// </summary>
	[Serializable]
	public sealed class Dictcustomertestdiscount
	{
		#region Private Members
		private bool _isChanged;
		private bool _isDeleted;
		private  double? _dictcustomerdiscountid; 
		private  double? _dictcustomerid; 
		private  double? _dicttestitemid; 
		private  double? _finalprice; 
		private DateTime _begindate; 
		private DateTime _enddate; 
		private  double? _updateby; 
		private DateTime _updatedate; 
		private DateTime _createdate;
        private string _customername;
        private string _testname;
		#endregion
		
		#region Default ( Empty ) Class Constuctor
		/// <summary>
		/// default constructor
		/// </summary>
		public Dictcustomertestdiscount()
		{
			_dictcustomerdiscountid = 0; 
			_dictcustomerid = 0; 
			_dicttestitemid = 0; 
			_finalprice = 0; 
			_begindate = new DateTime(); 
			_enddate = new DateTime(); 
			_updateby = 0; 
			_updatedate = new DateTime(); 
			_createdate = new DateTime();
            _customername = null;
            _testname = null;

		}
		#endregion // End of Default ( Empty ) Class Constuctor
		
		#region Public Properties
			
		/// <summary>
		/// 主键，自动ID
		/// </summary>		
		public  double? Dictcustomerdiscountid
		{
			get { return _dictcustomerdiscountid; }
			set { _isChanged |= (_dictcustomerdiscountid != value); _dictcustomerdiscountid = value; }
		}
			
		/// <summary>
		/// 客户ID
		/// </summary>		
		public  double? Dictcustomerid
		{
			get { return _dictcustomerid; }
			set { _isChanged |= (_dictcustomerid != value); _dictcustomerid = value; }
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
		/// 测试项ID
		/// </summary>		
		public  double? Dicttestitemid
		{
			get { return _dicttestitemid; }
			set { _isChanged |= (_dicttestitemid != value); _dicttestitemid = value; }
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
		/// 最终价钱
		/// </summary>		
		public  double? Finalprice
		{
			get { return _finalprice; }
			set { _isChanged |= (_finalprice != value); _finalprice = value; }
		}
			
		/// <summary>
		/// 开始日期
		/// </summary>		
		public DateTime Begindate
		{
			get { return _begindate; }
			set { _isChanged |= (_begindate != value); _begindate = value; }
		}
			
		/// <summary>
		/// 结束日期
		/// </summary>		
		public DateTime Enddate
		{
			get { return _enddate; }
			set { _isChanged |= (_enddate != value); _enddate = value; }
		}
			
		/// <summary>
		/// 维护人
		/// </summary>		
		public  double? Updateby
		{
			get { return _updateby; }
			set { _isChanged |= (_updateby != value); _updateby = value; }
		}
			
		/// <summary>
		/// 维护日期
		/// </summary>		
		public DateTime Updatedate
		{
			get { return _updatedate; }
			set { _isChanged |= (_updatedate != value); _updatedate = value; }
		}
			
		/// <summary>
		/// 创建时间
		/// </summary>		
		public DateTime Createdate
		{
			get { return _createdate; }
			set { _isChanged |= (_createdate != value); _createdate = value; }
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
