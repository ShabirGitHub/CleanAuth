using CleanAuth.UseCases.DTO;
using CleanAuth.UseCases.Interfaces;
using CleanAuth.UseCases.RepositoryPlugins;
using CleanAuth.UseCases.ServicesPlugins;
using Microsoft.Extensions.Logging;

namespace CleanAuth.UseCases
{
    public class BalanceUseCase : IBalanceUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly ILogger<BalanceUseCase> _logger;
        public BalanceUseCase(IUserRepository userRepository, IJwtService jwtService, ILogger<BalanceUseCase> logger)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _jwtService = jwtService ?? throw new ArgumentNullException(nameof(jwtService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<UserBalanceResponse> ExecuteAsync(string token)
        {
            var userId = _jwtService.ValidateToken(token);

            if (string.IsNullOrEmpty(userId) || userId == "-1")
            {
                // User is not authorized or does not have access
                _logger.LogWarning("Invalid or expired token received: {Token}", token);
                throw new UnauthorizedAccessException("Invalid or expired token.");
            }

            var balance = await _userRepository.GetBalanceAsync(userId);
            return new UserBalanceResponse { Balance = balance };
        }
    }
}
