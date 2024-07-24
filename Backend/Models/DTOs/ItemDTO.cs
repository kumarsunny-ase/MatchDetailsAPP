using System;
using MatchDetailsApp.Models.Domain;

namespace MatchDetailsApp.Models.DTOs
{
	public class ItemDTO
	{
        public int Id { get; set; }
        public ICollection<Value> Values { get; set; }
    }
}

