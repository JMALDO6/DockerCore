using DockerCore.Cross.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DockerCore.Data.Service.Abstract
{
    public interface IRouletteService : IService<Roulette>
    {
        Task<bool> OpenRoulette(Roulette roulette);
        Task<bool> BetRoulette(BetRoulette betRoulette);
        Task<bool> ClosedRoulette(Roulette roulette);
        Task<List<BetRoulette>> GetBetsRoulette(Roulette roulette);
    }
}
