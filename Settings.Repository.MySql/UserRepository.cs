using Settings.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace Settings.Repository.MySql
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(SqlOption option) : base(option) { }

        public User GetUser(int userId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                return connection.Query<User>(@"SELECT `UserId`, `Username`, `PasswordHash`, `Version`, `CreateAt`, `UpdateAt` FROM `settings_user`
WHERE `UserId` = @UserId AND DELETED = 0;", new { UserId = userId }).SingleOrDefault();
            }
        }

        public User GetUser(string username)
        {
            using (IDbConnection connection = OpenConnection())
            {
                return connection.Query<User>(@"SELECT `UserId`, `Username`, `PasswordHash`, `Version`, `CreateAt`, `UpdateAt` FROM `settings_user`
WHERE `Username` = @Username AND DELETED = 0;", new { Username = username }).SingleOrDefault();
            }
        }

        public bool SaveUser(User user)
        {
            using (IDbConnection connection = OpenConnection())
            {
                using (IDbTransaction transaction = connection.BeginTransaction())
                {
                    User found = connection.Query<User>(
@"SELECT `UserId`, `Username`, `PasswordHash`, `Version`, `CreateAt`, `UpdateAt` FROM `settings_user`
WHERE `UserId` = @UserId AND DELETED = 0 FOR UPDATE;", user).SingleOrDefault();
                    if (found == null)
                    {
                        if (1 != connection.Execute(
@"INSERT INTO `settings_user` 
(`Username`, `PasswordHash`, `Version`, `CreateAt`, `UpdateAt`)
VALUES (@Username, @PasswordHash, @Version, @CreateAt, @UpdateAt);", user))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (found.Version != user.Version)
                        {
                            return false;
                        }
                        user.Version++;
                        int n = connection.Execute(
@"UPDATE `settings_user` 
SET `PasswordHash` = @PasswordHash, `Version` = @Version, `UpdateAt` = @UpdateAt 
WHERE `UserId` = @UserId;", user);
                    }
                    transaction.Commit();
                    return true;
                }
            }
        }

        public bool DeleteUser(User user)
        {
            using (IDbConnection connection = OpenConnection())
            {
                using (IDbTransaction transaction = connection.BeginTransaction())
                {
                    User found = connection.Query<User>(
@"SELECT `UserId`, `Username`, `PasswordHash`, `Version`, `CreateAt`, `UpdateAt` FROM `settings_user`
WHERE `UserId` = @UserId AND DELETED = 0 FOR UPDATE;", user).SingleOrDefault();
                    if (found == null)
                    {
                        return false;
                    }
                    else
                    {
                        if (found.Version != user.Version)
                        {
                            return false;
                        }
                        user.Version++;
                        int n = connection.Execute(
@"UPDATE `settings_user` 
SET `Deleted` = 1, `DeleteAt` = @DeleteAt 
WHERE UserId = @UserId;", user);
                        transaction.Commit();
                        return 1 == n;
                    }
                }
            }
        }
    }
}
