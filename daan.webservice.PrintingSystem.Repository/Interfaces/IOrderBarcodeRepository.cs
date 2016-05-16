using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using daan.domain;
using IBatisNet.DataMapper.Configuration.Statements;

namespace daan.webservice.PrintingSystem.Repository.Interfaces
{
    public interface IOrderBarcodeRepository : IRepository<Orderbarcode, Int32>
    {
        Orderbarcode GetByBarcode(string barcode);
    }
}