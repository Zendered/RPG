
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RPG.Data;
using RPG.Dtos.Character;
using RPG.Services.Interfaces;

namespace RPG.Services.CharactersService
{
    public class CharacterService : ICharacterService
    {
        private readonly IMapper mapper;
        private readonly CharactersContext context;
        public CharacterService(IMapper mapper, CharactersContext context)
        {
            this.mapper = mapper;
            this.context = context;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            var userChar = mapper.Map<Character>(newCharacter);
            await context.AddAsync(userChar);
            var characters = await context.Characters.ToListAsync();
            serviceResponse.Data = characters.Select(c => mapper.Map<GetCharacterDto>(c)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            var characters = await context.Characters.ToListAsync();
            serviceResponse.Data = characters.Select(c => mapper.Map<GetCharacterDto>(c)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            var character = await context.Characters.FindAsync(id);
            serviceResponse.Data = mapper.Map<GetCharacterDto>(character);
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            try
            {
                var userChar = await context.Characters.FindAsync(updatedCharacter.Id);

                userChar.Strength = updatedCharacter.Strength;
                userChar.Defense = updatedCharacter.Defense;
                userChar.Name = updatedCharacter.Name;
                userChar.Class = updatedCharacter.Class;
                userChar.HitPoints = updatedCharacter.HitPoints;
                userChar.Intelligence = updatedCharacter.Intelligence;

                mapper.Map(updatedCharacter, userChar);
                serviceResponse.Data = mapper.Map<GetCharacterDto>(userChar);

                await context.SaveChangesAsync();

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
                var userChar = await context.Characters.FindAsync(id);
                context.Characters.Remove(userChar);
                await context.SaveChangesAsync();
                serviceResponse.Data = context.Characters.Select(c => mapper.Map<GetCharacterDto>(c)).ToList();

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
