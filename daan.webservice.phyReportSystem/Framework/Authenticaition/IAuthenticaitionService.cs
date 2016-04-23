using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace daan.webservice.phyReportSystem.Framework.Authenticaition
{
    public interface IAuthenticaitionService
    {
        bool Authenticate(UserCredential userCredential);
    }
}