using System;
using System.Globalization;
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
        public async Task<(HashSet<int> MatchDays, HashSet<DateTime> MatchDates)> ProcessXmlFileAsync(IFormFile file)
        {
            var matchDays = new HashSet<int>();
            var matchDates = new HashSet<DateTime>();

            using (var stream = new StreamReader(file.OpenReadStream()))
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(stream);

                // Select all nodes with the tag "Fixture"
                var nodeList = xmlDoc.DocumentElement.SelectNodes("/PutDataRequest/Fixtures/Fixture");

                // Iterate data through each node in the list
                foreach (XmlNode node in nodeList)
                {
                    // Extract and parse match details from XML attributes
                    var matchId = node.Attributes["MatchId"].Value;
                    var matchDay = int.Parse(node.Attributes["MatchDay"].Value);
                    var MatchType = node.Attributes["MatchType"]?.Value;
                    var homeTeamName = node.Attributes["HomeTeamName"].Value;
                    var guestTeamName = node.Attributes["GuestTeamName"].Value;
                    var plannedKickoffTime = DateTime.Parse(node.Attributes["PlannedKickoffTime"].Value);
                    var stadiumId = node.Attributes["StadiumId"]?.Value;
                    var stadiumName = node.Attributes["StadiumName"].Value;
                    var season = node.Attributes["Season"]?.Value;
                    var competitionId = node.Attributes["CompetitionId"]?.Value;
                    var competitionName = node.Attributes["CompetitionName"]?.Value;
                    var competitionType = node.Attributes["CompetitionType"]?.Value;
                    var matchDateFixed = bool.Parse(node.Attributes["MatchDateFixed"]?.Value ?? "false");
                    var startDate = DateTime.Parse(node.Attributes["StartDate"].Value);
                    var endDate = DateTime.Parse(node.Attributes["EndDate"].Value);


                    // Add extracted values to the respective lists
                    matchDays.Add(matchDay);
                    matchDates.Add(plannedKickoffTime);

                    var newValue = new Value
                    {
                        MatchId = matchId,
                        MatchDay = matchDay,
                        MatchType = MatchType,
                        HomeTeamName = homeTeamName,
                        GuestTeamName = guestTeamName,
                        PlannedKickoffTime = plannedKickoffTime,
                        StadiumId = stadiumId,
                        StadiumName = stadiumName,
                        Season = season,
                        CompetitionId = competitionId,
                        CompetitionName = competitionName,
                        CompetitionType = competitionType,
                        MatchDateFixed = matchDateFixed,
                        StartDate = startDate,
                        EndDate = endDate
                    };

                    // Add or update the value in the database
                    await AddOrUpdateAsync(newValue);
                }
            }

            return (matchDays, matchDates);
        }

        /// <summary>
        /// Retrieves all match details from the database.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of all match details.</returns>
        public async Task<IEnumerable<Value>> GetAllAsync(string teamName)
        {
            return await _matchDetailsDbContext.Values.Where(x => x.HomeTeamName == teamName).ToListAsync();
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

        // <summary>
        /// Retrieves match details for a specific match date.
        /// </summary>
        /// <param name="id">The match date to filter by.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of match details for the specified match date.
        public async Task<IEnumerable<Value>> GetByDate(DateTime matchDate)
        {
            // Parse the date string from the format received from the frontend (assuming MM/DD/YYYY)
            string formattedDate = matchDate.ToString("yyyy-MM-dd");

            // Convert the formatted string to a DateTime object for database comparison
            DateTime databaseDate = DateTime.ParseExact(formattedDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            // Extract the start and end of the day for the given date
            var startOfDay = databaseDate.Date;
            var endOfDay = startOfDay.AddDays(1).AddTicks(-1);

            // Query to get values scheduled for the specified date
            return await _matchDetailsDbContext.Values
                .Where(x => x.PlannedKickoffTime >= startOfDay && x.PlannedKickoffTime <= endOfDay)
                .Include(x => x.Item)
                .OrderByDescending(x => x.PlannedKickoffTime)
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
                existingValue.MatchType = value.MatchType;
                existingValue.HomeTeamName = value.HomeTeamName;
                existingValue.GuestTeamName = value.GuestTeamName;
                existingValue.PlannedKickoffTime = value.PlannedKickoffTime;
                existingValue.StadiumId = value.StadiumId;
                existingValue.StadiumName = value.StadiumName;
                existingValue.Season = value.Season;
                existingValue.CompetitionId = value.CompetitionId;
                existingValue.CompetitionName = value.CompetitionName;
                existingValue.CompetitionType = value.CompetitionType;
                existingValue.MatchDateFixed = value.MatchDateFixed;
                existingValue.StartDate = value.StartDate;
                existingValue.EndDate = value.EndDate;
            }
            else
            {
                // Create a new Item and Value
                var newItem = new Item();

                var newValue = new Value
                {
                    MatchId = value.MatchId,
                    MatchDay = value.MatchDay,
                    MatchType = value.MatchType,
                    HomeTeamName = value.HomeTeamName,
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

