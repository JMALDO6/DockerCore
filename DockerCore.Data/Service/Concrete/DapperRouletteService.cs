using Dapper;
using DockerCore.Cross.Entities;
using DockerCore.Data.Helpers;
using DockerCore.Data.Service.Abstract;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
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

                var idRoulette = await connection.ExecuteScalarAsync<long>("sp_RouletteSave",
                            commandType: CommandType.StoredProcedure);

                return new Roulette { Id = idRoulette };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }

        public async Task<bool> OpenRoulette(Roulette roulette)
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

        public async Task<bool> BetRoulette(BetRoulette betRoulette)
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

        public async Task<bool> ClosedRoulette(Roulette roulette)
        {
            try
            {
                using IDbConnection connection = new SqlConnection(SqlHelper.ConnectionString);
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                var result = await connection.ExecuteScalarAsync<string>("sp_RouletteClosed",
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

        public async Task<List<BetRoulette>> GetBetsRoulette(Roulette roulette)
        {
            try
            {
                using IDbConnection connection = new SqlConnection(SqlHelper.ConnectionString);
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                var result = await connection.QueryAsync<BetRoulette>("sp_RouletteGetBets",
                            param: SetParameters(roulette),
                            commandType: CommandType.StoredProcedure);


                return result.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }

        public async Task<List<Roulette>> Search(int page, int pageSize)
        {
            page = page <= 0 ? 0 : page - 1;
            int offset = page * pageSize;

            try
            {
                using IDbConnection connection = new SqlConnection(SqlHelper.ConnectionString);
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                var result = await connection.QueryAsync<Roulette>($"SELECT * FROM Roulette ORDER BY IdRoulette OFFSET {offset} ROWS FETCH NEXT {pageSize} ROWS ONLY");

                return result.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
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
