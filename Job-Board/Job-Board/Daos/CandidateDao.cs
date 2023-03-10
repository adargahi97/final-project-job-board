using Dapper;
using Job_Board.Models;
using Job_Board.Wrappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Job_Board.Daos
{
    public class CandidateDao : ICandidateDao
    {
        //DB Connection Setup
        private readonly ISqlWrapper sqlWrapper;

        public CandidateDao(ISqlWrapper sqlWrapper)
        {
            this.sqlWrapper = sqlWrapper;
        }

        //POST Request (Create)
        public async Task CreateCandidate(CandidateRequest candidate)
        {
            //SQL Query w/ dynamic params to be passed in
            var query = "INSERT INTO Candidate (FirstName, LastName, PhoneNumber, Job_Id, LocationsId) " +
                "VALUES (@FirstName, @LastName, @PhoneNumber, @Job_Id, @LocationsId)";

            //Parameters to be injected in the Query
            var parameters = new DynamicParameters();
            parameters.Add("FirstName", candidate.FirstName, DbType.String);
            parameters.Add("LastName", candidate.LastName, DbType.String);
            parameters.Add("PhoneNumber", candidate.PhoneNumber, DbType.String);
            parameters.Add("Job_Id", candidate.Job_Id, DbType.Int32);
            parameters.Add("LocationsId", candidate.LocationsId, DbType.Int32);

            //Connecting to DB
            using (var connection = sqlWrapper.CreateConnection())
            {
                //executing query
                await connection.ExecuteAsync(query, parameters);
            }
        }

        //GET Request (Read)
        public async Task<IEnumerable<Candidate>> GetCandidates()
        {
            //SQL Query

            var query = "SELECT * FROM Candidate";

            //Connect to DB
            using (var connection = sqlWrapper.CreateConnection())
            {
                //Run query, set to variable candidate
                var candidates = await connection.QueryAsync<Candidate>(query);

                //Send variable candidate to a list and return list
                return candidates.ToList();
            }
        }

        //GET Request (Read)
        public async Task<Candidate> GetCandidateByID(int id)
        {
            //SQL query with passed in integer 
            var query = $"SELECT * FROM Candidate WHERE Id = {id}";

            //Connect to DB
            using (sqlWrapper.CreateConnection())
            {
                //Run query, set to variable candidate
                var candidate = await sqlWrapper.QueryFirstOrDefaultAsync<Candidate>(query);

                //Return variable 
                return candidate;
            }
        }

        //DELETE Request
        public async Task DeleteCandidateById(int id)
        {
            //SQL Query to delete off of passed in integer
            var query = $"DELETE FROM Candidate WHERE Id = {id}";

            //Connect to DB
            using (sqlWrapper.CreateConnection())
            {
                //Execute query
                await sqlWrapper.ExecuteAsync(query);
            }
        }

        //PATCH Request (Update)
        public async Task<Candidate> UpdateCandidateById(Candidate candidate)
        {
            //SQL Query, injection with dynamic params & passed in candidate object to access id
            var query = $"UPDATE Candidate SET FirstName = @FirstName, LastName = @LastName, " +
                $"PhoneNumber = @PhoneNumber, Job_Id = @Job_Id, LocationsId = @LocationsId " +
                $"WHERE Id = {candidate.Id}";

            var parameters = new DynamicParameters();
            parameters.Add("FirstName", candidate.FirstName, DbType.String);
            parameters.Add("LastName", candidate.LastName, DbType.String);
            parameters.Add("PhoneNumber", candidate.PhoneNumber, DbType.String);
            parameters.Add("Job_Id", candidate.Job_Id, DbType.Int32);
            parameters.Add("LocationsId", candidate.LocationsId, DbType.Int32);

            //Connect to DB
            using (var connection = sqlWrapper.CreateConnection())
            {
                //set updated candidate to query result
                var updatedCandidate = await connection.QueryFirstOrDefaultAsync<Candidate>(query, parameters);

                return updatedCandidate;
            }

        }

        public async Task<Candidate> GetCandidateByFirstName(string firstName)
        {
            var query = $"GET FROM Candidate WHERE FirstName = {firstName}";

            using (sqlWrapper.CreateConnection())
            {
                var candidate = await sqlWrapper.QueryFirstOrDefaultAsync<Candidate>(query);
                return candidate;
            }
        }
    }
}