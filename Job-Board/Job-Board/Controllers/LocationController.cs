﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Job_Board.Daos;
using Job_Board.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using Job_Board.Responses;
using System.Linq;

namespace Job_Board.Controllers
{
    [ApiController]
    public class LocationController : ControllerBase
    {

        private ILocationDao _locationDao;
        public LocationController(ILocationDao locationDao)
        {
            _locationDao = locationDao;
        }

        /// <summary>Get Location</summary>
        /// <remarks>Retrieve a Locations information by its Location ID.</remarks>
        /// <response code="200">Returns Location information for a specific Location ID</response>
        /// <response code="404">Data invalid</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Route("Location/{id}")]
        public async Task<IActionResult> GetLocationById([FromRoute] Guid id)
        {
            try
            {
                var location = await _locationDao.GetLocationById(id);
                if (location == null)
                {
                    return ErrorResponses.Error404("The ID You Entered");
                }
                return Ok(location);
            }
            catch (Exception)
            {
                return ErrorResponses.Error500();
            }
        }


        /// <summary>Create Location</summary>
        /// <remarks>Create New Location information.</remarks>
        /// <response code="201">Update information on an existing Location</response>
        /// <response code="404">Data invalid</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(500)]
        [Route("Location")]
        public async Task<IActionResult> CreateLocation([FromBody] LocationRequest createRequest)
        {
            try
            {
                await _locationDao.CreateLocation(createRequest);
                return SuccessResponses.CreateSuccessful("Location");
            }
            catch (Exception)
            {
                return ErrorResponses.Error500();
            }
        }


        /// <summary>Delete Location</summary>
        /// <remarks>Remove an existing Location from the system by its unique ID.</remarks>
        /// <response code="200">Delete Location Information information for a specific Location ID</response>
        /// <response code="404">Data invalid</response>
        /// <response code="500">Internal Server Error</response>
        [HttpDelete]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Route("Location/{id}")]
        public async Task<IActionResult> DeleteLocationById([FromRoute] Guid id)
        {
            try
            {
                var location = await _locationDao.GetLocationById(id);
                if (location == null)
                {
                    return ErrorResponses.Error404("The ID You Entered");
                }

                await _locationDao.DeleteLocationById(id);
                return SuccessResponses.DeleteSuccessful("Location");
            }
            catch (Exception)
            {
                return ErrorResponses.Error500();
            }
        }


        /// <summary>Update Location</summary>
        /// <remarks>Modify an existing Location's information by the Location ID.</remarks>
        /// <response code="200">Update Location Information by ID</response>
        /// <response code="404">Data invalid</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPatch]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Route("Location")]
        public async Task<IActionResult> UpdateLocationById([FromBody] Location locationReq)
        {
            try
            {
                var location = await _locationDao.GetLocationById(locationReq.Id);
                if (location == null)
                {
                    return ErrorResponses.Error404("The ID You Entered");
                }

                var updatedLocation = await _locationDao.UpdateLocationById(locationReq);
                return SuccessResponses.UpdateObjectSuccessful(locationReq.Id.ToString());
            }
            catch (Exception)
            {
                return ErrorResponses.Error500();
            }
        }
        /// <summary>Get Address by Building</summary>
        /// <remarks>Retrieves Address based on Building.</remarks>
        /// <response code="200">Returns the Address of selected Building</response>
        /// <response code="404">Data invalid</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Route("Location/Building")]
        public async Task<IActionResult> GetAddressByLocation(string building)
        {
            try
            {
                IEnumerable<Location> address = await _locationDao.GetAddressByBuilding(building);
                if (!address.Any())
                {
                    return ErrorResponses.Error404(building);
                }
                return SuccessResponses.GetObjectSuccessful(address);
            }
            catch (Exception e)
            {
                return ErrorResponses.Error500();
            }
        }
    }
}