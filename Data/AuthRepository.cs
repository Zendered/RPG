using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace RPG.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly CharactersContext context;
        private readonly IConfiguration config;

        public AuthRepository(CharactersContext context, IConfiguration config)
        {
            this.context = context;
            this.config = config;
        }

        public async Task<ServiceResponse<string>> Login(string username, string password)
        {
            var res = new ServiceResponse<string>();

            var user = await context.Users.FirstOrDefaultAsync(user => user.UserName == username);

            if (user is null)
            {
                res.Success = false;
                res.Menssage = "Username/Password Incorrect.";
            }
            else if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                res.Success = false;
                res.Menssage = "Username/Password Incorrect.";
            }
            else
            {
                res.Data = $"Bearer {CreateToken(user)}";
            }

            return res;
        }

        public async Task<ServiceResponse<int>> Register(User user, string password)
        {
            var res = new ServiceResponse<int>();

            if (await UserExists(user.UserName))
            {
                res.Success = false;
                res.Menssage = "Username/Password Incorrect.";
                return res;
            }

            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            context.Users.Add(user);
            await context.SaveChangesAsync();
            res.Data = user.Id;
            return res;
        }

        public async Task<bool> UserExists(string username)
        {
            var user = await context.Users.AnyAsync(user => user.UserName.ToLower() == username.ToLower());

            return user is true;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA512(); ;
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt);
            {
                var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computeHash.SequenceEqual(passwordHash);
            }
        }

        private string CreateToken(User user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.UserName),
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8
                .GetBytes(config["AppSettings:Token"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
