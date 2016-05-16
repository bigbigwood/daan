using System;
using System.Data;
using daan.service.order;
using daan.webservice.PrintingSystem.Contract.Messages;
using daan.webservice.PrintingSystem.Framework.Operation;
using daan.webservice.PrintingSystem.Repository;
using daan.webservice.PrintingSystem.Repository.Interfaces;

namespace daan.webservice.PrintingSystem.Operations
{
    public class QueryOrdersOp : IOperation<QueryOrdersRequest, QueryOrdersResponse>
    {
        public QueryOrdersResponse Process(QueryOrdersRequest request)
        {
            var response = new QueryOrdersResponse() {ResultType = ResultTypes.Ok};
            var ordersService = new OrdersService();
            var ordersRepo = RepositoryManager.GetRepository<IOrderRepository>();

            if (!string.IsNullOrWhiteSpace(request.OrderNumber))
            {
                var dt = ordersRepo.QueryOrderReportSummaryByOrderNum(request.OrderNumber);
                foreach (DataRow row in dt.Rows)
                    row["AGE"] = GetAge(row["AGE"]);

                response.Result = dt;
                response.OrderCount = response.Result.Rows.Count;
            }
            else if (!string.IsNullOrWhiteSpace(request.Barcode))
            {
                var orderBarcodeRepo = RepositoryManager.GetRepository<IOrderBarcodeRepository>();
                var orderBarcodeInfo = orderBarcodeRepo.GetByBarcode(request.Barcode);
                if (orderBarcodeInfo != null)
                {
                    var dt = ordersRepo.QueryOrderReportSummaryByOrderNum(orderBarcodeInfo.Ordernum);
                    foreach (DataRow row in dt.Rows)
                        row["AGE"] = GetAge(row["AGE"]);

                    response.Result = dt;
                    response.OrderCount = response.Result.Rows.Count;
                }
                else
                {
                    response.OrderCount = 0;
                }
            }
            else
            {
                var htPara = new System.Collections.Hashtable();
                htPara.Add("pageStart", request.PageStart);
                htPara.Add("pageEnd", request.PageEnd);
                htPara.Add("dictlabid", request.Dictlabid); // 分点
                htPara.Add("dictcustomerid", request.Dictcustomerid); //体检单位
                htPara.Add("StartDate", request.StartDate);
                htPara.Add("EndDate", request.EndDate);
                htPara.Add("SamplingDateBegin", request.SamplingDateBegin);
                htPara.Add("SamplingDateEnd", request.SamplingDateEnd);
                htPara.Add("status", request.OrderStatus); ;
                htPara.Add("name", request.Keyword);
                htPara.Add("reportstatus", request.ReportStatus);

                response.OrderCount = ordersService.DataForFocusPrintPageTotal(htPara);
                response.Result = ordersService.DataForFocusPrintPageLst(htPara);
            }

            return response;
        }





        /// <summary>
        /// 截取年龄 >=5岁不取月日时，＜5岁时才取月日时
        /// </summary>
        /// <param name="objage">数据库取的Age字段值</param>
        /// <returns></returns>
        public static string GetAge(object objage)
        {
            string age = string.Empty;
            try
            {
                if (objage.ToString() == string.Empty) { return string.Empty; }
                if (objage.ToString().Contains("成人")) { return objage.ToString(); }
                string[] strage = objage.ToString().Split('岁');
                int year = Convert.ToInt32(strage[0]);
                if (year >= 5) { age = strage[0] + "岁"; }
                else
                {

                    if (strage[0] != "0") { age += strage[0] + "岁"; }

                    string[] strmonth = strage[1].Split('月');
                    int month = Convert.ToInt32(strmonth[0]);
                    if (month > 0) { age += month + "月"; }

                    string[] strday = strmonth[1].Split('日');
                    int day = Convert.ToInt32(strday[0]);
                    if (day > 0) { age += day + "日"; }

                    string[] strhour = strday[1].Split('时');
                    int hour = Convert.ToInt32(strhour[0]);
                    if (hour > 0) { age += hour + "时"; }

                    if (age == string.Empty) { age = "0岁"; }
                }
            }
            catch (Exception)
            {

                throw new Exception("年龄格式转换错误！");
            }

            return age;
        }

    }
}