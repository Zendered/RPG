using RPG.Dtos.Fight;

namespace RPG.Services.Interfaces
{
    public interface IFightService
    {
        Task<ServiceResponse<AttackerResultDto>> WeaponAttack(WeaponAttackDto request);
        Task<ServiceResponse<AttackerResultDto>> SkillAttack(SkillAttackDto request);
        Task<ServiceResponse<FightResultDto>> Fight(FightRequestDto request);
        Task<ServiceResponse<List<HighScoreDto>>> GetHighScore();
    }
}
