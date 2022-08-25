namespace RPG.Dtos.Character
{
    public class AddCharacterDto
    {
        public string Name { get; set; } = "Character";
        public int HitPoints { get; set; } = 100;
        public int Strength { get; set; } = 10;
        public int Defense { get; set; } = 10;
        public int Intelligence { get; set; } = 5;

        public RpgClassEnum Class { get; set; } = RpgClassEnum.Knight;
    }
}
