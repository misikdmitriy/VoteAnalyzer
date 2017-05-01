using System.Threading.Tasks;

using VoteAnalyzer.DataAccessLayer.Entities;

namespace VoteAnalyzer.DataAccessLayer.Repositories
{
    public interface IRepository<TModel, in TId> where TModel : IIdentifiable<TId>
    {
        Task CreateAsync(TModel model);
        Task<TModel> ReadAsync(TId id);
        Task UpdateAsync(TModel model);
        Task DeleteAsync(TId id);
    }
}
