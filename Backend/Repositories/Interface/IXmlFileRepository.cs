using System;
using MatchDetailsApp.Models.Domain;

namespace MatchDetailsApp.Repositories.Interface
{
	public interface IXmlFileRepository
	{
        Task<HashSet<int>> ProcessXmlFileAsync(IFormFile file);

        Task<IEnumerable<Value>> GetAllAsync();

		Task<IEnumerable<Value>> GetByDay(int id);

		Task<Value> AddOrUpdateAsync(Value value);

	}
}

