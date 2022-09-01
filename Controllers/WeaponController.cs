using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RPG.Dtos.Character;
using RPG.Dtos.Weapon;
using RPG.Services.Interfaces;

namespace RPG.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class WeaponController : ControllerBase
    {
        private readonly IWeaponService weaponService;

        public WeaponController(IWeaponService weaponService)
        {
            this.weaponService = weaponService;
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> AddWeapon(AddWeaponDto newWeapon)
        {
            return Ok(await weaponService.AddWeapon(newWeapon));
        }
    }
}
