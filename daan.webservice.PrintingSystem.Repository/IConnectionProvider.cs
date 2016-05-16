using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace daan.webservice.PrintingSystem.Repository
{
    public interface IConnectionProvider
    {
        /// <summary>
        /// Gets a connection from the persistance layer
        /// </summary>
        /// <returns>the new connection</returns>
        IPersistanceConnection GetConnection();
    }
}