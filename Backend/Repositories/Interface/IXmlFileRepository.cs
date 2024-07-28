using System;
using MatchDetailsApp.Models.Domain;

namespace MatchDetailsApp.Repositories.Interface
{
    public interface IXmlFileRepository
    {
        Task<(HashSet<int> MatchDays, HashSet<DateTime> MatchDates)> ProcessXmlFileAsync(IFormFile file);

        Task<IEnumerable<Value>> GetAllAsync(string teamName);

        Task<IEnumerable<Value>> GetByDay(int id);

        Task<IEnumerable<Value>> GetByDate(DateTime date);

        Task<Value> AddOrUpdateAsync(Value value);

    }
}

