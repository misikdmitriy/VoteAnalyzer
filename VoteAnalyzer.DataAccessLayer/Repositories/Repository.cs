using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

using VoteAnalyzer.DataAccessLayer.DbContexts;
using VoteAnalyzer.DataAccessLayer.Entities;

namespace VoteAnalyzer.DataAccessLayer.Repositories
{
    public class Repository<TModel> : IRepository<TModel, Guid>
        where TModel : class, IIdentifiable<Guid>
    {
        public async Task CreateAsync(TModel model)
        {
            using (var context = new MainDbContext())
            {
                model.Id = Guid.NewGuid();
                AttachAll(context, model);

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

        public async Task<TModel[]> ReadAsync(Func<TModel, bool> predicate)
        {
            using (var context = new MainDbContext())
            {
                return await Task.FromResult(GetQuery(context).Where(predicate).ToArray());
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
                var model = await context.Set<TModel>().FirstOrDefaultAsync(m => m.Id == id);

                if (model == null)
                {
                    return;
                }

                context.Entry(model).State = EntityState.Deleted;
                await context.SaveChangesAsync();
            }
        }

        protected virtual IQueryable<TModel> GetQuery(MainDbContext context)
        {
            return context.Set<TModel>();
        }

        protected virtual void AttachAll(MainDbContext context, TModel model)
        {
        }

        public async Task<TModel[]> ReadAllAsync()
        {
            using (var context = new MainDbContext())
            {
                return await context.Set<TModel>().ToArrayAsync();
            }
        }
    }
}
