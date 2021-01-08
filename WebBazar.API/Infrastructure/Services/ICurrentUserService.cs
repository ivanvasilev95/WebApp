namespace WebBazar.API.Infrastructure.Services
{
    public interface ICurrentUserService
    {
        string GetUserName();
        int GetId();
    }
}