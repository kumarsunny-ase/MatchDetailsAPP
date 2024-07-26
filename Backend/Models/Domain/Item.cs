using System;
namespace MatchDetailsApp.Models.Domain
{
	public class Item
	{
        public int Id { get; set; } // Primary key for Item

        public ICollection<Value> Values { get; set; }
    }
}

