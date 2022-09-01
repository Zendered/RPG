﻿using AutoMapper;
using RPG.Dtos.Character;
using RPG.Dtos.Skill;
using RPG.Dtos.Weapon;

namespace RPG
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Character, GetCharacterDto>();
            CreateMap<Weapon, GetWeaponDto>();
            CreateMap<Skill, GetSkillDto>();

            CreateMap<AddCharacterDto, Character>();
            CreateMap<AddWeaponDto, Weapon>();
            CreateMap<UpdateCharacterDto, Character>();

        }
    }
}
