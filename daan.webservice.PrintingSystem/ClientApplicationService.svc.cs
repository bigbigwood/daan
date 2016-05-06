﻿using daan.webservice.PrintingSystem.Contract.Interface;
using daan.webservice.PrintingSystem.Contract.Messages;
using daan.webservice.PrintingSystem.Framework;
using daan.webservice.PrintingSystem.Operations;

namespace daan.webservice.PrintingSystem
{
    public class ClientApplicationService : IClientApplicationServiceContract
    {
        public AuthenticateResponse Authenticate(AuthenticateRequest request)
        {
            return MessageProcessor.Process(request, new AuthenticateOp());
        }

        public GetLastClientAppVersionsResponse GetLastClientAppVersions(GetLastClientAppVersionsRequest request)
        {
            return MessageProcessor.Process(request, new GetLastClientAppVersionOp());
        }

        public GetReportTemplatesResponse GetReportTemplates(GetReportTemplatesRequest request)
        {
            return MessageProcessor.Process(request, new GetReportTemplatesOp());
        }
    }
}