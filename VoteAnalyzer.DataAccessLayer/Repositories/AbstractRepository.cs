using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using VoteAnalyzer.DataAccessLayer.DbContexts;
using VoteAnalyzer.DataAccessLayer.Entities;

namespace VoteAnalyzer.DataAccessLayer.Repositories
{
    public abstract class AbstractRepository<TModel> : IRepository<TModel, Guid>
        where TModel : class, IIdentifiable<Guid>
    {
        public async Task CreateAsync(TModel model)
        {
            using (var context = new MainDbContext())
            {
                context.Set<TModel>().Add(model);
                await context.SaveChangesAsync();
            }
        }

        public async Task<TModel> ReadAsync(Guid id)
        {
            using (var context = new MainDbContext())
            {
                return await GetQuery(context).FirstOrDefaultAsync(m => m.Id == id);
            }
        }

        public async Task UpdateAsync(TModel model)
        {
            using (var context = new MainDbContext())
            {
                context.Set<TModel>().Attach(model);
                context.Entry(model).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            using (var context = new MainDbContext())
            {
                var entity = await GetQuery(context).FirstOrDefaultAsync(m => m.Id == id);

                if (entity == null)
                {
                    return;
                }

                context.Set<TModel>().Remove(entity);
                await context.SaveChangesAsync();
            }
        }

        protected virtual IQueryable<TModel> GetQuery(MainDbContext context)
        {
            return context.Set<TModel>();
        }
    }
}
