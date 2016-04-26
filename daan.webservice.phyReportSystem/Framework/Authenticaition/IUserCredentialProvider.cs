using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace daan.webservice.phyReportSystem.Framework.Authenticaition
{
    public interface IUserCredentialProvider
    {
        UserCredential GetUserCredential();
    }
}
