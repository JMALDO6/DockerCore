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
        public async Task<Roulette> Add()
        {
            try
            {
                using IDbConnection connection = new SqlConnection(SqlHelper.ConnectionString);

                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                var roulette = await connection.ExecuteScalarAsync<Roulette>("sp_RouletteSave",
                            commandType: CommandType.StoredProcedure);

                return roulette;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }

        public async Task<bool> Open(Roulette roulette)
        {
            try
            {
                using IDbConnection connection = new SqlConnection(SqlHelper.ConnectionString);
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                var result =  await connection.ExecuteScalarAsync<string>("sp_RouletteOpen",
                            param: SetParameters(roulette),
                            commandType: CommandType.StoredProcedure);

                if (result.Equals("OK", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return false;
            }
        }

        public async Task<bool> Bet(BetRoulette betRoulette)
        {
            try
            {
                using IDbConnection connection = new SqlConnection(SqlHelper.ConnectionString);
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                var result = await connection.ExecuteScalarAsync<string>("sp_RouletteBet",
                            param: SetParameters(betRoulette),
                            commandType: CommandType.StoredProcedure);

                if (result.Equals("OK", StringComparison.OrdinalIgnoreCase))
                {

                    return true;
                }

                return false;
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
            parameters.Add("@IdRoulette", roulette.Id);

            return parameters;
        }

        private DynamicParameters SetParameters(BetRoulette betRoulette)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@IdRoulette", betRoulette.IdRoulette);
            parameters.Add("@Dollars", betRoulette.Dollars);
            parameters.Add("@Number", betRoulette.Number);
            parameters.Add("@Color", betRoulette.Color);

            return parameters;
        }
    }
}
