using System.Threading.Tasks;

namespace DockerCore.Business.Abstract
{
    public interface IManager<T> where T : class
    {
        Task<bool> Add(T model);
    }
}
