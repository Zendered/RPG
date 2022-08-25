using Microsoft.EntityFrameworkCore;

namespace RPG.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly CharactersContext context;

        public AuthRepository(CharactersContext context)
        {
            this.context = context;
        }

        public Task<ServiceResponse<string>> Login(string username, string password)
        {
            throw new NotImplementedException();
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
    }
}
