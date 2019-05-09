using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace WikidataGame.Backend.Repos
{
    public interface IRepository<TEntity, TIdEntity> where TEntity : class where TIdEntity : class
    {
        TEntity Get(TIdEntity id);
        TEntity GetWith<TProperty>(TIdEntity id, params Expression<Func<TEntity, TProperty>>[] navigationPropertyPaths);
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> GetAllWith<TProperty>(params Expression<Func<TEntity, TProperty>>[] navigationPropertyPaths);
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
        TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate);

        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);

        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);

        void Update(TEntity entity);
        void UpdateRange(IEnumerable<TEntity> entities);
    }
}
