using CleanAuth.UseCases.DTO;

namespace CleanAuth.UseCases.Interfaces
{
    public interface IAuthenticateUseCase
    {
        Task<UserLoginResponse> ExecuteAsync(UserLoginRequest userLoginRequest);
    }
}
