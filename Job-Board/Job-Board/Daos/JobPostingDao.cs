using Dapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data;
using System.Linq;
using Job_Board.Models;
using Job_Board.Wrappers;
using Microsoft.CodeAnalysis;

namespace Job_Board.Daos
{
    public class JobPostingDao : IJobPostingDao
    {
        private readonly ISqlWrapper sqlWrapper;

        public JobPostingDao(ISqlWrapper sqlWrapper)
        {
            this.sqlWrapper = sqlWrapper;
        }


        //POST Request (Create)
        public async Task CreateJobPosting(JobPostingRequest jobPosting)
        {
            //SQL Query w/ dynamic params to be passed in
            var query = "INSERT INTO Job_Posting (Position, LocationsId, Department, Description) " +
                "VALUES (@Position, @LocationsId, @Department, @Description)";

            //Parameters to be injected in the Query
            var parameters = new DynamicParameters();
            parameters.Add("Position", jobPosting.Position, DbType.String);
            parameters.Add("LocationsId", jobPosting.LocationsId, DbType.Int32);
            parameters.Add("Department", jobPosting.Department, DbType.String);
            parameters.Add("Description", jobPosting.Description, DbType.String);


            //Connecting to DB
            using (var connection = sqlWrapper.CreateConnection())
            {
                //executing query
                await connection.ExecuteAsync(query, parameters);
            }
        }

        //GET Request
        public async Task<IEnumerable<JobPosting>> GetJobPostings()
        {
            var query = "SELECT * FROM Job_Posting";
            using (var connection = sqlWrapper.CreateConnection())
            {
                var jobPostings = await connection.QueryAsync<JobPosting>(query);

                return jobPostings.ToList();
            }
        }

        //GET Request
        public async Task<JobPostingRequest> GetJobPostingByID(int id)
        {
            //SQL query with passed in integer 
            var query = $"SELECT * FROM Job_Posting WHERE Id = {id}";

            //Connect to DB
            using (var connection = sqlWrapper.CreateConnection())
            {
                //Run query, set to variable candidate
                var jobPosting = await connection.QueryFirstOrDefaultAsync<JobPostingRequest>(query);

                //Return variable 
                return jobPosting;
            }
        }

        //DELETE Request
        public async Task DeleteJobPostingById(int id)
        {
            //SQL Query to delete off of passed in integer
            var query = $"DELETE FROM Job_Posting WHERE Id = {id}";

            //Connect to DB
            using (var connection = sqlWrapper.CreateConnection())
            {
                //Execute query
                await connection.ExecuteAsync(query);
            }
        }

        //PATCH Request (Update)
        public async Task<JobPosting> UpdateJobPostingById(JobPosting jobPosting)
        {
            //SQL Query, injection with dynamic params & passed in candidate object to access id
            var query = $"UPDATE Job_Posting SET Position = @Position, LocationsId = @LocationsId, " +
                $"Department = @Department, Description = @Description, " +
                $"WHERE Id = {jobPosting.Id}";

            var parameters = new DynamicParameters();
            parameters.Add("Position", jobPosting.Position, DbType.String);
            parameters.Add("LocationsId", jobPosting.LocationsId, DbType.Int32);
            parameters.Add("Department", jobPosting.Department, DbType.String);
            parameters.Add("Description", jobPosting.Description, DbType.String);

            //Connect to DB
            using (var connection = sqlWrapper.CreateConnection())
            {
                //set updated candidate to query result
                var updatedJobPosting = await connection.QueryFirstOrDefaultAsync<JobPosting>(query, parameters);

                return updatedJobPosting;
            }

        }

        public async Task<JobPosting> GetJobPostingByPosition(string position)
        {
            var query = $"SELECT * FROM Job_Posting WHERE Position = {position}";

            using (sqlWrapper.CreateConnection())
            {
                var jobPosting = await sqlWrapper.QueryFirstOrDefaultAsync<JobPosting>(query);
                return jobPosting;
            }
        }

        public async Task<JobPostingRequest> GetJobPostingByLocationsId(int locationsId)
        {
            var query = $"SELECT * FROM Job_Posting WHERE LocationsId = {locationsId}";

            using (sqlWrapper.CreateConnection())
            {
                var jobPosting = await sqlWrapper.QueryFirstOrDefaultAsync<JobPostingRequest>(query);
                return jobPosting;
            }
        }

    }
}