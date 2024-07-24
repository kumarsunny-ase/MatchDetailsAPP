using System;
using MatchDetailsApp.Models.Domain;

namespace MatchDetailsApp.Models.DTOs
{
	public class ItemDto
	{
        public int Id { get; set; }
        public ICollection<Value> Values { get; set; }
    }
}

