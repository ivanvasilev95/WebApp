namespace WebBazar.API.Services.Interfaces
{
    public interface ICurrentUserService
    {
        string GetUserName();
        int GetId();
    }
}