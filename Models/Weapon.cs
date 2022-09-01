namespace RPG.Models
{
    public class Weapon
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Damage { get; set; } = string.Empty;
        public Character Character { get; set; }
        public int CharacterId { get; set; }
    }
}
