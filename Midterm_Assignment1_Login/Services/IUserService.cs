using YourApp.Models;

namespace YourApp.Services
{
    public interface IUserService
    {
        bool RegisterUser(RegisterModel model);
        bool AuthenticateUser(LoginModel model, out UserModel user);
    }
}