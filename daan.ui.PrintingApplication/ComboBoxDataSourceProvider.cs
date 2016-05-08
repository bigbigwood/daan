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
                new EnumEntity() {EnumValue = -1, EnumText ="All", EnumDisplayText = "全部"},
                new EnumEntity() {EnumValue = (int)OrdersStatus.Register, EnumText =OrdersStatus.Register.ToString(), EnumDisplayText = "已登记"},
                new EnumEntity() {EnumValue = (int)OrdersStatus.BarCodePrint, EnumText =OrdersStatus.BarCodePrint.ToString(), EnumDisplayText = "条码已打印"},
                new EnumEntity() {EnumValue = (int)OrdersStatus.WaitCheck, EnumText =OrdersStatus.WaitCheck.ToString(), EnumDisplayText = "待总检"},
                new EnumEntity() {EnumValue = (int)OrdersStatus.FirstCheck, EnumText =OrdersStatus.FirstCheck.ToString(), EnumDisplayText = "初步总检"},
                new EnumEntity() {EnumValue = (int)OrdersStatus.FinishCheck, EnumText =OrdersStatus.FinishCheck.ToString(), EnumDisplayText = "完成总检"},
                new EnumEntity() {EnumValue = (int)OrdersStatus.FinishPrint, EnumText =OrdersStatus.FinishPrint.ToString(), EnumDisplayText = "报告已打印"},
            };
        }

        public List<EnumEntity> GetReportStatusDataSource()
        {
            return new List<EnumEntity>()
            {
                new EnumEntity() {EnumValue = -1, EnumText ="All", EnumDisplayText = "全部"},
                new EnumEntity() {EnumValue = (int)ReportStatus.Normal, EnumText =ReportStatus.Normal.ToString(), EnumDisplayText = "正常"},
                new EnumEntity() {EnumValue = (int)ReportStatus.Delay, EnumText =ReportStatus.Delay.ToString(), EnumDisplayText = "迟发"},
                new EnumEntity() {EnumValue = (int)ReportStatus.Refund, EnumText =ReportStatus.Refund.ToString(), EnumDisplayText = "退单"},
            };
        }

        //public List<EnumEntity> GetDataSource(Type enumType)
        //{
        //    List<EnumEntity> data = new List<EnumEntity>();

        //    var enumValues = Enum.GetValues(enumType);

        //    FieldInfo[] fields = enumType.GetFields();
        //    foreach (var fieldInfo in fields)
        //    {
        //        DescriptionAttribute[] arrDesc = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
        //        string desc = arrDesc[0].Description;
        //    }

        //    return data;
        //}

    }
}
