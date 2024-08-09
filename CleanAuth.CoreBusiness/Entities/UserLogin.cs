namespace CleanAuth.CoreBusiness.Entities
{
    public class UserLogin
    {
        public int UserId { get; set; }
        public DateTime LoginTime { get; set; }
        public string IpAddress { get; set; }
        public string Device { get; set; }
        public string Browser { get; set; }

        public UserLogin(int userId, string ipAddress, string device, string browser)
        {
            // Validate UserId (must not be 0)
            if (userId <= 0)
            {
                throw new ArgumentException("UserId must be greater than zero.", nameof(userId));
            }

            // Initialize properties
            UserId = userId;
            LoginTime = DateTime.UtcNow;
            IpAddress = ipAddress;
            Device = device;
            Browser = browser;
        }
    }
}
