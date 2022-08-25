using Microsoft.AspNetCore.Mvc;
using RPG.Data;
using RPG.Dtos.User;

namespace RPG.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository repo;

        public AuthController(IAuthRepository repo)
        {
            this.repo = repo;
        }

        [HttpPost("register")]
        public async Task<ActionResult<ServiceResponse<int>>> RegisterAsync(UserRegisterDto request)
        {
            var res = await repo.Register(
                new User { UserName = request.Username },
                request.Password
                );
            return res.Success is true ? Ok(res) : BadRequest(res);
        }

        [HttpPost("login")]
        public async Task<ActionResult<ServiceResponse<string>>> LoginAsync(UserLoginDto request)
        {
            var res = await repo.Login(
                request.Username, request.Password
                );
            return res.Success is true ? Ok(res) : BadRequest(res);
        }
    }
}
