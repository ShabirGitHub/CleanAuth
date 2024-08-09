using CleanAuth.CoreBusiness.Exceptions;
using CleanAuth.UseCases.DTO;
using CleanAuth.UseCases.Interfaces;
using CleanAuth.UseCases.Mappings;
using CleanAuth.UseCases.RepositoryPlugins;

namespace CleanAuth.UseCases
{
    public class SignupUseCase : ISignupUseCase
    {
        private readonly IUserRepository _userRepository;

        public SignupUseCase(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<int> ExecuteAsync(UserSignupRequest requestDto)
        {
            // Use Mapper to map UserSignupRequest to User
            var user = UserMapper.FromDTO(requestDto);

            // Check for existing user
            var DoesUserExist = await _userRepository.DoesUserExistAsync(user.Username);
            if (DoesUserExist)
            {
                throw new ServiceException("A user with this username already exists.");
            }

            // Add the user to the repository
            return await _userRepository.AddUserAsync(user);
        }
    }
}

