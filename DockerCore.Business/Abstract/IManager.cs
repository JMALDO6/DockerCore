using DockerCore.Cross.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DockerCore.Business.Abstract
{
    public interface IManager<T> where T : class
    {
        Task<T> AddRoulette();
        Task<bool> OpenRoulette(T model);
        Task<ResultBet> BetInRoulette(BetRoulette model);
        Task<ResultRoulette> ClosedRoulette(T model);
        Task<List<T>> Search(int page);
    }
}
