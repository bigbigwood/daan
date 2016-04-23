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
	public sealed class Customervaliddiagnosis:BaseDomain
	{
		#region Private Members
		private bool isChanged;
		private bool isDeleted;
		private double? customervaliddiagnosisid; 
		private double? dictcustomerid; 
		private string ordersyear; 
		private double? dictdiagnosisid; 
		private DateTime createdate; 		
		#endregion
		
		#region Default ( Empty ) Class Constuctor
		/// <summary>
		/// default constructor
		/// </summary>
		public Customervaliddiagnosis()
		{
			customervaliddiagnosisid = null; 
			dictcustomerid = null; 
			ordersyear = null; 
			dictdiagnosisid = null;
            createdate = DateTime.Now;
		}
		#endregion // End of Default ( Empty ) Class Constuctor
		
		#region Public Properties
			
		/// <summary>
		/// 主键
		/// </summary>	
		[LogInfo("主键")]
		public double? Customervaliddiagnosisid
		{
			get { return customervaliddiagnosisid; }
			set { isChanged |= (customervaliddiagnosisid != value); customervaliddiagnosisid = value; }
		}
			
		/// <summary>
		/// 主键，自动ID
		/// </summary>	
		[LogInfo("主键，自动ID")]
		public double? Dictcustomerid
		{
			get { return dictcustomerid; }
			set { isChanged |= (dictcustomerid != value); dictcustomerid = value; }
		}
			
		/// <summary>
		/// 体检年度
		/// </summary>	
		[LogInfo("体检年度")]
		public string Ordersyear
		{
			get { return ordersyear; }
			set	
			{
				if( value!= null && value.Length > 10)
					throw new ArgumentOutOfRangeException("Invalid value for Ordersyear", value, value.ToString());
				
				isChanged |= (ordersyear != value); ordersyear = value;
			}
		}
			
		/// <summary>
		/// 诊断ID
		/// </summary>	
		[LogInfo("诊断ID")]
		public double? Dictdiagnosisid
		{
			get { return dictdiagnosisid; }
			set { isChanged |= (dictdiagnosisid != value); dictdiagnosisid = value; }
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
