using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace daan.domain
{
    [Serializable]
    public class DictModule : BaseDomain
    {
        private int id;
        private int category_id;
        private string category_name;
        private string per_name;

        private int levelid;
        private int permissions_id;
        private string frmnamespace;
        private string frmclassname;
        private int ordernum;
        private char isclock;
        private string categorycode;


        #region Default ( Empty ) Class DictModule
		/// <summary>
		/// default constructor
		/// </summary>
        public DictModule()
		{
			id = 0;
            category_id = 0;
            category_name = null;
            per_name = null;            
            levelid = 0;
            permissions_id = 0;
            frmnamespace = null;
            frmclassname = null;
            ordernum = 0;
            isclock = '0';
		}
		#endregion // End of Default ( Empty ) Class Constuctor
       /// <summary>
       /// ID
       /// </summary>
        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }
        /// <summary>
        /// 分类ID
        /// </summary>
        public int CategoryId
        {
            get { return this.category_id; }
            set { this.category_id = value; }
        }
        /// <summary>
        /// 分类名称
        /// </summary>
        public string CategoryName
        {
            get { return this.category_name; }
            set { this.category_name = value; }
        }
        /// <summary>
        /// 名称
        /// </summary>
        public string PerName
        {
            get { return this.per_name; }
            set { this.per_name = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int LevelId
        {
            get { return this.levelid; }
            set { this.levelid = value; }
        }
        /// <summary>
        /// 菜单ID
        /// </summary>
        public int PermissionsId
        {
            get { return this.permissions_id; }
            set { this.permissions_id = value; }
        }
        /// <summary>
        /// 窗体命名空间
        /// </summary>
        public string FrmnameSpace
        {
            get { return this.frmnamespace; }
            set { this.frmnamespace = value; }
        }
        /// <summary>
        /// 窗体类名
        /// </summary>
        public string FrmclassName
        {
            get { return this.frmclassname; }
            set { this.frmclassname = value; }
        }
        /// <summary>
        /// 显示顺序
        /// </summary>
        public int OrderNum
        {
            get { return this.ordernum; }
            set { this.ordernum = value; }
        }
        /// <summary>
        /// 是否关闭
        /// </summary>
        public char IsClock
        {
            get { return this.isclock; }
            set { this.isclock = value; }
        }
        /// <summary>
        /// 父级菜单关键词
        /// </summary>
        public string CategoryCode
        {
            get { return this.categorycode; }
            set { this.categorycode = value; }
        }
       
    }
}
