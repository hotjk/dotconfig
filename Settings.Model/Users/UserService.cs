using Grit.Utility.Security;
using Settings.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Settings.Model
{
    public class UserService : IUserService
    {
        public UserService(IUserRepository userRepository)
        {
            this.UserRepository = userRepository;
        }
        private IUserRepository UserRepository { get; set; }

        public User GetUser(int userId)
        {
            return UserRepository.GetUser(userId);
        }

        public User GetUser(string username)
        {
            return UserRepository.GetUser(username);
        }

        public bool ValidatePassword(string username, string password)
        {
            User user = UserRepository.GetUser(username);
            return PasswordHash.ValidatePassword(password, user.PasswordHash);
        }

        public bool SaveUser(User user)
        {
            user.PasswordHash = PasswordHash.CreateHash(user.Password);
            return UserRepository.SaveUser(user);
        }

        public bool DeleteUser(User user)
        {
            return UserRepository.DeleteUser(user);
        }
    }
}
