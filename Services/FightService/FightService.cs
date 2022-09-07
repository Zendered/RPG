using Microsoft.EntityFrameworkCore;
using RPG.Data;
using RPG.Dtos.Fight;
using RPG.Services.Interfaces;

namespace RPG.Services.FightService
{
    public class FightService : IFightService
    {
        private readonly CharactersContext context;

        public FightService(CharactersContext context)
        {
            this.context = context;
        }

        public async Task<ServiceResponse<AttackerResultDto>> SkillAttack(SkillAttackDto request)
        {
            var serviceResponse = new ServiceResponse<AttackerResultDto>();
            try
            {
                var attacker = await context.Characters
                    .Include(c => c.Skills)
                    .FirstOrDefaultAsync(c => c.Id == request.AttackerId);

                var opponent = await context.Characters
                    .FirstOrDefaultAsync(c => c.Id == request.OpponentId);

                var skill = attacker.Skills.FirstOrDefault(s => s.Id == request.SkillId);

                if (skill is null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Menssage = $"{attacker.Name} doesn't know that skill.";
                    return serviceResponse;
                }

                var damage = skill.Damage + new Random().Next(attacker.Intelligence);
                damage -= new Random().Next(opponent.Defense);

                if (damage > 0)
                {
                    opponent.HitPoints -= damage;
                }

                if (opponent.HitPoints <= 0)
                {
                    opponent.HitPoints = 0;
                    serviceResponse.Menssage = $"{opponent.Name} has been defeated!";
                }

                await context.SaveChangesAsync();

                serviceResponse.Data = new AttackerResultDto
                {
                    Attacker = attacker.Name,
                    Opponent = opponent.Name,
                    AttackerHP = attacker.HitPoints,
                    OpponentHP = opponent.HitPoints,
                    Damage = damage
                };

            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Menssage = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<AttackerResultDto>> WeaponAttack(WeaponAttackDto request)
        {
            var serviceResponse = new ServiceResponse<AttackerResultDto>();
            try
            {
                var attacker = await context.Characters
                    .Include(c => c.Weapon)
                    .FirstOrDefaultAsync(c => c.Id == request.AttackerId);

                var opponent = await context.Characters
                    .FirstOrDefaultAsync(c => c.Id == request.OpponentId);

                var damage = attacker.Weapon.Damage + new Random().Next(attacker.Strength);
                damage -= new Random().Next(opponent.Defense);

                if (damage > 0)
                {
                    opponent.HitPoints -= damage;
                }

                if (opponent.HitPoints <= 0)
                {
                    opponent.HitPoints = 0;
                    serviceResponse.Menssage = $"{opponent.Name} has been defeated!";
                }

                await context.SaveChangesAsync();

                serviceResponse.Data = new AttackerResultDto
                {
                    Attacker = attacker.Name,
                    Opponent = opponent.Name,
                    AttackerHP = attacker.HitPoints,
                    OpponentHP = opponent.HitPoints,
                    Damage = damage
                };

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
