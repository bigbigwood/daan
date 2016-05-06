﻿using daan.webservice.PrintingSystem.Contract.Models.User;

namespace daan.ui.PrintingApplication
{
    public class PrinterApp
    {
        static PrinterApp()
        {
            CurrentUserInfo = MockUserInfo();
        }

        public static UserInfo CurrentUserInfo { get; set; }

        private static UserInfo MockUserInfo()
        {
            return new UserInfo()
            {
                UserPrinterConfig = new UserPrinterConfig() { A4Printer = "Microsoft XPS Document Writer", A5Printer = "HP 910", BarcodePrinter = "Fax", PdfPrinter = "OFax" }
            };
        }
    }
}