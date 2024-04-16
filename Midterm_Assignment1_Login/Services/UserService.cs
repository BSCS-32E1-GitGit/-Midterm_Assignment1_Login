using BCrypt.Net;
using YourApp.Models;

namespace YourApp.Services
{
    public class UserService : IUserService
    {
        private readonly IDictionary<string, string> _users;

        public UserService()
        {
            _users = new Dictionary<string, string>();
        }

        public bool RegisterUser(RegisterModel model)
        {
            if (_users.ContainsKey(model.Username))
                return false;

            _users.Add(model.Username, model.Email);
            _users.Add($"{model.Username}_Password", HashPassword(model.Password));

            return true;
        }

        public bool AuthenticateUser(LoginModel model, out UserModel user)
        {
            user = null;

            if (_users.ContainsKey(model.Username) && BCrypt.Net.BCrypt.Verify(model.Password, _users[$"{model.Username}_Password"]))
            {
                user = new UserModel { Username = model.Username, Email = _users[model.Username] };
                return true;
            }

            return false;
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, 12);
        }
    }
}