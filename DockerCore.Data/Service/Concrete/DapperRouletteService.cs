using Dapper;
using DockerCore.Cross.Entities;
using DockerCore.Data.Helpers;
using DockerCore.Data.Service.Abstract;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DockerCore.Data.Service.Concrete
{
    public class DapperRouletteService : IRouletteService
    {
        public async Task<bool> Add(Roulette model)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(SqlHelper.ConnectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }

                    await connection.QueryAsync<Roulette>("sp_RouletteSave",
                                SetParameters(model),
                                commandType: CommandType.StoredProcedure);

                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        Task<bool> IService<Roulette>.AddList(List<Roulette> modelList)
        {
            throw new NotImplementedException();
        }

        Task<bool> IService<Roulette>.Delete(int id)
        {
            throw new NotImplementedException();
        }

        Task<Roulette> IService<Roulette>.GetById(int id)
        {
            throw new NotImplementedException();
        }

        Task<List<Roulette>> IService<Roulette>.Search(int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        Task<bool> IService<Roulette>.Update(int id, Roulette model)
        {
            throw new NotImplementedException();
        }

        private DynamicParameters SetParameters(Roulette roulette)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@DateCreation", DateTime.Now);

            return parameters;

        }
    }
}
