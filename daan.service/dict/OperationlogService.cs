using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using daan.domain;
using daan.service.common;

namespace daan.service.dict
{
    public class OperationlogService : BaseService
    {
        /// <summary>
        /// /根据体检流水号查询操作日志
        /// </summary>
        /// <param name="ordernum">体检流水号</param>
        /// <returns></returns>
        public IList<Operationlog> SelectOperationlogByOrdernum(object param)
        {
            try
            {
                return this.QueryList<Operationlog>("dict.SelectOperationlogByOrdernum", param);
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 根据体检流水号查询操作日志总数
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public int SelectOperationlogCountByOrdernum(string ordernum)
        {
            try
            {
                return int.Parse(this.selectIList("dict.SelectOperationlogCountByOrdernum", ordernum)[0].ToString());
            }
            catch
            {
                return -1;
            }
        }
    }
}
