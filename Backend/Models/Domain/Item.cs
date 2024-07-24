using System;
namespace MatchDetailsApp.Models.Domain
{
	public class Item
	{
        public int Id { get; set; }

        public ICollection<Value> Values { get; set; }
    }
}

