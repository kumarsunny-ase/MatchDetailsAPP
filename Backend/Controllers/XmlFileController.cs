using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MatchDetailsApp.Data;
using MatchDetailsApp.Models.Domain;
using System.Xml;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MatchDetailsApp.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using MatchDetailsApp.Repositories.Implementation;
using System.Globalization;

namespace MatchDetailsApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    /// <summary>
    /// Controller for handling XML file uploads and retrieval of match data.
    /// </summary>
    public class XmlFileController : Controller
    {
        private readonly XmlFileRepository _xmlFileRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlFileController"/> class.
        /// </summary>
        /// <param name="xmlFileRepository">The XML file repository.</param>
        public XmlFileController(XmlFileRepository xmlFileRepository)
        {
            _xmlFileRepository = xmlFileRepository;
        }

        /// <summary>
        /// Uploads an XML file and processes it to store match data.
        /// </summary>
        /// <param name="file">The XML file containing match data.</param>
        /// <returns>A response indicating the result of the upload operation.</returns>
        [HttpPost("upload")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> UploadXml(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            try
            {
                var matchData = await _xmlFileRepository.ProcessXmlFileAsync(file);
                return Ok(new { message = "File uploaded and data saved!", matchDays = matchData.MatchDays, matchDates = matchData.MatchDates.ToList() });
            }
            catch (XmlException xmlEx)
            {
                return BadRequest($"Error parsing XML file: {xmlEx.Message}");
            }
            catch (DbUpdateException dbEx)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database update error: {dbEx.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An unexpected error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves the details of matches for a given team name.
        /// </summary>
        /// <param name="teamName">The name of the team for which match details are to be retrieved.</param>
        /// <returns>A list of match details for the specified team wrapped in an ActionResult. Returns a 500 status code if an error occurs.</returns>
        [HttpGet("byTeamName/{teamName}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<IEnumerable<ValueDto>>> GetTeamDetails(string teamName)
         {
            try
            {
                var values = await _xmlFileRepository.GetAllAsync(teamName);
                var response = values.Select(value => new ValueDto
                {
                    MatchDay = value.MatchDay,
                    MatchType = value.MatchType,
                    GuestTeamName = value.GuestTeamName,
                    PlannedKickoffTime = value.PlannedKickoffTime,
                    StadiumId = value.StadiumId,
                    StadiumName = value.StadiumName,
                    Season = value.Season,
                    CompetitionId = value.CompetitionId,
                    CompetitionName = value.CompetitionName,
                    CompetitionType = value.CompetitionType,
                    MatchDateFixed = value.MatchDateFixed,
                    StartDate = value.StartDate,
                    EndDate = value.EndDate,
                }).ToList();

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while fetching values: {ex.Message}");
            }

        }

        /// <summary>
        /// Retrieves match data for a specific match day.
        /// </summary>
        /// <param name="matchDay">The match day to filter by.</param>
        /// <returns>A list of match data for the specified match day.</returns>
        [HttpGet("byMatchDay/{matchDay}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<IEnumerable<ValueDto>>> GetByMatchDay(int matchDay)
        {
            try
            {
                var values = await _xmlFileRepository.GetByDay(matchDay);
                var response = values.Select(v => new ValueDto
                {
                    MatchDay = v.MatchDay,
                    HomeTeamName = v.HomeTeamName,
                    GuestTeamName = v.GuestTeamName,
                    PlannedKickoffTime = v.PlannedKickoffTime,
                    StadiumName = v.StadiumName,
                }).ToList();

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while fetching values: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves match data for a specific match date.
        /// </summary>
        /// <param name="matchDate">The match date to filter by.</param>
        /// <returns>A list of match data for the specified match date.</returns>
        [HttpGet("byMatchDate/{matchDate}")]
        //[Authorize(Roles = "User")]
        public async Task<ActionResult<IEnumerable<MatchDateValueDto>>> GetByMatchDate(DateTime matchDate)
        {
            try
            {
                // Adjust repository call to use the new date-based method
                var values = await _xmlFileRepository.GetByDate(matchDate);

                // Transform entities into DTOs
                var response = values.Select(v => new MatchDateValueDto
                {
                    HomeTeamName = v.HomeTeamName,
                    GuestTeamName = v.GuestTeamName,
                    StadiumName = v.StadiumName,
                }).ToList();

                return Ok(response);
            }
            catch (Exception ex)
            {
                // Handle exceptions and return a server error response
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while fetching values: {ex.Message}");
            }
        }
    }
}

