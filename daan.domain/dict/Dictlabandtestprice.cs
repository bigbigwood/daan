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
	public sealed class Dictlabandtestprice
	{
		#region Private Members
		private bool _isChanged;
		private bool _isDeleted;
		private  double? _dictlabandtestpriceid; 
		private  double? _dictlabid; 
		private  double? _dicttestitemid; 
		private DateTime _begindate; 
		private DateTime _enddate; 
		private  double? _price; 
		private DateTime _createdate;
        private string _labname;
        private string _testname;
		#endregion
		
		#region Default ( Empty ) Class Constuctor
		/// <summary>
		/// default constructor
		/// </summary>
		public Dictlabandtestprice()
		{
			_dictlabandtestpriceid = 0; 
			_dictlabid = 0; 
			_dicttestitemid = 0; 
			_begindate = new DateTime(); 
			_enddate = new DateTime(); 
			_price = 0; 
			_createdate = new DateTime();
            _labname = null;
            _testname = null;
		}
		#endregion // End of Default ( Empty ) Class Constuctor
		
		#region Public Properties
			
		/// <summary>
		/// 测试项主键
		/// </summary>		
		public  double? Dictlabandtestpriceid
		{
			get { return _dictlabandtestpriceid; }
			set { _isChanged |= (_dictlabandtestpriceid != value); _dictlabandtestpriceid = value; }
		}
			
		/// <summary>
		/// 试验室ID
		/// </summary>		
		public  double? Dictlabid
		{
			get { return _dictlabid; }
			set { _isChanged |= (_dictlabid != value); _dictlabid = value; }
		}

        /// <summary>
        /// 试验室名称
        /// </summary>	
        [LogInfo("试验室名称")]
        public string Labname
        {
            get { return _labname; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Invalid value for Labname", value, value.ToString());

                _isChanged |= (_labname != value); _labname = value;
            }
        }
			
		/// <summary>
		/// 检测项目
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
		/// 此折扣开始时间
		/// </summary>		
		public DateTime Begindate
		{
			get { return _begindate; }
			set { _isChanged |= (_begindate != value); _begindate = value; }
		}
			
		/// <summary>
		/// 此折扣结束时间
		/// </summary>		
		public DateTime Enddate
		{
			get { return _enddate; }
			set { _isChanged |= (_enddate != value); _enddate = value; }
		}
			
		/// <summary>
		/// 价钱
		/// </summary>		
		public  double? Price
		{
			get { return _price; }
			set { _isChanged |= (_price != value); _price = value; }
		}
			
		/// <summary>
		/// 
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
