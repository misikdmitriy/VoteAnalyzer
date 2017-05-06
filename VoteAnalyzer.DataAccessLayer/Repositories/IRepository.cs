using System;
using System.Threading.Tasks;

using VoteAnalyzer.DataAccessLayer.Entities;

namespace VoteAnalyzer.DataAccessLayer.Repositories
{
    public interface IRepository<TModel, in TId> where TModel : IIdentifiable<TId>
    {
        // CRUD
        Task CreateAsync(TModel model);
        Task<TModel> ReadAsync(TId id);
        Task<TModel[]> ReadAsync(Func<TModel, bool> predicate);
        Task UpdateAsync(TModel model);
        Task DeleteAsync(TId id);

        Task<TModel[]> GetAll();
    }
}
