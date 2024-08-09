using CleanAuth.CoreBusiness.Entities;
using CleanAuth.UseCases.DTO;
using System.Net;

namespace CleanAuth.UseCases.Mappings
{
    public class UserLoginMapper
    {
        public static UserLogin FromDTO(UserLoginRequest dto)
        {
            return new UserLogin(dto.Id, dto.IpAddress, dto.Device, dto.Browser);
        }
    }
}
