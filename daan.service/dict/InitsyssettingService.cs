using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using daan.domain;
using daan.service.common;

namespace daan.service.dict
{
    public class InitsyssettingService : BaseService
    {
        /// <summary>
        /// 获得当前登录用户所拥有的资源
        /// </summary>
        /// <returns></returns>
        public IList<Initsyssetting> GetInitSysSetting()
        {
            return this.QueryList<Initsyssetting>("Dict.SelectInitsyssetting", null);
        }
    }
}
