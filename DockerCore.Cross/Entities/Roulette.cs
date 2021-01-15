using DockerCore.Cross.Contracts;
using System;

namespace DockerCore.Cross.Entities
{
    public class Roulette : IEntity
    {
        public int Id { get; set; }
        public DateTime DateCreation { get; set; }
    }
}
