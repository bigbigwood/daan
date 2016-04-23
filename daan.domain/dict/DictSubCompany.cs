using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace daan.domain
{
    [Serializable]
    public sealed class DictSubCompany
    {

        #region Private Members
        private double? _subCompanyId;
        private string _subCompanyName;
        private string _addres;
        private string _phone;
        private double? _displayorder;
        private string _remark;
        private bool _isChanged;
        private bool _isDeleted;
        #endregion

        #region Public Properties

        /// <summary>
        /// 自动生成
        /// </summary>		
        public double? SubCompanyId
        {
            get { return _subCompanyId; }
            set { _isChanged |= (_subCompanyId != value); _subCompanyId = value; }
        }

        /// <summary>
        /// 子公司名称
        /// </summary>		
        public string SubCompanyName
        {
            get { return _subCompanyName; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Invalid value for Labname", value, value.ToString());

                _isChanged |= (_subCompanyName != value); _subCompanyName = value;
            }
        }

        /// <summary>
        /// 地点
        /// </summary>		
        public string Addres
        {
            get { return _addres; }
            set
            {
                if (value != null && value.Length > 500)
                    throw new ArgumentOutOfRangeException("Invalid value for Addres", value, value.ToString());

                _isChanged |= (_addres != value); _addres = value;
            }
        }

        /// <summary>
        /// 联系电话
        /// </summary>		
        public string Phone
        {
            get { return _phone; }
            set
            {
                if (value != null && value.Length > 20)
                    throw new ArgumentOutOfRangeException("Invalid value for Phone", value, value.ToString());

                _isChanged |= (_phone != value); _phone = value;
            }
        }

        /// <summary>
        /// 排序
        /// </summary>		
        public double? Displayorder
        {
            get { return _displayorder; }
            set { _isChanged |= (_displayorder != value); _displayorder = value; }
        }


        /// <summary>
        /// 备注
        /// </summary>		
        public string Remark
        {
            get { return _remark; }
            set
            {
                if (value != null && value.Length > 200)
                    throw new ArgumentOutOfRangeException("Invalid value for Addres", value, value.ToString());

                _isChanged |= (_remark != value); _remark = value;
            }
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
