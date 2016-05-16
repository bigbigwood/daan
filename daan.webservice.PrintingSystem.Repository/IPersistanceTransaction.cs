using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace daan.webservice.PrintingSystem.Repository
{
    public interface IPersistanceTransaction : IDisposable
    {
        /// <summary>
        /// Commits changes done in the in the repositories
        /// </summary>
        void Commit();

        /// <summary>
        /// Rollback the changes done in the repositories
        /// </summary>
        void Rollback();
    }
}