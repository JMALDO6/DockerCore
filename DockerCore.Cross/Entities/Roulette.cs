using DockerCore.Cross.Contracts;
using System;

namespace DockerCore.Cross.Entities
{
    public class Roulette : IEntity
    {
        public long Id { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
