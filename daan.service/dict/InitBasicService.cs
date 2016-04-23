using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using daan.service.common;
using System.Data;
using daan.domain;

namespace daan.service.dict
{
    public class InitBasicService : BaseService
    {
        #region >>>> 按基本资料类别获取基础资料数据
        /// <summary>
        /// 按基本资料类别获取基础资料数据
        /// </summary>
        /// <param name="strkey"></param>
        /// <returns></returns>
        public List<Initbasic> GetInitbasicLst(string strkey)
        {
            return this.QueryList<Initbasic>("Dict.GetInitbasicLst", strkey).ToList<Initbasic>();
        }
        #endregion
    }
}
