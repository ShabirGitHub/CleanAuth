using CleanAuth.UseCases.DTO;
using CleanAuth.UseCases.Interfaces;
using CleanAuth.UseCases.RepositoryPlugins;
using CleanAuth.UseCases.ServicesPlugins;

namespace CleanAuth.UseCases
{
    public class BalanceUseCase : IBalanceUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;

        public BalanceUseCase(IUserRepository userRepository, IJwtService jwtService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
        }
        public async Task<UserBalanceResponse> ExecuteAsync(string token)
        {
            var userId = _jwtService.ValidateToken(token);

            if (string.IsNullOrEmpty(userId) || userId == "-1")
            {
                // User is not authorized or does not have access
                throw new UnauthorizedAccessException("Invalid or expired token.");
            }

            var balance = await _userRepository.GetBalanceAsync(userId);
            return new UserBalanceResponse { Balance = balance };
        }
    }
}
