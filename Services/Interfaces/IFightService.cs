using RPG.Dtos.Fight;

namespace RPG.Services.Interfaces
{
    public interface IFightService
    {
        Task<ServiceResponse<AttackerResultDto>> WeaponAttack(WeaponAttackDto attack);
        Task<ServiceResponse<AttackerResultDto>> SkillAttack(SkillAttackDto attack);
    }
}
