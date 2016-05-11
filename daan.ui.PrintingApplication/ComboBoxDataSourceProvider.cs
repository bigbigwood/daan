using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using daan.webservice.PrintingSystem.Contract.Models.Order;
using daan.webservice.PrintingSystem.Contract.Models.Report;

namespace daan.ui.PrintingApplication
{
    public class EnumEntity
    {
        public Int32 EnumValue { get; set; }
        public string EnumText { get; set; }
        public string EnumDisplayText { get; set; }
    }

    public class ComboBoxDataSourceProvider
    {
        public List<EnumEntity> GetOrderStatusDataSource()
        {
            return new List<EnumEntity>()
            {
                new EnumEntity() {EnumValue = -1, EnumText ="All", EnumDisplayText = ConstString.ALL},
                new EnumEntity() {EnumValue = (int)OrdersStatus.Register, EnumText =OrdersStatus.Register.ToString(), EnumDisplayText = ConstString.OrdersStatus_Register},
                new EnumEntity() {EnumValue = (int)OrdersStatus.BarCodePrint, EnumText =OrdersStatus.BarCodePrint.ToString(), EnumDisplayText = ConstString.OrdersStatus_BarCodePrint},
                new EnumEntity() {EnumValue = (int)OrdersStatus.WaitCheck, EnumText =OrdersStatus.WaitCheck.ToString(), EnumDisplayText = ConstString.OrdersStatus_WaitCheck},
                new EnumEntity() {EnumValue = (int)OrdersStatus.FirstCheck, EnumText =OrdersStatus.FirstCheck.ToString(), EnumDisplayText = ConstString.OrdersStatus_FirstCheck},
                new EnumEntity() {EnumValue = (int)OrdersStatus.FinishCheck, EnumText =OrdersStatus.FinishCheck.ToString(), EnumDisplayText = ConstString.OrdersStatus_FinishCheck},
                new EnumEntity() {EnumValue = (int)OrdersStatus.FinishPrint, EnumText =OrdersStatus.FinishPrint.ToString(), EnumDisplayText = ConstString.OrdersStatus_FinishPrint},
            };
        }

        public List<EnumEntity> GetReportStatusDataSource()
        {
            return new List<EnumEntity>()
            {
                new EnumEntity() {EnumValue = -1, EnumText ="All", EnumDisplayText = ConstString.ALL},
                new EnumEntity() {EnumValue = (int)ReportStatus.Normal, EnumText =ReportStatus.Normal.ToString(), EnumDisplayText = ConstString.ReportStatus_Normal},
                new EnumEntity() {EnumValue = (int)ReportStatus.Delay, EnumText =ReportStatus.Delay.ToString(), EnumDisplayText = ConstString.ReportStatus_Delay},
                new EnumEntity() {EnumValue = (int)ReportStatus.Refund, EnumText =ReportStatus.Refund.ToString(), EnumDisplayText = ConstString.ReportStatus_Refund},
            };
        }
    }
}
