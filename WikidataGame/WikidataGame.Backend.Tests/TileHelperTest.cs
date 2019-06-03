
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using Xunit;
using WikidataGame.Backend;

namespace WikidataGame.Backend.Tests
{
    public class CategoryRepositoryMock : Repos.IRepository<Models.Category, string>
    {
        private IEnumerable<Models.Category> _categories = Enumerable.Range(0, 10)
            .Select(i => new Models.Category {
              Id = Guid.NewGuid().ToString(),
              Title = $"Category number {i}"
            })
            .ToList();

        public IEnumerable<Models.Category> GetAll()
        {
            // we implement GetAll to return 10 randomly generated categories
            return _categories;
        }

        // The methods below are all dummies
        public Models.Category Get(string id)
        {
            return null;
        }
        public IEnumerable<Models.Category> Find(Expression<Func<Models.Category, bool>> predicate)
        {
            return null;
        }
        public Models.Category SingleOrDefault(Expression<Func<Models.Category, bool>> predicate)
        {
            return null;
        }

        public void Add(Models.Category entity) {}
        public void AddRange(IEnumerable<Models.Category> entities) {}

        public void Remove(Models.Category entity) {}
        public void RemoveRange(IEnumerable<Models.Category> entities) {}

        public void Update(Models.Category entity) {}
        public void UpdateRange(IEnumerable<Models.Category> entities) {}
    }

    public class TileHelperTest
    {
        [Fact]
        public void TileCategoryTest()
        {
            // we should get stable categories for a tile
            var tile = new Models.Tile { Id = Guid.NewGuid().ToString() };
            var categoryRepo = new CategoryRepositoryMock();

            var categoriesForFirstDraw = Helpers.TileHelper.GetCategoriesForTile(categoryRepo, tile);
            var categoriesForSecondDraw = Helpers.TileHelper.GetCategoriesForTile(categoryRepo, tile);

            Assert.Equal(categoriesForFirstDraw, categoriesForSecondDraw);
        }

        [Fact]
        public void DifferentTilesCategoryTest()
        {
            var tileOne = new Models.Tile { Id = "b32b5e31-20f7-4c5d-971b-c7b558049e03" };
            var tileTwo = new Models.Tile { Id = "d3d4e3eb-a90c-4dde-96c9-870f19547529" };
            var categoryRepo = new CategoryRepositoryMock();

            var categoriesForFirstDraw = Helpers.TileHelper.GetCategoriesForTile(categoryRepo, tileOne);
            var categoriesForSecondDraw = Helpers.TileHelper.GetCategoriesForTile(categoryRepo, tileTwo);

            Assert.NotEqual(categoriesForFirstDraw, categoriesForSecondDraw);
        }

        [Fact]
        public void TopLeftNeighborTest()
        {
            var tiles = Services.MapGeneratorService.GenerateMapCandidate(
                10, 10, 5
            );
            var neighbors = Helpers.TileHelper.GetNeighbors(tiles, 0, 0);
            var expectedCoordinates = new HashSet<Tuple<int, int>> {
                Tuple.Create(0, 1),
                Tuple.Create(1, 0)
            };

            Assert.Equal(neighbors.Count(), expectedCoordinates.Count());
            Assert.Equal(neighbors.Keys, expectedCoordinates);

            // TODO: Check if the tile at the coordinate is the one we expected
        }

        [Fact]
        public void TopRowOddXNeighborTest()
        {
            var tiles = Services.MapGeneratorService.GenerateMapCandidate(
                10, 10, 5
            );

            var neighbors = Helpers.TileHelper.GetNeighbors(tiles, 1, 0);
            var expectedCoordinates = new HashSet<Tuple<int, int>> {
                Tuple.Create(0, 0),
                Tuple.Create(2, 0),
                Tuple.Create(1, 0),
                Tuple.Create(1, 1),
                Tuple.Create(2, 1)
            };

            Assert.Equal(neighbors.Count(), expectedCoordinates.Count());
            Assert.Equal(neighbors.Keys, expectedCoordinates);

            // TODO: Check if the tile at the coordinate is the one we expected
        }

        public void TopRowEvenXNeighborTest()
        {
            var tiles = Services.MapGeneratorService.GenerateMapCandidate(
                10, 10, 5
            );

            var neighbors = Helpers.TileHelper.GetNeighbors(tiles, 2, 0);
            var expectedCoordinates = new HashSet<Tuple<int, int>> {
                Tuple.Create(1, 0),
                Tuple.Create(3, 0),
                Tuple.Create(2, 1)
            };

            Assert.Equal(neighbors.Count(), expectedCoordinates.Count());
            Assert.Equal(neighbors.Keys, expectedCoordinates);

            // TODO: Check if the tile at the coordinate is the one we expected
        }

        [Fact]
        public void TopRightNeighborTest()
        {
            var tiles = Services.MapGeneratorService.GenerateMapCandidate(
                10, 10, 5
            );

            var neighbors = Helpers.TileHelper.GetNeighbors(tiles, 2, 0);
            var expectedCoordinates = new HashSet<Tuple<int, int>> {
                Tuple.Create(1, 0),
                Tuple.Create(3, 0),
                Tuple.Create(2, 1)
            };

            Assert.Equal(neighbors.Count(), expectedCoordinates.Count());
            Assert.Equal(neighbors.Keys, expectedCoordinates);

            // TODO: Check if the tile at the coordinate is the one we expected
        }
    }
}