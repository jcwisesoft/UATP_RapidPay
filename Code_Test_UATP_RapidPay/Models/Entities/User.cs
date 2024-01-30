namespace Code_Test_UATP_RapidPay.Models.Entities
{
    public class User
    {
        public int Id { get; set; } // Primary key
        public string Username { get; set; }
        public string PasswordHash { get; set; } // Store a hash, not the plain password
    }
}
