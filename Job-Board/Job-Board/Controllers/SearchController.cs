﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Job_Board.Daos;
using Job_Board.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;

namespace Job_Board.Controllers
{
    [ApiController]
    public class SearchController : ControllerBase
    {
        private ISearchDao _searchDao;
        public SearchController(ISearchDao searchDao)
        {
            _searchDao = searchDao;
        }

        /// <summary>Search for Location Information by State</summary>
        /// <returns>Location Information</returns>
        /// <response code="200">Returns the Information by State</response>
        [HttpGet]
        [Route("Location/State/{state}")]
        public async Task<IActionResult> GetLocationByState([FromRoute] string state)
        {
            try
            {
                IEnumerable<LocationByState> location = await _searchDao.GetLocationByState(state);
                if (location == null)
                {
                    return StatusCode(404);
                }
                return Ok(location);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }



        /// <summary>Interview Info By Position</summary>
        /// <returns>Interview Information</returns>
        /// <response code="200">Returns the Information by Position</response>
        [HttpGet]
        [Route("JobPosting/Position")]
        public async Task<IActionResult> DailySearchByPosition(string position)
        {
            try
            {
                IEnumerable<JobPostingDailySearchByPosition> candidates = await _searchDao.DailySearchByPosition(position);
                if (candidates == null)
                {
                    return StatusCode(404);
                }
                return Ok(candidates);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
        /// <summary>Pulls Candidate(s) based on Last Name</summary>
        /// <returns>Candidate Information</returns>
        /// <response code="200">Returns the Candidates with matching last names</response>
        [HttpGet]
        [Route("Candidate/LastName/{lastName}")]
        public async Task<IActionResult> GetCandidateByLastName(string lastName)
        {
            try
            {
                IEnumerable<CandidateByLastName> candidates = await _searchDao.GetCandidateByLastName(lastName);
                return Ok(candidates);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>Search for Candidates who are applying for a certain position</summary>
        /// <returns>Candidate Information</returns>
        /// <response code="200">Returns the Candidate Information by Job Id</response>
        [HttpGet]
        [Route("Candidate/JobId/{JobId}")]
        public async Task<IActionResult> GetCandidateByJobId(Guid JobId)
        {
            try
            {
                IEnumerable<CandidateByJobId> candidates = await _searchDao.GetCandidateByJobId(JobId);
                return Ok(candidates);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>Search for Interview Information by Job Id</summary>
        /// <returns>Interview Information</returns>
        /// <response code="200">Returns the Interview Information by Job Id</response>
        [HttpGet]
        [Route("Interview/JobId/{id:guid}")]
        public async Task<IActionResult> GetInterviewByJobId([FromRoute] Guid id)
        {
            try
            {
                var interview = await _searchDao.GetInterviewByJobId(id);
                if (interview == null)
                {
                    return StatusCode(404);
                }
                return Ok(interview);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>Search for Interview Information by Last Name</summary>
        /// <returns>Interview Information</returns>
        /// <response code="200">Returns the Interview Information found by last name</response>
        [HttpGet]
        [Route("Interview/{lastName}")]
        public async Task<IActionResult> GetInterviewByLastName([FromRoute] string lastName)
        {
            try
            {
                IEnumerable<InterviewJoinCandidate> interview = await _searchDao.GetInterviewByLastName(lastName);
                return Ok(interview);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>Search for Interview Information by Date</summary>
        /// <returns>Interview Information</returns>
        /// <response code="200">Returns the Interview Information found by Date</response>
        [HttpGet]
        [Route("Interview/DateTime/{dateTime}")]
        public async Task<IActionResult> GetInterviewsByDateTime([FromRoute] DateTime dateTime)
        {
            try
            {
                IEnumerable<Interview> interview = await _searchDao.GetInterviewsByDate(dateTime);
                return Ok(interview);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

    }
}