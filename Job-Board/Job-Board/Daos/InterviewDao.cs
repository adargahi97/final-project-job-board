﻿using Dapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data;
using System.Linq;
using Job_Board.Models;
using Job_Board.Wrappers;

namespace Job_Board.Daos
{
    public class InterviewDao : IInterviewDao
    {
        private readonly DapperContext _context;
        private readonly ISqlWrapper sqlWrapper;

        public InterviewDao(ISqlWrapper sqlWrapper)
        {
            this.sqlWrapper = sqlWrapper;
        }

        public InterviewDao(DapperContext context)
        {
            _context = context;
        }

        public void GetInterview()
        {
            sqlWrapper.Query<Candidate>("SELECT * FROM [DBO].[JOBBOARD]");

        }

        //POST Request (Create)
        public async Task CreateInterview(InterviewRequest interview)
        {
            //SQL Query w/ dynamic params to be passed in
            var query = "INSERT INTO Interview (Date, Time, LocationsId, CandidateId) " +
                "VALUES (@Date, @Time, @LocationsId, @CandidateId)";

            var parameters = new DynamicParameters();
            parameters.Add("Date", interview.Date, DbType.String);
            parameters.Add("Time", interview.Time, DbType.String);
            parameters.Add("LocationsId", interview.LocationsId, DbType.Int32);
            parameters.Add("CandidateId", interview.CandidateId, DbType.Int32);

            //Connecting to DB
            using (var connection = _context.CreateConnection())
            {
                //executing query
                await connection.ExecuteAsync(query, parameters);
            }
        }

        //GET Request (Read)
        public async Task<IEnumerable<Interview>> GetInterviews()
        {
            var query = "SELECT * FROM Interview";
            using (var connection = _context.CreateConnection())
            {
                var interviews = await connection.QueryAsync<Interview>(query);

                return interviews.ToList();
            }
        }
        //GET Request (Read)
        public async Task<InterviewRequest> GetInterviewByID(int id)
        {
            //SQL query with passed in integer 
            var query = $"SELECT * FROM Interview WHERE Id = {id}";

            //Connect to DB
            using (var connection = _context.CreateConnection())
            {
                //Run query, set to variable candidate
                var candidate = await connection.QueryFirstOrDefaultAsync<InterviewRequest>(query);

                //Return variable 
                return candidate;
            }
        }

        //DELETE Request
        public async Task DeleteInterviewById(int id)
        {
            //SQL Query to delete off of passed in integer
            var query = $"DELETE FROM Interview WHERE Id = {id}";

            //Connect to DB
            using (var connection = _context.CreateConnection())
            {
                //Execute query
                await connection.ExecuteAsync(query);
            }
        }

        //PATCH Request (Update)
        public async Task<Interview> UpdateInterviewById(Interview interview)
        {
            //SQL Query, injection with dynamic params & passed in candidate object to access id
            var query = $"UPDATE Interview SET Date = @Date, Time = @Time, " +
                $"LocationsId = @LocationsId, CandidateId = @CandidateId " +
                $"WHERE Id = {interview.Id}";

            var parameters = new DynamicParameters();
            parameters.Add("Date", interview.Date, DbType.String);
            parameters.Add("Time", interview.Time, DbType.String);
            parameters.Add("LocationsId", interview.LocationsId, DbType.Int32);
            parameters.Add("CandidateId", interview.CandidateId, DbType.Int32);

            //Connect to DB
            using (var connection = _context.CreateConnection())
            {
                //set updated candidate to query result
                var updatedInterview = await connection.QueryFirstOrDefaultAsync<Interview>(query, parameters);

                return updatedInterview;
            }

        }
    }
}
