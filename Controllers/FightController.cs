using Microsoft.AspNetCore.Mvc;
using RPG.Dtos.Fight;
using RPG.Services.Interfaces;

namespace RPG.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class FightController : ControllerBase
    {
        private readonly IFightService fightService;

        public FightController(IFightService fightService)
        {
            this.fightService = fightService;
        }

        [HttpPost("Weapon")]
        public async Task<ActionResult<ServiceResponse<AttackerResultDto>>> WeaponAttack(WeaponAttackDto attack)
        {
            return Ok(await fightService.WeaponAttack(attack));
        }

        [HttpPost("Skill")]
        public async Task<ActionResult<ServiceResponse<AttackerResultDto>>> SkillAttack(SkillAttackDto attack)
        {
            return Ok(await fightService.SkillAttack(attack));
        }
    }
}
