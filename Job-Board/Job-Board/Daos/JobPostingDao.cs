﻿using Dapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data;
using System.Linq;
using Job_Board.Models;
using Job_Board.Wrappers;
using Microsoft.CodeAnalysis;
using System;

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
            var query = "INSERT INTO JobPosting (Position, LocationId, Department, Description) " +
                "VALUES (@Position, @LocationId, @Department, @Description)";

            //Parameters to be injected in the Query
            var parameters = new DynamicParameters();
            parameters.Add("Position", jobPosting.Position, DbType.String);
            parameters.Add("LocationId", jobPosting.LocationId, DbType.Guid);
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
            var query = "SELECT * FROM JobPosting";
            using (var connection = sqlWrapper.CreateConnection())
            {
                var jobPostings = await connection.QueryAsync<JobPosting>(query);

                return jobPostings.ToList();
            }
        }

        //GET Request
        public async Task<JobPostingRequest> GetJobPostingByID(Guid id)
        {
            //SQL query with passed in integer 
            var query = $"SELECT * FROM JobPosting WHERE Id = '{id}'";

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
        public async Task DeleteJobPostingById(Guid id)
        {
            //SQL Query to delete off of passed in integer
            var query = $"DELETE FROM JobPosting WHERE Id = '{id}'";

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
            var query = $"UPDATE JobPosting SET Position = ISNULL(@Position, Position), LocationId = COALESCE(@LocationId, LocationId), " +
                $"Department = ISNULL(@Department, Department), Description = ISNULL(@Description, Description)" +
                $"WHERE Id = '{jobPosting.Id}'";

            var parameters = new DynamicParameters();
            parameters.Add("Position", jobPosting.Position, DbType.String);
            if (jobPosting.LocationId == Guid.Empty)
            {
                parameters.Add("LocationId", DBNull.Value, DbType.Guid);
            }
            else
            {
                parameters.Add("LocationId", jobPosting.LocationId, DbType.Guid);
            }
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

        public async Task<JobPostingByPosition> GetJobPostingByPosition(string position)
        {
            var query = $"SELECT * FROM JobPosting WHERE Position = '{position}'";

            using (sqlWrapper.CreateConnection())
            {
                var jobPosting = await sqlWrapper.QueryFirstOrDefaultAsync<JobPostingByPosition>(query);
                return jobPosting;
            }
        }

        public async Task<JobPostingByLocationId> GetJobPostingByLocationId(Guid locationId)
        {
            var query = $"SELECT * FROM JobPosting WHERE LocationId = '{locationId}'";

            using (sqlWrapper.CreateConnection())
            {
                var jobPosting = await sqlWrapper.QueryFirstOrDefaultAsync<JobPostingByLocationId>(query);
                return jobPosting;
            }
        }






    }
}