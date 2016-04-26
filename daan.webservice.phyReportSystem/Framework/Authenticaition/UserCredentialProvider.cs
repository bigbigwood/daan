using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using daan.webservice.phyReportSystem.Contract.Messages;

namespace daan.webservice.phyReportSystem.Framework.Authenticaition
{
    public class UserCredentialProvider : IUserCredentialProvider
    {
        public UserCredential GetUserCredential()
        {
            var userCredential = new UserCredential();

            int userCodeHeaderIndex = OperationContext.Current.IncomingMessageHeaders.FindHeader("Username", Declarations.NameSpace);
            if (userCodeHeaderIndex >= 0)
            {
                userCredential.Username = OperationContext.Current.IncomingMessageHeaders.GetHeader<string>(userCodeHeaderIndex).ToString();
            }

            int passWordHeaderIndex = OperationContext.Current.IncomingMessageHeaders.FindHeader("Password", Declarations.NameSpace);
            if (passWordHeaderIndex >= 0)
            {
                userCredential.Password = OperationContext.Current.IncomingMessageHeaders.GetHeader<string>(passWordHeaderIndex).ToString();
            }

            return userCredential;
        }
    }
}