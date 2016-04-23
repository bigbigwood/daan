using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace daan.util.Web
{
    /// <summary>
    /// 获取分页首值和尾值
    /// </summary>
    public class PageUtil
    {
        protected int pageIndex = 0;
        protected int pageSize = 20;

        public PageUtil(int pageIndex, int pageSize)
        {
            this.pageIndex = pageIndex;
            this.pageSize = pageSize == 0 ? 20 : pageSize;
        }
        #region >>>获取分页首值与尾值
        /// <summary>
        /// 获取分页首值
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public int GetPageStartNum()
        {
            return pageIndex * pageSize + 1;
        }
        /// <summary>
        /// 获取分页尾值
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public int GetPageEndNum()
        {
            return pageIndex * pageSize + pageSize;
        }
        #endregion
    }
}
