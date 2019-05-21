using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using WikidataGame.Backend.Helpers;

namespace WikidataGame.Backend.Repos
{
    public class Repository<TEntity, TIdEntity> : IRepository<TEntity, TIdEntity> where TEntity : class where TIdEntity : class
    {
        protected readonly DbContext Context;
        private string _primaryKeyPropertyName;

        public Repository(DataContext context)
        {
            Context = context;
            _primaryKeyPropertyName = typeof(TEntity).GetProperties().SingleOrDefault(prop => Attribute.IsDefined(prop, typeof(System.ComponentModel.DataAnnotations.KeyAttribute))).Name;
        }

        /// <summary>
        /// Retrieves the entity with the supplied id/primary key
        /// </summary>
        /// <param name="id">Id/primary key</param>
        /// <returns>The entity</returns>
        public TEntity Get(TIdEntity id) => Context.Set<TEntity>().Find(id);

        /// <summary>
        /// Retrieves all entities
        /// </summary>
        /// <returns>An enumerable of entites</returns>
        public IEnumerable<TEntity> GetAll() => Context.Set<TEntity>().ToList();

        /// <summary>
        /// Retrieves all entities that match the given filter
        /// </summary>
        /// <param name="predicate">filter to apply</param>
        /// <returns>An enumerable of entites</returns>
        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate) => Context.Set<TEntity>().Where(predicate).ToList();

        /// <summary>
        /// Returns a single entity thats matches the supplied filter or default(Entity) if there is no matching element
        /// </summary>
        /// <param name="predicate">filter to apply</param>
        /// <returns>An entity or default(Entity) if there is no matching element</returns>
        public TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate) => Context.Set<TEntity>().SingleOrDefault(predicate);

        /// <summary>
        /// Adds the given entity
        /// </summary>
        /// <param name="entity">Entity to store</param>
        public void Add(TEntity entity) => Context.Set<TEntity>().Add(entity);

        /// <summary>
        /// Adds a range of entities
        /// </summary>
        /// <param name="entities">Enumerable of entities to store</param>
        public void AddRange(IEnumerable<TEntity> entities) => Context.Set<TEntity>().AddRange(entities);

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
