using DockerCore.Cross.Contracts;
using System.ComponentModel.DataAnnotations;

namespace DockerCore.Cross.Entities
{
    public class BetRoulette : IEntity
    {
        public long Id { get; set; }
        [Required]
        public long IdRoulette { get; set; }
        public int? Number { get; set; }
        public string Color { get; set; }
        [Required]
        public float Dollars { get; set; }
    }
}
