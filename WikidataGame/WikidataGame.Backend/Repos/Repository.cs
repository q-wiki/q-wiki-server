using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WikidataGame.Backend.Helpers;

namespace WikidataGame.Backend.Repos
{
    public class Repository<TEntity, TIdEntity> : IRepository<TEntity, TIdEntity> where TEntity : class
    {
        protected readonly DbContext Context;
        public Repository(DataContext context)
        {
            Context = context;
        }

        /// <summary>
        /// Retrieves the entity with the supplied id/primary key
        /// </summary>
        /// <param name="id">Id/primary key</param>
        /// <returns>The entity</returns>
        public async Task<TEntity> GetAsync(TIdEntity id) => await Context.Set<TEntity>().FindAsync(id);

        /// <summary>
        /// Retrieves all entities
        /// </summary>
        /// <returns>An enumerable of entites</returns>
        public async Task<IEnumerable<TEntity>> GetAllAsync() => await Context.Set<TEntity>().ToListAsync();

        /// <summary>
        /// Retrieves all entities that match the given filter
        /// </summary>
        /// <param name="predicate">filter to apply</param>
        /// <returns>An enumerable of entites</returns>
        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate) => await Context.Set<TEntity>().Where(predicate).ToListAsync();

        /// <summary>
        /// Returns a single entity thats matches the supplied filter or default(Entity) if there is no matching element
        /// </summary>
        /// <param name="predicate">filter to apply</param>
        /// <returns>An entity or default(Entity) if there is no matching element</returns>
        public async Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate) => await Context.Set<TEntity>().SingleOrDefaultAsync(predicate);

        /// <summary>
        /// Adds the given entity
        /// </summary>
        /// <param name="entity">Entity to store</param>
        public async Task AddAsync(TEntity entity) => await Context.Set<TEntity>().AddAsync(entity);

        /// <summary>
        /// Adds a range of entities
        /// </summary>
        /// <param name="entities">Enumerable of entities to store</param>
        public async Task AddRangeAsync(IEnumerable<TEntity> entities) => await Context.Set<TEntity>().AddRangeAsync(entities);

        /// <summary>
        /// Removes the given entity
        /// </summary>
        /// <param name="entity">Entity to delete</param>
        public void Remove(TEntity entity) => Context.Set<TEntity>().Remove(entity);

        /// <summary>
        /// Removes a range of entities
        /// </summary>
        /// <param name="entities">Enumerable of entities to remove</param>
        public void RemoveRange(IEnumerable<TEntity> entities) => Context.Set<TEntity>().RemoveRange(entities);

        /// <summary>
        /// Updates the given entity
        /// </summary>
        /// <param name="entity">Entity to update</param>
        public void Update(TEntity entity) => Context.Set<TEntity>().Update(entity);

        /// <summary>
        /// Updates a range of entities
        /// </summary>
        /// <param name="entities">Enumerable of entities to update</param>
        public void UpdateRange(IEnumerable<TEntity> entities) => Context.Set<TEntity>().UpdateRange(entities);

    }
}
