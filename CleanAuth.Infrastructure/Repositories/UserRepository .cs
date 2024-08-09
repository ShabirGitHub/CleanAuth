using Dapper;
using System.Data;
using CleanAuth.UseCases.RepositoryPlugins;
using CleanAuth.CoreBusiness.Entities;

namespace CleanAuth.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnection _dbConnection;

        public UserRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<int> AddUserAsync(User user)
        {
            var sql = @"INSERT INTO Users (Username, PasswordHash, FirstName, LastName, Device, IpAddress, CreatedAt)
                    VALUES (@Username, @PasswordHash, @FirstName, @LastName, @Device, @IpAddress, @CreatedAt);
                    SELECT CAST(SCOPE_IDENTITY() as int)";
            var userId = await _dbConnection.QuerySingleAsync<int>(sql, user);
            return userId;
        }

        public async Task<bool> AddUserLoginAsync(UserLogin userLogin)
        {
            var sql = @"INSERT INTO UserLogin (UserId, IpAddress, Device, Browser, LoginTime)
                VALUES (@UserId, @IpAddress, @Device, @Browser, @LoginTime)";
            var rowsAffected = await _dbConnection.ExecuteAsync(sql, userLogin);
            return rowsAffected > 0;
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            var sql = "UPDATE Users SET Balance = @Balance WHERE Username = @Username;";
            var rowsAffected = await _dbConnection.ExecuteAsync(sql, user);
            return rowsAffected > 0;
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            var sql = "SELECT * FROM Users WHERE Username = @Username";
            var user = await _dbConnection.QuerySingleOrDefaultAsync<User>(sql, new { Username = username });
            return user;
        }

        public async Task<decimal> GetBalanceAsync(string userName)
        {
            string sql = "SELECT Balance FROM Users WHERE Username = @Username";
            return await _dbConnection.ExecuteScalarAsync<decimal>(sql, new { Username = userName });
        }

        public async Task<bool> DoesUserExistAsync(string username)
        {
            var sql = "SELECT COUNT(1) FROM Users WHERE Username = @Username";
            var count = await _dbConnection.ExecuteScalarAsync<int>(sql, new { Username = username });
            return count > 0;
        }
    }
}
