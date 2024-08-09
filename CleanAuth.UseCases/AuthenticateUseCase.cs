using CleanAuth.Infrastructure.UnitOfWork;
using CleanAuth.UseCases.DTO;
using CleanAuth.UseCases.Interfaces;
using CleanAuth.UseCases.Mappings;
using CleanAuth.UseCases.RepositoryPlugins;
using CleanAuth.UseCases.ServicesPlugins;
using System.ComponentModel.DataAnnotations;

namespace CleanAuth.UseCases
{
    public class AuthenticateUseCase : IAuthenticateUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly IUnitOfWorkFactory _uowFactory;

        public AuthenticateUseCase(IUserRepository userRepository, IJwtService jwtService, IUnitOfWorkFactory uowFactory)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _uowFactory = uowFactory ?? throw new ArgumentNullException(nameof(uowFactory));
        }

        public async Task<UserLoginResponse> ExecuteAsync(UserLoginRequest requestDto)
        {
            try
            {
                // Validate request
                if (string.IsNullOrWhiteSpace(requestDto.Username) || string.IsNullOrWhiteSpace(requestDto.Password))
                {
                    throw new ArgumentException("Username and password must be provided.");
                }

                // Retrieve user
                var user = await _userRepository.GetUserByUsernameAsync(requestDto.Username);

                if (user == null || !BCrypt.Net.BCrypt.Verify(requestDto.Password, user.PasswordHash))
                {
                    throw new UnauthorizedAccessException("Invalid credentials.");
                }

                // Handle first-time login gift
                if (user.Balance == 0)
                {
                    user.Balance += 5.0m; // Gift balance
                }

                // Prepare user login data
                requestDto.Id = user.Id;
                var userLogin = UserLoginMapper.FromDTO(requestDto);

                bool loginAdded, userUpdated;
                using (IUnitOfWork uow = _uowFactory.Create())
                {
                    // Add user login
                    loginAdded = await _userRepository.AddUserLoginAsync(userLogin);
                    // Update user
                    userUpdated = await _userRepository.UpdateUserAsync(user);

                    uow.Commit();
                }

                if (loginAdded && userUpdated)
                {
                    // Generate JWT token
                    var token = _jwtService.GenerateToken(user);

                    return new UserLoginResponse
                    {
                        FirstName = user.Firstname,
                        LastName = user.Lastname,
                        Token = token,
                    };
                }

                throw new ApplicationException("Failed to log in. Please try again.");
            }
            catch (ArgumentException ex)
            {
                throw new ValidationException("Request validation failed.", ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new UnauthorizedAccessException("Invalid credentials.", ex);
            }
            catch (Exception ex)
            {
                // Handle unexpected exceptions
                throw new ApplicationException("An unexpected error occurred during login.", ex);
            }
        }
    }
}
