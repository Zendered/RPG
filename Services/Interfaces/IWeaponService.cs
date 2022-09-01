using RPG.Dtos.Character;
using RPG.Dtos.Weapon;

namespace RPG.Services.Interfaces
{
    public interface IWeaponService
    {
        Task<ServiceResponse<GetCharacterDto>> AddWeapon(AddWeaponDto newWeapon);
        //Task<ServiceResponse<List<GetCharacterDto>>> GetAllWeapons();
        //Task<ServiceResponse<GetCharacterDto>> GetWeaponById(int id);
        //Task<ServiceResponse<GetCharacterDto>> UpdateWeapon(UpdateWeaponDto updatedWeapon);
        //Task<ServiceResponse<List<GetCharacterDto>>> DeleteWeapon(int id);
    }
}
