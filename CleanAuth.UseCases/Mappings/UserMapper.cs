using CleanAuth.CoreBusiness.Entities;
using CleanAuth.UseCases.DTO;
using CleanAuth.UseCases.Mappings;

namespace CleanAuth.UseCases.Mappings
{
    public class UserMapper
    {
        public static User FromDTO(UserSignupRequest dto)
        {
            return new User(dto.Username, dto.Password, dto.FirstName, dto.LastName, dto.Device,
           dto.IpAddress);
        }
    }
}
