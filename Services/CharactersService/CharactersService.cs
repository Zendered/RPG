
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RPG.Data;
using RPG.Dtos.Character;
using RPG.Services.Interfaces;
using System.Security.Claims;

namespace RPG.Services.CharactersService
{
    public class CharacterService : ICharacterService
    {
        private readonly IMapper mapper;
        private readonly CharactersContext context;
        private readonly IHttpContextAccessor httpContext;

        public CharacterService(IMapper mapper, CharactersContext context, IHttpContextAccessor httpContext)
        {
            this.mapper = mapper;
            this.context = context;
            this.httpContext = httpContext;
        }

        private int GetUserId() => int.Parse(httpContext.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            var userChar = mapper.Map<Character>(newCharacter);
            userChar.User = await context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());

            await context.AddAsync(userChar);
            await context.SaveChangesAsync();
            var characters = await context.Characters
                .Where(c => c.User.Id == GetUserId())
                .ToListAsync();
            serviceResponse.Data = characters.Select(c => mapper.Map<GetCharacterDto>(c)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            var characters = await context.Characters
                .Where(c => c.User.Id == GetUserId())
                .ToListAsync();
            serviceResponse.Data = characters.Select(c => mapper.Map<GetCharacterDto>(c)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            var character = await context.Characters
                .FirstOrDefaultAsync(c => c.Id == id && c.User.Id == GetUserId());
            serviceResponse.Data = mapper.Map<GetCharacterDto>(character);
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            try
            {
                var userChar = await context.Characters
                    .Include(c => c.User)
                    .FirstOrDefaultAsync(c => c.Id == updatedCharacter.Id);

                if (userChar.User.Id == GetUserId())
                {
                    userChar.Strength = updatedCharacter.Strength;
                    userChar.Defense = updatedCharacter.Defense;
                    userChar.Name = updatedCharacter.Name;
                    userChar.Class = updatedCharacter.Class;
                    userChar.HitPoints = updatedCharacter.HitPoints;
                    userChar.Intelligence = updatedCharacter.Intelligence;
                    await context.SaveChangesAsync();

                    serviceResponse.Data = mapper.Map<GetCharacterDto>(userChar);
                }
                else
                {
                    serviceResponse.Success = false;
                    serviceResponse.Menssage = "Character not found";
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Menssage = ex.Message;
            }
            return serviceResponse;

        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            try
            {
                var userChar = await context.Characters.FirstOrDefaultAsync(c => c.Id == id && c.User.Id == GetUserId());
                if (userChar is not null)
                {
                    context.Characters.Remove(userChar);
                    await context.SaveChangesAsync();
                    serviceResponse.Data = context.Characters
                            .Where(c => c.User.Id == GetUserId())
                            .Select(c => mapper.Map<GetCharacterDto>(c)).ToList();
                }
                else
                {
                    serviceResponse.Success = false;
                    serviceResponse.Menssage = "Character not found";
                }
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
