using Microsoft.AspNetCore.Mvc;
using RPG.Dtos.Character;
using RPG.Services.Interfaces;

namespace RPG.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CharactersController : ControllerBase
    {
        private readonly ICharacterService characterService;

        public CharactersController(ICharacterService characterService)
        {
            this.characterService = characterService;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> GetAllCharacters()
        {
            return Ok(await characterService.GetAllCharacters());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> GetOneCharacter(int id)
        {
            return Ok(await characterService.GetCharacterById(id));
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {
            return Ok(await characterService.AddCharacter(newCharacter));
        }

        [HttpPut]
        public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
        {
            var res = await characterService.UpdateCharacter(updatedCharacter);

            return res.Data != null ? Ok(res) : NotFound(res);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> DeleteCharacter(int id)
        {
            var res = await characterService.DeleteCharacter(id);

            return res.Data != null ? Ok(res) : NotFound(res);
        }
    }
}
