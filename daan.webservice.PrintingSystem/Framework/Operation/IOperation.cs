namespace daan.webservice.PrintingSystem.Framework.Operation
{
    public interface IOperation<TRequest, TResponse>
    {
        TResponse Process(TRequest request);
    }
}