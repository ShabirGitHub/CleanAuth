using CleanAuth.CoreBusiness.Entities;

namespace CleanAuth.UseCases.RepositoryPlugins
{
    public interface IUserRepository
    {
        Task<int> AddUserAsync(User user);
        Task<bool> AddUserLoginAsync(UserLogin userLogin);
        Task<bool> UpdateUserAsync(User user);
        Task<decimal> GetBalanceAsync(string userName);
        Task<User> GetUserByUsernameAsync(string username);
        Task<bool> DoesUserExistAsync(string userName);
    }
}
