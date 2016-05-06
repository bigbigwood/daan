namespace daan.webservice.PrintingSystem.Framework.Authenticaition
{
    public interface IAuthenticaitionService
    {
        AuthenticaitionResultCode Authenticate(string username, string password);
    }

    public enum AuthenticaitionResultCode
    {
        Ok,
        UserOrPasswordIsEmpty,
        UserOrPasswordIsIncorrect,
        UserIsNotExisting,
        PasswordIsIncorrect,
        Error,
    }
}