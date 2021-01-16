using DockerCore.Cross.Entities;
using System.Threading.Tasks;

namespace DockerCore.Data.Service.Abstract
{
    public interface IRouletteService : IService<Roulette>
    {
        Task<bool> Open(Roulette roulette);
        Task<bool> Bet(BetRoulette betRoulette);
    }
}
