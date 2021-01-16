using System.Collections.Generic;

namespace DockerCore.Cross.Entities
{
    public class ResultRoulette
    {
        public List<BetRoulette> BetRoulettes { get; set; }
        public int ResultNumber { get; set; }
    }
}
