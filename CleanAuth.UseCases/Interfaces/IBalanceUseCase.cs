using CleanAuth.UseCases.DTO;

namespace CleanAuth.UseCases.Interfaces
{
    public interface IBalanceUseCase
    {
        Task<UserBalanceResponse> ExecuteAsync(string token);
    }
}
