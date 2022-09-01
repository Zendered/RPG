using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RPG.Data;
using RPG.Dtos.Character;
using RPG.Dtos.Weapon;
using RPG.Services.Interfaces;
using System.Security.Claims;

namespace RPG.Services.WeaponService
{
    public class WeaponService : IWeaponService
    {
        private readonly CharactersContext context;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContext;

        public WeaponService(CharactersContext context, IMapper mapper, IHttpContextAccessor httpContext)
        {
            this.context = context;
            this.mapper = mapper;
            this.httpContext = httpContext;
        }

        private int GetUserId() => int.Parse(httpContext.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

        public async Task<ServiceResponse<GetCharacterDto>> AddWeapon(AddWeaponDto newWeapon)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            try
            {
                var character = await context.Characters
                    .FirstOrDefaultAsync(c => c.Id == newWeapon.CharacterId && c.User.Id == GetUserId());

                if (character is null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Menssage = "Character not found";
                    return serviceResponse;
                }

                var weapon = new Weapon
                {
                    Name = newWeapon.Name,
                    Damage = newWeapon.Damage,
                    Character = character
                };
                await context.Weapons.AddAsync(weapon);
                await context.SaveChangesAsync();

                serviceResponse.Data = mapper.Map<GetCharacterDto>(character);
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Menssage = ex.Message;
            }
            return serviceResponse;

        }
    }
}
