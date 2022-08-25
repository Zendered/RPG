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
                res.Data = user.Id.ToString();
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
    }
}
