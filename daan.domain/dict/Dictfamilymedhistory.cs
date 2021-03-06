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
	public sealed class Dictfamilymedhistory:BaseDomain
	{
		#region Private Members
		private bool isChanged;
		private bool isDeleted;
		private double? dictfamilymedhistoryid; 
		private double? dictmemberid; 
		private string diseasename; 
		private DateTime? diseasedate; 
		private string patientname; 
		private string currentsituation; 
		private string medname; 
		private DateTime? createdate; 		
		#endregion
		
		#region Default ( Empty ) Class Constuctor
		/// <summary>
		/// default constructor
		/// </summary>
		public Dictfamilymedhistory()
		{
			dictfamilymedhistoryid = null; 
			dictmemberid = null; 
			diseasename = null; 
			diseasedate = new DateTime(); 
			patientname = null; 
			currentsituation = null; 
			medname = null; 
			createdate = new DateTime(); 
		}
		#endregion // End of Default ( Empty ) Class Constuctor
		
		#region Public Properties
			
		/// <summary>
		/// 即往病史主键
		/// </summary>	
		[LogInfo("即往病史主键")]
		public double? Dictfamilymedhistoryid
		{
			get { return dictfamilymedhistoryid; }
			set { isChanged |= (dictfamilymedhistoryid != value); dictfamilymedhistoryid = value; }
		}
			
		/// <summary>
		/// 
		/// </summary>	
		[LogInfo("")]
		public double? Dictmemberid
		{
			get { return dictmemberid; }
			set { isChanged |= (dictmemberid != value); dictmemberid = value; }
		}
			
		/// <summary>
		/// 疾病ID
		/// </summary>	
		[LogInfo("疾病ID")]
		public string Diseasename
		{
			get { return diseasename; }
			set	
			{
				if( value!= null && value.Length > 200)
					throw new ArgumentOutOfRangeException("Invalid value for Diseasename", value, value.ToString());
				
				isChanged |= (diseasename != value); diseasename = value;
			}
		}
			
		/// <summary>
		/// 患病时间(年)
		/// </summary>	
		[LogInfo("患病时间(年)")]
		public DateTime? Diseasedate
		{
			get { return diseasedate; }
			set { isChanged |= (diseasedate != value); diseasedate = value; }
		}
			
		/// <summary>
		/// 病人姓名
		/// </summary>	
		[LogInfo("病人姓名")]
		public string Patientname
		{
			get { return patientname; }
			set	
			{
				if( value!= null && value.Length > 200)
					throw new ArgumentOutOfRangeException("Invalid value for Patientname", value, value.ToString());
				
				isChanged |= (patientname != value); patientname = value;
			}
		}
			
		/// <summary>
		/// 现在情况
		/// </summary>	
		[LogInfo("现在情况")]
		public string Currentsituation
		{
			get { return currentsituation; }
			set	
			{
				if( value!= null && value.Length > 200)
					throw new ArgumentOutOfRangeException("Invalid value for Currentsituation", value, value.ToString());
				
				isChanged |= (currentsituation != value); currentsituation = value;
			}
		}
			
		/// <summary>
		/// 药品名称
		/// </summary>	
		[LogInfo("药品名称")]
		public string Medname
		{
			get { return medname; }
			set	
			{
				if( value!= null && value.Length > 200)
					throw new ArgumentOutOfRangeException("Invalid value for Medname", value, value.ToString());
				
				isChanged |= (medname != value); medname = value;
			}
		}
			
		/// <summary>
		/// 创建时间
		/// </summary>	
		[LogInfo("创建时间")]
		public DateTime? Createdate
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
