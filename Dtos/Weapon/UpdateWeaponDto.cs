namespace RPG.Dtos.Weapon
{
    public class UpdateWeaponDto
    {
        public string Name { get; set; } = string.Empty;
        public string Damage { get; set; } = string.Empty;
        public int CharacterId { get; set; }
    }
}
