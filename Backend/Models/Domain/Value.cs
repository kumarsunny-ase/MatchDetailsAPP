using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MatchDetailsApp.Models.Domain
{
	public class Value
	{
        [Key]
        public string MatchId { get; set; }  // Primary key for Value

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "MatchDay must be a positive number.")]
        public int MatchDay { get; set; }

        [Required(ErrorMessage = "Home team name is required.")]
        [StringLength(100, ErrorMessage = "Home team name can't be longer than 100 characters.")]
        public string HomeTeamName { get; set; }

        [Required(ErrorMessage = "Guest team name is required.")]
        [StringLength(100, ErrorMessage = "Guest team name can't be longer than 100 characters.")]
        public string GuestTeamName { get; set; }

        [Required(ErrorMessage = "Planned kickoff time is required.")]
        public DateTime PlannedKickoffTime { get; set; }

        [Required(ErrorMessage = "Stadium ID is required.")]
        [StringLength(50, ErrorMessage = "Stadium ID can't be longer than 50 characters.")]
        public string StadiumId { get; set; }

        [Required(ErrorMessage = "Stadium name is required.")]
        [StringLength(100, ErrorMessage = "Stadium name can't be longer than 100 characters.")]
        public string StadiumName { get; set; }

        [Required(ErrorMessage = "Competition ID is required.")]
        [StringLength(50, ErrorMessage = "Competition ID can't be longer than 50 characters.")]
        public string CompetitionId { get; set; }

        [Required(ErrorMessage = "Competition name is required.")]
        [StringLength(100, ErrorMessage = "Competition name can't be longer than 100 characters.")]
        public string CompetitionName { get; set; }

        [Required(ErrorMessage = "Competition type is required.")]
        [StringLength(50, ErrorMessage = "Competition type can't be longer than 50 characters.")]
        public string CompetitionType { get; set; }

        [Required(ErrorMessage = "Match type is required.")]
        [StringLength(50, ErrorMessage = "Match type can't be longer than 50 characters.")]
        public string MatchType { get; set; }

        [Required(ErrorMessage = "Season is required.")]
        [StringLength(50, ErrorMessage = "Season can't be longer than 50 characters.")]
        public string Season { get; set; }

        [Required(ErrorMessage = "Match date fixed status is required.")]
        public bool MatchDateFixed { get; set; }

        [Required(ErrorMessage = "Start date is required.")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date is required.")]
        public DateTime EndDate { get; set; }

        // Foreign key to Item
        // Foreign key to Item
        [Required(ErrorMessage = "Item ID is required.")]
        public int ItemId { get; set; }

        [ForeignKey("ItemId")]
        public Item Item { get; set; }
    }
}

