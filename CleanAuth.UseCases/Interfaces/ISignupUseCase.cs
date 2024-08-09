using CleanAuth.UseCases.DTO;

namespace CleanAuth.UseCases.Interfaces
{
    public interface ISignupUseCase
    {
        Task<int> ExecuteAsync(UserSignupRequest request);
    }
}
