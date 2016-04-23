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
	public sealed class Dictruleformular:BaseDomain
	{
		#region Private Members
		private bool isChanged;
		private bool isDeleted;
		private double? dictruleformularid; 
		private double? dictdiagnosisid; 
		private double? dictlabdeptid; 
		private string formular; 
		private string formulardesc; 
		private string remark; 
		private DateTime createdate; 
		private double? agelow; 
		private double? agehight; 
		private string ageunit; 
		private double? caculatedagelow; 
		private double? caculatedagehigh; 
		private string sex; 
		private string ismarry; 
		private double? displayorder; 
		private string formularname; 
		private string formulartests; 
		private string sourcecode;
        private string dictrulecode;
        private double? dictlabid;
      
		#endregion
		
		#region Default ( Empty ) Class Constuctor
		/// <summary>
		/// default constructor
		/// </summary>
		public Dictruleformular()
		{
			dictruleformularid = null; 
			dictdiagnosisid = null; 
			dictlabdeptid = null; 
			formular = null; 
			formulardesc = null; 
			remark = null; 
			createdate = new DateTime(); 
			agelow = null; 
			agehight = null; 
			ageunit = null; 
			caculatedagelow = null; 
			caculatedagehigh = null; 
			sex = null; 
			ismarry = null; 
			displayorder = null; 
			formularname = null; 
			formulartests = null; 
			sourcecode = null;
            dictrulecode = null;
            dictlabid = 0;
		}
		#endregion // End of Default ( Empty ) Class Constuctor
		
		#region Public Properties
			
		/// <summary>
		/// 主键
		/// </summary>	
		[LogInfo("主键")]
		public double? Dictruleformularid
		{
			get { return dictruleformularid; }
			set { isChanged |= (dictruleformularid != value); dictruleformularid = value; }
		}
			
		/// <summary>
		/// 主键
		/// </summary>	
		[LogInfo("主键")]
		public double? Dictdiagnosisid
		{
			get { return dictdiagnosisid; }
			set { isChanged |= (dictdiagnosisid != value); dictdiagnosisid = value; }
		}
			
		/// <summary>
		/// 测试项物理实验室分组,对应表DictLibrary
		/// </summary>	
		[LogInfo("测试项物理实验室分组,对应表DictLibrary")]
		public double? Dictlabdeptid
		{
			get { return dictlabdeptid; }
			set { isChanged |= (dictlabdeptid != value); dictlabdeptid = value; }
		}
			
		/// <summary>
		/// 公式内容
		/// </summary>	
		[LogInfo("公式内容")]
		public string Formular
		{
			get { return formular; }
			set	
			{
				if( value!= null && value.Length > 500)
					throw new ArgumentOutOfRangeException("Invalid value for Formular", value, value.ToString());
				
				isChanged |= (formular != value); formular = value;
			}
		}
			
		/// <summary>
		/// 公式描述
		/// </summary>	
		[LogInfo("公式描述")]
		public string Formulardesc
		{
			get { return formulardesc; }
			set	
			{
				if( value!= null && value.Length > 500)
					throw new ArgumentOutOfRangeException("Invalid value for Formulardesc", value, value.ToString());
				
				isChanged |= (formulardesc != value); formulardesc = value;
			}
		}
			
		/// <summary>
		/// 公式备注
		/// </summary>	
		[LogInfo("公式备注")]
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
		/// 创建时间
		/// </summary>	
		[LogInfo("创建时间")]
		public DateTime Createdate
		{
			get { return createdate; }
			set { isChanged |= (createdate != value); createdate = value; }
		}
			
		/// <summary>
		/// 起始年龄
		/// </summary>	
		[LogInfo("起始年龄")]
		public double? Agelow
		{
			get { return agelow; }
			set { isChanged |= (agelow != value); agelow = value; }
		}
			
		/// <summary>
		/// 结束年龄
		/// </summary>	
		[LogInfo("结束年龄")]
		public double? Agehight
		{
			get { return agehight; }
			set { isChanged |= (agehight != value); agehight = value; }
		}
			
		/// <summary>
		/// 年龄单位
		/// </summary>	
		[LogInfo("年龄单位")]
		public string Ageunit
		{
			get { return ageunit; }
			set	
			{
				if( value!= null && value.Length > 10)
					throw new ArgumentOutOfRangeException("Invalid value for Ageunit", value, value.ToString());
				
				isChanged |= (ageunit != value); ageunit = value;
			}
		}
			
		/// <summary>
		/// 年龄计算值--低值（小时）
		/// </summary>	
		[LogInfo("年龄计算值--低值（小时）")]
		public double? Caculatedagelow
		{
			get { return caculatedagelow; }
			set { isChanged |= (caculatedagelow != value); caculatedagelow = value; }
		}
			
		/// <summary>
		/// 年龄计算值--高值（小时）
		/// </summary>	
		[LogInfo("年龄计算值--高值（小时）")]
		public double? Caculatedagehigh
		{
			get { return caculatedagehigh; }
			set { isChanged |= (caculatedagehigh != value); caculatedagehigh = value; }
		}
			
		/// <summary>
		/// 性别：男/女/空白
		/// </summary>	
		[LogInfo("性别：男/女/空白")]
		public string Sex
		{
			get { return sex; }
			set	
			{
				if( value!= null && value.Length > 10)
					throw new ArgumentOutOfRangeException("Invalid value for Sex", value, value.ToString());
				
				isChanged |= (sex != value); sex = value;
			}
		}
			
		/// <summary>
		/// 结婚状态：0 未婚；1已婚；2全部
		/// </summary>	
		[LogInfo("结婚状态：0 未婚；1已婚；2全部")]
		public string Ismarry
		{
			get { return ismarry; }
			set	
			{
				if( value!= null && value.Length > 1)
					throw new ArgumentOutOfRangeException("Invalid value for Ismarry", value, value.ToString());
				
				isChanged |= (ismarry != value); ismarry = value;
			}
		}
			
		/// <summary>
		/// 次序
		/// </summary>	
		[LogInfo("次序")]
		public double? Displayorder
		{
			get { return displayorder; }
			set { isChanged |= (displayorder != value); displayorder = value; }
		}
			
		/// <summary>
		/// 公式名称
		/// </summary>	
		[LogInfo("公式名称")]
		public string Formularname
		{
			get { return formularname; }
			set	
			{
				if( value!= null && value.Length > 100)
					throw new ArgumentOutOfRangeException("Invalid value for Formularname", value, value.ToString());
				
				isChanged |= (formularname != value); formularname = value;
			}
		}
			
		/// <summary>
		/// 运算项目，用逗号隔开的项目ID
		/// </summary>	
		[LogInfo("运算项目，用逗号隔开的项目ID")]
		public string Formulartests
		{
			get { return formulartests; }
			set	
			{
				if( value!= null && value.Length > 500)
					throw new ArgumentOutOfRangeException("Invalid value for Formulartests", value, value.ToString());
				
				isChanged |= (formulartests != value); formulartests = value;
			}
		}
			
		/// <summary>
		/// 公式对应的C#代码
		/// </summary>	
		[LogInfo("公式对应的C#代码")]
		public string Sourcecode
		{
			get { return sourcecode; }
			set	
			{
				if( value!= null && value.Length > 4000)
					throw new ArgumentOutOfRangeException("Invalid value for Sourcecode", value, value.ToString());
				
				isChanged |= (sourcecode != value); sourcecode = value;
			}
		}
        /// <summary>
        /// 规则代码
        /// </summary>
        [LogInfo("规则代码")]
        public string Dictrulecode
        {
            get { return dictrulecode; }
            set { dictrulecode = value; }
        }

        /// <summary>
        /// 主键
        /// </summary>	
        [LogInfo("分点ID，0为通用规则公式")]
        public double? Dictlabid
        {
            get { return dictlabid; }
            set { isChanged |= (dictlabid != value); dictlabid = value; }
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
