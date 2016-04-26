namespace daan.webservice.phyReportSystem.Framework.Operation
{
    public interface IOperation<TRequest, TResponse>
    {
        TResponse Process(TRequest request);
    }
}