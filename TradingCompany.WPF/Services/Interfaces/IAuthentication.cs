namespace TradingCompany.WPF.Services.Interfaces
{
    public interface IAuthentication
    {
        bool ValidateUser(string login, string password, out string role);
    }
}