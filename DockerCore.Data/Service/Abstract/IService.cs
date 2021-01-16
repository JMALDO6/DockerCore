using System.Collections.Generic;
using System.Threading.Tasks;

namespace DockerCore.Data.Service.Abstract
{
    public interface IService<T> where T : class
    {
        Task<List<T>> Search(int page, int pageSize);
        Task<T> GetById(int id);
        Task<T> Add();
        Task<bool> AddList(List<T> modelList);
        Task<bool> Delete(int id);
        Task<bool> Update(int id, T model);
    }
}
