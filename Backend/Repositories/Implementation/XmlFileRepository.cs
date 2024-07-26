using System;
using System.Xml;
using MatchDetailsApp.Data;
using MatchDetailsApp.Models.Domain;
using MatchDetailsApp.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace MatchDetailsApp.Repositories.Implementation
{
    /// <summary>
    /// Repository for handling XML file processing and database operations related to match details.
    /// </summary>
    public class XmlFileRepository : IXmlFileRepository
	{
        private readonly MatchDetailsDbContext _matchDetailsDbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlFileRepository"/> class.
        /// </summary>
        /// <param name="matchDetailsDbContext">The database context used for accessing match details.</param>
        public XmlFileRepository(MatchDetailsDbContext matchDetailsDbContext)
		{
            _matchDetailsDbContext = matchDetailsDbContext;
        }

        /// <summary>
        /// Processes an XML file and updates or inserts match details into the database.
        /// </summary>
        /// <param name="file">The XML file containing match details.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a set of match days extracted from the XML file.</returns>
        public async Task<HashSet<int>> ProcessXmlFileAsync(IFormFile file)
        {
            var matchDays = new HashSet<int>();

            using (var stream = new StreamReader(file.OpenReadStream()))
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(stream);

                var nodeList = xmlDoc.DocumentElement.SelectNodes("/PutDataRequest/Fixtures/Fixture");
                foreach (XmlNode node in nodeList)
                {
                    var matchId = node.Attributes["MatchId"].Value;
                    var matchDay = int.Parse(node.Attributes["MatchDay"].Value);
                    matchDays.Add(matchDay);
                    var homeTeamName = node.Attributes["HomeTeamName"].Value;
                    var guestTeamName = node.Attributes["GuestTeamName"].Value;
                    var plannedKickoffTime = node.Attributes["PlannedKickoffTime"].Value;
                    var stadiumName = node.Attributes["StadiumName"].Value;

                    var newValue = new Value
                    {
                        MatchId = matchId,
                        MatchDay = matchDay,
                        HomeTeamName = homeTeamName,
                        GuestTeamName = guestTeamName,
                        PlannedKickoffTime = plannedKickoffTime,
                        StadiumName = stadiumName
                    };

                    // Add or update the value in the database
                    await AddOrUpdateAsync(newValue);
                }
            }

            return matchDays;
        }

        /// <summary>
        /// Retrieves all match details from the database.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of all match details.</returns>
        public async Task<IEnumerable<Value>> GetAllAsync()
        {
            return await _matchDetailsDbContext.Values.ToListAsync();
        }

        /// <summary>
        /// Retrieves match details for a specific match day.
        /// </summary>
        /// <param name="id">The match day to filter by.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of match details for the specified match day.</returns>
        public async Task<IEnumerable<Value>> GetByDay(int id)
        {
            return await _matchDetailsDbContext.Values.Where(x => x.MatchDay == id)
                .Include(x => x.Item).OrderByDescending(x => x.PlannedKickoffTime)
                .ToListAsync();
        }

        /// <summary>
        /// Adds a new match detail or updates an existing one in the database.
        /// </summary>
        /// <param name="value">The match detail to add or update.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the added or updated match detail.</returns>
        public async Task<Value> AddOrUpdateAsync(Value value)
        {
            var existingValue = await _matchDetailsDbContext.Values
            .Include(v => v.Item)
            .FirstOrDefaultAsync(v => v.MatchId == value.MatchId);

            if (existingValue != null)
            {
                // Update existing record
                existingValue.MatchDay = value.MatchDay;
                existingValue.HomeTeamName = value.HomeTeamName;
                existingValue.GuestTeamName = value.GuestTeamName;
                existingValue.PlannedKickoffTime = value.PlannedKickoffTime;
                existingValue.StadiumName = value.StadiumName;
            }
            else
            {
                // Create a new Item and Value
                var newItem = new Item();

                var newValue = new Value
                {
                    MatchId = value.MatchId,
                    MatchDay = value.MatchDay,
                    HomeTeamName = value.HomeTeamName,
                    GuestTeamName = value.GuestTeamName,
                    PlannedKickoffTime = value.PlannedKickoffTime,
                    StadiumName = value.StadiumName,
                    Item = newItem // Set the new Item
                };

                newItem.Values = new List<Value> { newValue };

                // Add newItem and newValue to the context
                _matchDetailsDbContext.Items.Add(newItem);
                _matchDetailsDbContext.Values.Add(newValue);
            }

            // Save changes to the database
            await _matchDetailsDbContext.SaveChangesAsync();

            return value;
        }
    }
}

