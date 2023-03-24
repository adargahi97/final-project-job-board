﻿using Dapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data;
using System.Linq;
using Job_Board.Models;
using Job_Board.Wrappers;
using System;

namespace Job_Board.Daos
{
    public class LocationDao : ILocationDao
    {

        private readonly ISqlWrapper sqlWrapper;

        public LocationDao(ISqlWrapper sqlWrapper)
        {
            this.sqlWrapper = sqlWrapper;
        }

        //GET Request
        public async Task<IEnumerable<Location>> GetLocations()
        {
            var query = "SELECT * FROM Locations";
            using (var connection = sqlWrapper.CreateConnection())
            {
                var employees = await connection.QueryAsync<Location>(query);

                return employees.ToList();
            }
        }

        //POST Request (Create)
        public async Task CreateLocation(LocationRequest location)
        {
            //SQL Query w/ dynamic params to be passed in
            var query = "INSERT INTO Locations (StreetAddress, City, State, Zip, Building) " +
                "VALUES (@StreetAddress, @City, @State, @Zip, @Building)";

            //Parameters to be injected in the Query
            var parameters = new DynamicParameters();
            parameters.Add("StreetAddress", location.StreetAddress, DbType.String);
            parameters.Add("City", location.City, DbType.Int32);
            parameters.Add("State", location.State, DbType.String);
            parameters.Add("Zip", location.Zip, DbType.String);
            parameters.Add("Building", location.Building, DbType.String);

            //Connecting to DB
            using (var connection = sqlWrapper.CreateConnection())
            {
                //executing query
                await connection.ExecuteAsync(query, parameters);
            }
        }

        //GET Request
        public async Task<LocationRequest> GetLocationByID(Guid id)
        {
            //SQL query with passed in integer 
            var query = $"SELECT * FROM Locations WHERE Id = {id}";

            //Connect to DB
            using (var connection = sqlWrapper.CreateConnection())
            {
                //Run query, set to variable candidate
                var locations = await connection.QueryFirstOrDefaultAsync<LocationRequest>(query);

                //Return variable 
                return locations;
            }
        }

        //DELETE Request
        public async Task DeleteLocationById(Guid id)
        {
            //SQL Query to delete off of passed in integer
            var query = $"DELETE FROM Locations WHERE Id = {id}";

            //Connect to DB
            using (var connection = sqlWrapper.CreateConnection())
            {
                //Execute query
                await connection.ExecuteAsync(query);
            }
        }

        //PATCH Request (Update)
        public async Task<Location> UpdateLocationById(Location location)
        {
            //SQL Query, injection with dynamic params & passed in candidate object to access id
            var query = $"UPDATE Locations SET StreetAddress = @StreetAddress, City = @City, " +
                $"State = @State, Zip = @Zip, Building = @Building" +
                $"WHERE Id = {location.Id}";

            //Parameters to be injected in the Query
            var parameters = new DynamicParameters();
            parameters.Add("StreetAddress", location.StreetAddress, DbType.String);
            parameters.Add("City", location.City, DbType.Int32);
            parameters.Add("State", location.State, DbType.String);
            parameters.Add("Zip", location.Zip, DbType.String);
            parameters.Add("Building", location.Building, DbType.String);

            //Connect to DB
            using (var connection = sqlWrapper.CreateConnection())
            {
                //set updated candidate to query result
                var locationToUpdate = await connection.QueryFirstOrDefaultAsync<Location>(query, parameters);

                return locationToUpdate;
            }

        }
    }
}