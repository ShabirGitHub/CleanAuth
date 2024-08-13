using Dapper;
using System.Data;
using CleanAuth.UseCases.RepositoryPlugins;
using CleanAuth.CoreBusiness.Entities;
using Microsoft.Extensions.Logging;

namespace CleanAuth.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnection _dbConnection;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(IDbConnection dbConnection, ILogger<UserRepository> logger)
        {
            _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<int> AddUserAsync(User user)
        {
            var userId = await _dbConnection.QuerySingleAsync<int>(
                "usp_AddUser",
                user,
                commandType: CommandType.StoredProcedure
            );

            _logger.LogInformation("User added successfully with ID: {UserId}", userId);
            return userId;
        }

        public async Task<bool> AddUserLoginAsync(UserLogin userLogin)
        {
            var rowsAffected = await _dbConnection.ExecuteAsync(
                "usp_AddUserLogin",
                userLogin,
                commandType: CommandType.StoredProcedure
            );

            var result = rowsAffected > 0;
            _logger.LogInformation("User login {Result} for UserId: {UserId}", result ? "successful" : "failed", userLogin.UserId);
            return result;
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            var rowsAffected = await _dbConnection.ExecuteAsync(
                "usp_UpdateUserBalance",
                new { user.Username, user.Balance },
                commandType: CommandType.StoredProcedure
            );

            var result = rowsAffected > 0;
            _logger.LogInformation("Update {Result} for user: {Username}", result ? "successful" : "failed", user.Username);
            return result;
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            var user = await _dbConnection.QuerySingleOrDefaultAsync<User>(
                "usp_GetUserByUsername",
                new { Username = username },
                commandType: CommandType.StoredProcedure
            );

            if (user != null)
            {
                _logger.LogInformation("User retrieved successfully: {Username}", username);
            }
            else
            {
                _logger.LogWarning("User not found: {Username}", username);
            }
            return user;
        }

        public async Task<decimal> GetBalanceAsync(string username)
        {
            var balance = await _dbConnection.ExecuteScalarAsync<decimal>(
                "usp_GetUserBalance",
                new { Username = username },
                commandType: CommandType.StoredProcedure
            );

            _logger.LogInformation("Balance retrieved for user: {Username}", username);
            return balance;
        }

        public async Task<bool> DoesUserExistAsync(string username)
        {
            var count = await _dbConnection.ExecuteScalarAsync<int>(
                "usp_DoesUserExist",
                new { Username = username },
                commandType: CommandType.StoredProcedure
            );

            var exists = count > 0;
            _logger.LogInformation("User {Exists} for username: {Username}", exists ? "exists" : "does not exist", username);
            return exists;
        }
    }
}
