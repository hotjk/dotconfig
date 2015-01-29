using Settings.Client;
using System;
using System.Collections.Generic;

namespace Settings.Model
{
    public interface IUserService
    {
        User GetUser(int userId);
        User GetUser(string username);
        bool ValidatePassword(string username, string password);
        bool SaveUser(User user);
        bool DeleteUser(User user);
    }
}
