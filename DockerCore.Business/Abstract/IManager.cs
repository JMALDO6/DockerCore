using DockerCore.Cross.Entities;
using System.Threading.Tasks;

namespace DockerCore.Business.Abstract
{
    public interface IManager<T> where T : class
    {
        Task<T> Add();
        Task<bool> Open(Roulette roulette);
        Task<ResultBet> Bet(BetRoulette betRoulette);
    }
}
