using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Settings.Model
{
    public interface IUserRepository
    {
        User GetUser(int userId);
        User GetUser(string username);
        bool SaveUser(User user);
        bool DeleteUser(User user);
    }
}
