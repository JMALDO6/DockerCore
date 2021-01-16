using DockerCore.Cross.Contracts;
using System;

namespace DockerCore.Cross.Entities
{
    public class Roulette : IEntity
    {
        public long Id { get; set; }
        public DateTime CreationDate { get; set; }
        public bool SatatusOpen { get; set; }
        public bool StatusClosed { get; set; }
        public DateTime OpenDate { get; set; }
        public DateTime ClosedDate { get; set; }
    }
}
