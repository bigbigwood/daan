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
	public class Dictlabdept:BaseDomain
	{
		#region Private Members
		private bool isChanged;
		private bool isDeleted;
		private double? dictlabdeptid; 
		private string labdeptname; 
		private string labdepttype; 
		private DateTime createdate;
        private string basicname; 
		#endregion
		
		#region Default ( Empty ) Class Constuctor
		/// <summary>
		/// default constructor
		/// </summary>
		public Dictlabdept()
		{
			dictlabdeptid = 0; 
			labdeptname = null; 
			labdepttype = null; 
			createdate = new DateTime();
            basicname = null;
		}
		#endregion // End of Default ( Empty ) Class Constuctor
		
		#region Public Properties
			
		/// <summary>
		/// 主键ID
		/// </summary>	
		[LogInfo("主键ID")]
		public double? Dictlabdeptid
		{
			get { return dictlabdeptid; }
			set { isChanged |= (dictlabdeptid != value); dictlabdeptid = value; }
		}
			
		/// <summary>
		/// 科室名称
		/// </summary>	
		[LogInfo("科室名称")]
		public string Labdeptname
		{
            get { return labdeptname == null ? "" : labdeptname; }
			set	
			{
				if( value!= null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for Labdeptname", value, value.ToString());
				
				isChanged |= (labdeptname != value); labdeptname = value;
			}
		}
			
		/// <summary>
		/// 对应INITBASIC表
		/// </summary>	
		[LogInfo("对应INITBASIC表")]
		public string Labdepttype
		{
            get { return labdepttype == null ? "" : labdepttype; }
			set	
			{
				if( value!= null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for Labdepttype", value, value.ToString());
				
				isChanged |= (labdepttype != value); labdepttype = value;
			}
		}

        /// <summary>
        /// 界面显示的值
        /// </summary>	
        [LogInfo("界面显示的值")]
        public string Basicname
        {
            get { return basicname == null ? "" : basicname; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Invalid value for Basicname", value, value.ToString());

                isChanged |= (basicname != value); basicname = value;
            }
        }
		/// <summary>
		/// 创建时间
		/// </summary>	
		[LogInfo("创建时间")]
		public DateTime Createdate
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
