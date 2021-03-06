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
	public sealed class Orderresultcomment:BaseDomain
	{
		#region Private Members
		private bool isChanged;
		private bool isDeleted;
		private double? orderresultcommentid; 
		private string ordernum; 
		private string engresultcomment; 
		private string engresultsuggestion; 
		private string resultcomment; 
		private string resultsuggestion; 
		private DateTime createdate; 		
		#endregion
		
		#region Default ( Empty ) Class Constuctor
		/// <summary>
		/// default constructor
		/// </summary>
		public Orderresultcomment()
		{
			orderresultcommentid = null; 
			ordernum = string.Empty;
            engresultcomment = string.Empty;
            engresultsuggestion = string.Empty;
            resultcomment = string.Empty;
            resultsuggestion = string.Empty; 
			createdate = new DateTime(); 
		}
		#endregion // End of Default ( Empty ) Class Constuctor
		
		#region Public Properties
			
		/// <summary>
		/// 自动生成
		/// </summary>	
		[LogInfo("自动生成")]
		public double? Orderresultcommentid
		{
			get { return orderresultcommentid; }
			set { isChanged |= (orderresultcommentid != value); orderresultcommentid = value; }
		}
			
		/// <summary>
		/// 体检流水号
		/// </summary>	
		[LogInfo("体检流水号")]
		public string Ordernum
		{
			get { return ordernum; }
			set	
			{
				if( value!= null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for Ordernum", value, value.ToString());
				
				isChanged |= (ordernum != value); ordernum = value;
			}
		}
			
		/// <summary>
		/// 结果评价、小结,异常结果小结(英文)
		/// </summary>	
		[LogInfo("结果评价、小结,异常结果小结(英文)")]
		public string Engresultcomment
		{
			get { return engresultcomment; }
			set	
			{
				if( value!= null && value.Length > 4000)
					throw new ArgumentOutOfRangeException("Invalid value for Engresultcomment", value, value.ToString());
				
				isChanged |= (engresultcomment != value); engresultcomment = value;
			}
		}
			
		/// <summary>
		/// 专家建议（英文）
		/// </summary>	
		[LogInfo("专家建议（英文）")]
		public string Engresultsuggestion
		{
			get { return engresultsuggestion; }
			set	
			{
				if( value!= null && value.Length > 4000)
					throw new ArgumentOutOfRangeException("Invalid value for Engresultsuggestion", value, value.ToString());
				
				isChanged |= (engresultsuggestion != value); engresultsuggestion = value;
			}
		}
			
		/// <summary>
		/// 结果评价、小结,异常结果小结
		/// </summary>	
		[LogInfo("结果评价、小结,异常结果小结")]
		public string Resultcomment
		{
			get { return resultcomment; }
			set	
			{
				if( value!= null && value.Length > 4000)
					throw new ArgumentOutOfRangeException("Invalid value for Resultcomment", value, value.ToString());
				
				isChanged |= (resultcomment != value); resultcomment = value;
			}
		}
			
		/// <summary>
		/// 专家建议
		/// </summary>	
		[LogInfo("专家建议")]
		public string Resultsuggestion
		{
			get { return resultsuggestion; }
			set	
			{
				if( value!= null && value.Length > 4000)
					throw new ArgumentOutOfRangeException("Invalid value for Resultsuggestion", value, value.ToString());
				
				isChanged |= (resultsuggestion != value); resultsuggestion = value;
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
