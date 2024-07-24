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

namespace MatchDetailsApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class XmlFileController : Controller
    {
        private readonly MatchDetailsDbContext _matchDetailsDbContext;

        public XmlFileController(MatchDetailsDbContext matchDetailsDbContext)
        {
            _matchDetailsDbContext = matchDetailsDbContext;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadXml(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            try
            {
                using (var stream = new StreamReader(file.OpenReadStream()))
                {
                    var xmlDoc = new XmlDocument();
                    xmlDoc.Load(stream);

                    // Assuming a structure for the XML and database models
                    var nodeList = xmlDoc.DocumentElement.SelectNodes("/PutDataRequest/Fixtures/Fixture");
                    foreach (XmlNode node in nodeList)
                    {
                        var newItem = new Item
                        {

                        };

                        // Create a new Value from the Fixture node
                        var newValue = new Value
                        {
                            MatchId = node.Attributes["MatchId"].Value,
                            MatchDay = int.Parse(node.Attributes["MatchDay"].Value),
                            HomeTeamName = node.Attributes["HomeTeamName"].Value,
                            GuestTeamName = node.Attributes["GuestTeamName"].Value,
                            PlannedKickoffTime = node.Attributes["PlannedKickoffTime"].Value,
                            StadiumName = node.Attributes["StadiumName"].Value,
                            ItemId = newItem.Id  // This will need to be set after the Item is saved
                        };

                        newItem.Values = new List<Value> { newValue };


                        _matchDetailsDbContext.Items.Add(newItem);
                    }

                    await _matchDetailsDbContext.SaveChangesAsync();
                }

                return Ok("File uploaded and data saved.");
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ValueDto>>> GetAll()
        {
            try
            {
                var values = await _matchDetailsDbContext.Values.ToListAsync();

                var response = new List<ValueDto>();
                foreach (var item in values)
                {
                    response.Add(new ValueDto
                    {
                        MatchId = item.MatchId,
                        MatchDay = item.MatchDay,
                        HomeTeamName = item.HomeTeamName,
                        GuestTeamName = item.GuestTeamName,
                        PlannedKickoffTime = item.PlannedKickoffTime,
                        StadiumName = item.StadiumName,
                        ItemId = item.ItemId,
                    });
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while fetching values: {ex.Message}");
            }

        }

        [HttpGet("byMatchDay/{matchDay}")]
        public async Task<ActionResult<IEnumerable<ValueDto>>> GetByMatchDay(int matchDay)
        {
            try
            {
                var values = await _matchDetailsDbContext.Values
                    .Where(v => v.MatchDay == matchDay)
                    .Include(v => v.Item)
                    .OrderByDescending(v => v.PlannedKickoffTime)
                    .ToListAsync();

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
    }
}

