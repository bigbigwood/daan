using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;
using daan.service.common;

namespace daan.ui.LisToshequ
{
    public class SqlService : BaseService
    {
        #region >>>>
        /// <summary>
        /// 获得体检信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetGetPEEntity()
        {
            return selectDS("Common.GetPEEntity", null).Tables[0];
        }


        /// <summary>
        /// 获取体检报告
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public DataTable GetPEReportEntity(Hashtable ht)
        {
            return selectDS("Common.GetPEReportEntity", ht).Tables[0];
        }


        /// <summary>
        /// 如果reportoption = 0
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public DataTable GetGetPEItemEntityOne(Hashtable ht)
        {
            return selectDS("Common.GetPEItemEntityOne", ht).Tables[0];
        }


        /// <summary>
        /// 如果reportoption = 1
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public DataTable GetPEItemEntityTwo(Hashtable ht)
        {
            return selectDS("Common.GetPEItemEntityTwo", ht).Tables[0];
        }

        /// <summary>
        /// 如果reportoption =2
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public DataTable GetPEItemEntityThree(Hashtable ht)
        {
            return selectDS("Common.GetPEItemEntityThree", ht).Tables[0];
        }

        public DataTable GetFilename(Hashtable ht)
        {
            return selectDS("Common.GetFilename", ht).Tables[0];
        }


        /// <summary>
        /// 根据条码号修改是否上传状态
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public bool EditSqflag(Hashtable ht)
        {
            return update("Common.UpdateSqflag", ht) > 0;
        }
        #endregion
    }
}
