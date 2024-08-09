using BCrypt.Net;
using CleanAuth.CoreBusiness.ValueObjects;

namespace CleanAuth.CoreBusiness.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Device { get; set; }
        public string IpAddress { get; set; }
        public decimal Balance { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string Browser { get; set; }

        public User() { }

        public User(string username, string password, string firstname, string lastname, string device,
           string ipAddress)
        {
            Username = new Email(username, "Email");
            PasswordHash = password;
            
            // Validate and assign password
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Password cannot be empty.");
            }
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password); // Hash the password

            Firstname = new Name(firstname, 40, "First Name");
            Lastname = new Name(lastname, 40, "First Name");
            Device = device;
            IpAddress = ipAddress;
            CreatedAt = DateTime.UtcNow;
            Balance = 0;
        }
    }
}