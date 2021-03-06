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
	public sealed class Interfacemanager:BaseDomain
	{
		#region Private Members
		private bool isChanged;
		private bool isDeleted;
		private double? interfacemanagerid; 
		private string interfacename; 
		private string isstop; 
		private string status; 
		private double? frequency; 
		private string starttime; 
		private string endtime; 
		private string remark; 
		private double? displayorder; 
		private DateTime createdate; 
		private string executecode; 		
		#endregion
		
		#region Default ( Empty ) Class Constuctor
		/// <summary>
		/// default constructor
		/// </summary>
		public Interfacemanager()
		{
			interfacemanagerid = null; 
			interfacename = null; 
			isstop = null; 
			status = null; 
			frequency = null; 
			starttime = null; 
			endtime = null; 
			remark = null; 
			displayorder = null; 
			createdate = new DateTime(); 
			executecode = null; 
		}
		#endregion // End of Default ( Empty ) Class Constuctor
		
		#region Public Properties
			
		/// <summary>
		/// 主键
		/// </summary>	
		[LogInfo("主键")]
		public double? Interfacemanagerid
		{
			get { return interfacemanagerid; }
			set { isChanged |= (interfacemanagerid != value); interfacemanagerid = value; }
		}
			
		/// <summary>
		/// 接口名称
		/// </summary>	
		[LogInfo("接口名称")]
		public string Interfacename
		{
			get { return interfacename; }
			set	
			{
				if( value!= null && value.Length > 100)
					throw new ArgumentOutOfRangeException("Invalid value for Interfacename", value, value.ToString());
				
				isChanged |= (interfacename != value); interfacename = value;
			}
		}
			
		/// <summary>
		/// 是否停止  0运行  1 停止
		/// </summary>	
		[LogInfo("是否停止  0运行  1 停止")]
		public string Isstop
		{
			get { return isstop; }
			set	
			{
				if( value!= null && value.Length > 1)
					throw new ArgumentOutOfRangeException("Invalid value for Isstop", value, value.ToString());
				
				isChanged |= (isstop != value); isstop = value;
			}
		}
			
		/// <summary>
		/// 运行状态   取值 正常、异常
		/// </summary>	
		[LogInfo("运行状态   取值 正常、异常")]
		public string Status
		{
			get { return status; }
			set	
			{
				if( value!= null && value.Length > 100)
					throw new ArgumentOutOfRangeException("Invalid value for Status", value, value.ToString());
				
				isChanged |= (status != value); status = value;
			}
		}
			
		/// <summary>
		/// 频率单位（天）
		/// </summary>	
		[LogInfo("频率单位（天）")]
		public double? Frequency
		{
			get { return frequency; }
			set { isChanged |= (frequency != value); frequency = value; }
		}
			
		/// <summary>
		/// 每频次第一天的起始时间
		/// </summary>	
		[LogInfo("每频次第一天的起始时间")]
		public string Starttime
		{
			get { return starttime; }
			set	
			{
				if( value!= null && value.Length > 100)
					throw new ArgumentOutOfRangeException("Invalid value for Starttime", value, value.ToString());
				
				isChanged |= (starttime != value); starttime = value;
			}
		}
			
		/// <summary>
		/// 每频次最后一天的结束时间
		/// </summary>	
		[LogInfo("每频次最后一天的结束时间")]
		public string Endtime
		{
			get { return endtime; }
			set	
			{
				if( value!= null && value.Length > 100)
					throw new ArgumentOutOfRangeException("Invalid value for Endtime", value, value.ToString());
				
				isChanged |= (endtime != value); endtime = value;
			}
		}
			
		/// <summary>
		/// 备注
		/// </summary>	
		[LogInfo("备注")]
		public string Remark
		{
			get { return remark; }
			set	
			{
				if( value!= null && value.Length > 500)
					throw new ArgumentOutOfRangeException("Invalid value for Remark", value, value.ToString());
				
				isChanged |= (remark != value); remark = value;
			}
		}
			
		/// <summary>
		/// 排序
		/// </summary>	
		[LogInfo("排序")]
		public double? Displayorder
		{
			get { return displayorder; }
			set { isChanged |= (displayorder != value); displayorder = value; }
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
		/// 执行代码 一个接口一个DLL，维护命名空间名
		/// </summary>	
		[LogInfo("执行代码 一个接口一个DLL，维护命名空间名")]
		public string Executecode
		{
			get { return executecode; }
			set	
			{
				if( value!= null && value.Length > 100)
					throw new ArgumentOutOfRangeException("Invalid value for Executecode", value, value.ToString());
				
				isChanged |= (executecode != value); executecode = value;
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
