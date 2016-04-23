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
	public sealed class Billtrace:BaseDomain  
	{
		#region Private Members
		private bool isChanged;
		private bool isDeleted;
		private double? billtraceid; 
		private double? billheadid; 
		private string barcode; 
		private double? dicttestitem; 
		private double? originalprice; 
		private double? finalprice; 
		private string remark; 
		private double? operateby; 
		private DateTime createdate; 		
		#endregion
		
		#region Default ( Empty ) Class Constuctor
		/// <summary>
		/// default constructor
		/// </summary>
		public Billtrace()
		{
			billtraceid = null; 
			billheadid = null; 
			barcode = null; 
			dicttestitem = null; 
			originalprice = null; 
			finalprice = null; 
			remark = null; 
			operateby = null; 
			createdate = new DateTime(); 
		}
		#endregion // End of Default ( Empty ) Class Constuctor
		
		#region Public Properties
			
		/// <summary>
		/// 主键，序列
		/// </summary>	
		[LogInfo("主键，序列")]
		public double? Billtraceid
		{
			get { return billtraceid; }
			set { isChanged |= (billtraceid != value); billtraceid = value; }
		}
			
		/// <summary>
		/// 主键，自增长
		/// </summary>	
		[LogInfo("主键，自增长")]
		public double? Billheadid
		{
			get { return billheadid; }
			set { isChanged |= (billheadid != value); billheadid = value; }
		}
			
		/// <summary>
		/// 条码号
		/// </summary>	
		[LogInfo("条码号")]
		public string Barcode
		{
			get { return barcode; }
			set	
			{
				if( value!= null && value.Length > 40)
					throw new ArgumentOutOfRangeException("Invalid value for Barcode", value, value.ToString());
				
				isChanged |= (barcode != value); barcode = value;
			}
		}
			
		/// <summary>
		/// 外键，项目ID
		/// </summary>	
		[LogInfo("外键，项目ID")]
		public double? Dicttestitem
		{
			get { return dicttestitem; }
			set { isChanged |= (dicttestitem != value); dicttestitem = value; }
		}
			
		/// <summary>
		/// 标准价格
		/// </summary>	
		[LogInfo("标准价格")]
		public double? Originalprice
		{
			get { return originalprice; }
			set { isChanged |= (originalprice != value); originalprice = value; }
		}
			
		/// <summary>
		/// 客户定价
		/// </summary>	
		[LogInfo("客户定价")]
		public double? Finalprice
		{
			get { return finalprice; }
			set { isChanged |= (finalprice != value); finalprice = value; }
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
				if( value!= null && value.Length > 200)
					throw new ArgumentOutOfRangeException("Invalid value for Remark", value, value.ToString());
				
				isChanged |= (remark != value); remark = value;
			}
		}
			
		/// <summary>
		/// 操作人
		/// </summary>	
		[LogInfo("操作人")]
		public double? Operateby
		{
			get { return operateby; }
			set { isChanged |= (operateby != value); operateby = value; }
		}
			
		/// <summary>
		/// 记录生成日期
		/// </summary>	
		[LogInfo("记录生成日期")]
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
