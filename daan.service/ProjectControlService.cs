using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using daan.domain;
using System.Collections;
using daan.util.Common;
using daan.service.common;

namespace daan.service
{
    /// <summary>
    /// 项目对照  服务层 
    /// 2015-09-16  李国庆
    /// </summary>
    public class ProjectControlService : BaseService
    {
        /// <summary>
        /// 通过事物将 项目对照的数据集 插入到数据库当中 并删除所有的原有数据
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool InsertProjectControl(IList<ProjectControl> list, ref string error)
        {
            SortedList SQLlist = new SortedList(new MySort());
            bool result = false;
            if (list != null && list.Count > 0)
            {
                SQLlist.Add(new Hashtable() { { "DELETE", "Dict.DeleteProjectControl" } }, null);
                foreach (ProjectControl item in list)
                {
                    SQLlist.Add(new Hashtable() { { "INSERT", "Dict.InsertProjectControl" } }, item);
                }
                result = ExecuteSqlTran(SQLlist, ref error);
            }
            return result;
        }
        /// <summary>
        /// 根据UNIQUECODE查询是否有对照记录
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public bool GetProjectControlCountByUniquecode(Hashtable ht)
        {
            return Convert.ToInt32(selectDS("Dict.GetProjectControlCountByUniquecode", ht).Tables[0].Rows[0][0]) > 0;
        }
    }
}
