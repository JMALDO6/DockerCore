using System.ComponentModel.DataAnnotations;

namespace DockerCore.Cross.Entities
{
    public class BetRoulette
    {
        [Required]
        public long IdRoulette { get; set; }
        public int? Number { get; set; }
        public string Color { get; set; }
        [Required]
        public float Dollars { get; set; }
        public float EarnedMoney { get; set; }
    }
}
