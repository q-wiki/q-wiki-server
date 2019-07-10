using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WikidataGame.Backend.Helpers;
using WikidataGame.Backend.Models;
using WikidataGame.Backend.Repos;
using WikidataGame.Backend.Services;

namespace WikidataGame.Backend.Tests
{
    public class QueryTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            return Db.Instance.QuestionRepo.GetAll().Select(q => new object[] { q }).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    }
    public sealed class Db
    {
        private IQuestionRepository _questionRepo;
        private IMinigameRepository _minigameRepo;
        private DataContext _context;
        private Db()
        {
            var Builderoptions = new DbContextOptionsBuilder<Helpers.DataContext>();
            Builderoptions.UseLazyLoadingProxies().UseInMemoryDatabase(Guid.NewGuid().ToString());
            var context = new DataContext(Builderoptions.Options);
            context.Database.EnsureCreated();
            _context = context;
        }

        private static readonly Lazy<Db> lazy = new Lazy<Db>(() => new Db());
        public static Db Instance
        {
            get
            {
                return lazy.Value;
            }
        }

        public DataContext Context => _context;

        public IQuestionRepository QuestionRepo
        {
            get
            {
                if (_questionRepo == null)
                    _questionRepo = new QuestionRepository(_context);
                return _questionRepo;
            }
        }

        public IMinigameRepository MinigameRepo
        {
            get
            {
                if (_minigameRepo == null)
                    _minigameRepo = new MinigameRepository(_context);
                return _minigameRepo;
            }
        }

    }

    public class TestMinigameService : MinigameServiceBase
    {
        public TestMinigameService(IMinigameRepository minigameRepo, DataContext dataContext) : base(minigameRepo, dataContext)
        {
        }

        public new List<Tuple<string, string>> QueryWikidata(String sparql)
        {
            return base.QueryWikidata(sparql);
        }
    }
}
