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
	public sealed class Interfacelog:BaseDomain
	{
		#region Private Members
		private bool isChanged;
		private bool isDeleted;
		private string interfacelogid; 
		private double? interfacemanagerid; 
		private string logtext; 
		private DateTime createdate; 
		private string logdate; 		
		#endregion
		
		#region Default ( Empty ) Class Constuctor
		/// <summary>
		/// default constructor
		/// </summary>
		public Interfacelog()
		{
			interfacelogid = null; 
			interfacemanagerid = null; 
			logtext = null; 
			createdate = new DateTime(); 
			logdate = null; 
		}
		#endregion // End of Default ( Empty ) Class Constuctor
		
		#region Public Properties
			
		/// <summary>
		/// 主键
		/// </summary>	
		[LogInfo("主键")]
		public string Interfacelogid
		{
			get { return interfacelogid; }
			set	
			{
				if( value!= null && value.Length > 100)
					throw new ArgumentOutOfRangeException("Invalid value for Interfacelogid", value, value.ToString());
				
				isChanged |= (interfacelogid != value); interfacelogid = value;
			}
		}
			
		/// <summary>
		/// 接口ID
		/// </summary>	
		[LogInfo("接口ID")]
		public double? Interfacemanagerid
		{
			get { return interfacemanagerid; }
			set { isChanged |= (interfacemanagerid != value); interfacemanagerid = value; }
		}
			
		/// <summary>
		/// 日志信息
		/// </summary>	
		[LogInfo("日志信息")]
		public string Logtext
		{
			get { return logtext; }
			set	
			{
				if( value!= null && value.Length > 4000)
					throw new ArgumentOutOfRangeException("Invalid value for Logtext", value, value.ToString());
				
				isChanged |= (logtext != value); logtext = value;
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
		/// 日志的日期 格式YYYY-MM-DD
		/// </summary>	
		[LogInfo("日志的日期 格式YYYY-MM-DD")]
		public string Logdate
		{
			get { return logdate; }
			set	
			{
				if( value!= null && value.Length > 100)
					throw new ArgumentOutOfRangeException("Invalid value for Logdate", value, value.ToString());
				
				isChanged |= (logdate != value); logdate = value;
			}
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
