using System.Collections.Generic;
using System.Threading.Tasks;

namespace DockerCore.Data.Service.Abstract
{
    public interface IService<T> where T : class
    {
        Task<List<T>> Search(int page, int pageSize);
        Task<T> Add();
    }
}
