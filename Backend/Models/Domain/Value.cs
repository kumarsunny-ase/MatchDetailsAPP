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
        public string PlannedKickoffTime { get; set; }
        public string StadiumName { get; set; }

        // Foreign key to Item
        public int ItemId { get; set; }
        public Item Item { get; set; }
    }
}

