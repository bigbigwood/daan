using System;
using System.Collections.Generic;
using System.Linq;
using daan.domain;
using daan.service.dict;
using daan.webservice.PrintingSystem.Contract.Messages;
using daan.webservice.PrintingSystem.Framework.Operation;
using System.Collections;
using daan.service.order;
using daan.webservice.PrintingSystem.Repository;
using daan.webservice.PrintingSystem.Repository.Interfaces;
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

            var orderRepo = RepositoryManager.GetRepository<IOrderRepository>();
            var userRepo = RepositoryManager.GetRepository<IDictUserRepository>();
            var dictUser = userRepo.GetByUserCode(request.Username);
            if (dictUser == null)
            {
                throw new Exception("");
            }
            int operaterid = dictUser.Dictuserid == 0 ? 4 : (int)dictUser.Dictuserid;
            string operatername = string.IsNullOrEmpty(dictUser.Username) ? "admin" : dictUser.Username;

            foreach (var orderTransition in request.OrderTransitions)
            {
                bool singleUpdateOrderResult = orderRepo.UpdateOrderStatus(orderTransition.OrderNumber, ((int)orderTransition.NewStatus).ToString());
                if (singleUpdateOrderResult == false)
                {
                    string message = String.Format("{0}:{1}", orderTransition.OrderNumber, singleUpdateOrderResult.ToString());
                    Log.Warn(message);
                    messages.Add(message);
                }

                AddOperationLog(orderTransition.OrderNumber, null, "报告单集中打印", "新版打印报告单", "修改留痕", "", operatername, operaterid);
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


        public void AddOperationLog(string orderNum, string barCode, string moduleName, string content, string operationType, string remark, string operatername, int operaterid)
        {
            var operationLogRepo = RepositoryManager.GetRepository<IOperationLogRepository>();
            var sequenceProvider = RepositoryManager.GetRepository<ISequenceProvider>();

            var log = new Operationlog();
            log.Operationid = sequenceProvider.GetNextSequence("seq_OperationLog");
            log.Ordernum = orderNum;
            log.Barcode = barCode;
            log.Modulename = moduleName;
            log.Createdate = DateTime.Now;
            log.Operationtype = operationType;
            log.Operatername = operatername;
            log.Operaterid = operaterid;
            log.Content = content;
            log.Remark = remark;
            operationLogRepo.Insert(log);
        }
    }
}