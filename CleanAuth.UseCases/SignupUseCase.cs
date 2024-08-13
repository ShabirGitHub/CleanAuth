using CleanAuth.CoreBusiness.Exceptions;
using CleanAuth.UseCases.DTO;
using CleanAuth.UseCases.Interfaces;
using CleanAuth.UseCases.Mappings;
using CleanAuth.UseCases.RepositoryPlugins;
using Microsoft.Extensions.Logging;

namespace CleanAuth.UseCases
{
    public class SignupUseCase : ISignupUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<SignupUseCase> _logger;
        public SignupUseCase(IUserRepository userRepository, ILogger<SignupUseCase> logger)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<int> ExecuteAsync(UserSignupRequest requestDto)
        {
            // Use Mapper to map UserSignupRequest to User
            var user = UserMapper.FromDTO(requestDto);

            // Check for existing user
            var DoesUserExist = await _userRepository.DoesUserExistAsync(user.Username);
            if (DoesUserExist)
            {
                _logger.LogWarning("Signup attempt failed: A user with username {Username} already exists.", user.Username);
                throw new ServiceException("A user with this username already exists.");
            }

            // Add the user to the repository
            return await _userRepository.AddUserAsync(user);
        }
    }
}

