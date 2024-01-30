using Code_Test_UATP_RapidPay.Models.Entities;
using Code_Test_UATP_RapidPay.Models.RequestModels;
using Code_Test_UATP_RapidPay.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Code_Test_UATP_RapidPay.Services
{
    public class UserService: IUserService
    {
        private readonly ApiDbContext _context;
        private readonly IConfiguration _configuration;


        public UserService(ApiDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task CreateUser(UserModel model)
        {
            User user = new User();
            // Hash the password before saving
            user.PasswordHash = HashPassword(model.Password);
            user.Username= model.Username;
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

        }


        public async Task<dynamic> Authenticate(UserModel model)
        {
            // Retrieve the user by username
            User user = _context.Users.SingleOrDefault(p => p.Username == model.Username);
            if (user is null)
            {
                throw new Exception("Username or password is incorrect");
            }

            // Verify the password
            if (!VerifyPassword(model.Password, user.PasswordHash))
            {
                throw new Exception("Username or password is incorrect");
            }

            // Generate JWT token
            var token = GenerateJwtToken(user.Username);
            return new { token= token, userId = user.Id, username = user.Username };

        }

        public async Task<List<User>> GetAll()
        {
            return await _context.Users.ToListAsync();

        }
        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }

        public string GenerateJwtToken(string username)
        {

            var tokenHandler = new JwtSecurityTokenHandler();
            var secretKey = _configuration.GetValue<string>("Jwt:Secret");
            var issuer = _configuration.GetValue<string>("Jwt:Issuer");
            var audience = _configuration.GetValue<string>("Jwt:Audience");
            var currentDateTime = DateTime.Now;

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = currentDateTime.AddHours(Convert.ToDouble(5));

            var token = new JwtSecurityToken(
                issuer,
                audience,
                expires: expires,
                signingCredentials: creds
            );

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(token);
            return encodedJwt;
        }
    }
}
