using System;
using System.Collections.Generic;
using System.Linq;
using daan.webservice.PrintingSystem.Contract.Messages;
using daan.webservice.PrintingSystem.Framework.Operation;
using System.Collections;
using daan.service.order;
using log4net;

namespace daan.webservice.PrintingSystem.Operations
{
    public class UpdateOrdersStatusOp : IOperation<UpdateOrdersStatusRequest, UpdateOrdersStatusResponse>
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly OrdersService ordersService = new OrdersService();

        public UpdateOrdersStatusResponse Process(UpdateOrdersStatusRequest request)
        {
            List<String> messages = new List<string>();

            foreach (var orderTransition in request.OrderTransitions)
            {
                Hashtable ht = new Hashtable();
                ht.Add("ordernum", orderTransition.OrderNumber);
                ht.Add("oldstatus", (int)orderTransition.CurrentStatus);
                ht.Add("status", (int)orderTransition.NewStatus);
                bool singleUpdateOrderResult = ordersService.EditStatusByOldStatus(ht);
                if (singleUpdateOrderResult == false)
                {
                    string message = String.Format("{0}:{1}", orderTransition.OrderNumber, singleUpdateOrderResult.ToString());
                    Log.Warn(message);
                    messages.Add(message);
                }

                ordersService.AddOperationLog(orderTransition.OrderNumber, null, "报告单集中打印", "新版打印报告单", "修改留痕", "");
            }

            if (messages.Any())
            {
                return new UpdateOrdersStatusResponse() { ResultType = ResultTypes.PartiallyOk, Messages = messages.ToArray() };
            }
            else
            {
                return new UpdateOrdersStatusResponse() { ResultType = ResultTypes.Ok };
            }
        }


    }
}