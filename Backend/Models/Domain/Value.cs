using System;
using System.ComponentModel.DataAnnotations;

namespace MatchDetailsApp.Models.Domain
{
	public class Value
	{
        [Key]
        public string MatchId { get; set; }  // Primary key for Value

        public int MatchDay { get; set; }
        public string HomeTeamName { get; set; }
        public string GuestTeamName { get; set; }
        public DateTime PlannedKickoffTime { get; set; }
        public string StadiumId { get; set; }
        public string StadiumName { get; set; }
        public string CompetitionId { get; set; }
        public string CompetitionName { get; set; }
        public string CompetitionType { get; set; }
        public string MatchType { get; set; }
        public string Season { get; set; }
        public bool MatchDateFixed { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // Foreign key to Item
        public int ItemId { get; set; }
        public Item Item { get; set; }
    }
}

