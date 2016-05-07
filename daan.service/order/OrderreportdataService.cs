using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using daan.domain;
using daan.service.common;

namespace daan.service.order
{
    public class OrderreportdataService : BaseService
    {
        public bool AddOrderreportdata(Orderreportdata report)
        {
            try
            {
                var tryGetReport = GetOrderreportdata(report.Ordernum);
                if (tryGetReport != null)
                {
                    update("Order.UpdateOrderreportdata", report);
                }
                else
                {
                    report.Orderreportdataid = getSeqID("SEQ_ORDERREPORTDATA");
                    insert("Order.InsertOrderreportdata", report);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public Orderreportdata GetOrderreportdata(string orderNumber)
        {
            try
            {
                var reportData = selectObj<Orderreportdata>("Order.SelectOrderreportdataByOrdernum", orderNumber);
                return reportData;
            }
            catch
            {
                return null;
            }
        }
    }
}
