
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
        public void StableTileCategoryTest()
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
            var width = 10;
            var height = 10;
            var tiles = Services.MapGeneratorService.GenerateMapCandidate(
                width, height, 50
            );
            var neighbors = Helpers.TileHelper.GetNeighbors(tiles, 0, 0, width, height);
            var expectedCoordinates = new HashSet<(int, int)> {
                (1, 0),
                (0, 1)
            };

            Assert.Equal(neighbors.Count(), expectedCoordinates.Count());
            Assert.Equal(neighbors.Keys, expectedCoordinates);

            Assert.Equal(tiles.ElementAt(1), neighbors[(1, 0)]);
            Assert.Equal(tiles.ElementAt(width), neighbors[(0, 1)]);
        }

        [Fact]
        public void TopRowOddXNeighborTest()
        {
            var width = 10;
            var height = 10;
            var tiles = Services.MapGeneratorService.GenerateMapCandidate(
                width, height, 50
            );

            var neighbors = Helpers.TileHelper.GetNeighbors(tiles, 1, 0, width, height);
            var expectedCoordinates = new HashSet<(int, int)> {
                (0, 0),
                (2, 0),
                (0, 1),
                (1, 1),
                (2, 1)
            };

            Assert.Equal(expectedCoordinates.Count(), neighbors.Count());
            Assert.Equal(expectedCoordinates, neighbors.Keys);

            Assert.Equal(tiles.ElementAt(0), neighbors[(0, 0)]);
            Assert.Equal(tiles.ElementAt(2), neighbors[(2, 0)]);
            Assert.Equal(tiles.ElementAt(width), neighbors[(0, 1)]);
            Assert.Equal(tiles.ElementAt(1 + width), neighbors[(1, 1)]);
            Assert.Equal(tiles.ElementAt(2 + width), neighbors[(2, 1)]);
        }

        [Fact]
        public void TopRowEvenXNeighborTest()
        {
            var width = 10;
            var height = 10;
            var tiles = Services.MapGeneratorService.GenerateMapCandidate(
                width, height, 50
            );

            var neighbors = Helpers.TileHelper.GetNeighbors(tiles, 2, 0, width, height);
            var expectedCoordinates = new HashSet<(int, int)> {
                (1, 0),
                (3, 0),
                (2, 1)
            };

            Assert.Equal(expectedCoordinates.Count(), neighbors.Count());
            Assert.Equal(expectedCoordinates, neighbors.Keys);

            Assert.Equal(tiles.ElementAt(1), neighbors[(1, 0)]);
            Assert.Equal(tiles.ElementAt(3), neighbors[(3, 0)]);
            Assert.Equal(tiles.ElementAt(2 + width), neighbors[(2, 1)]);
        }

        [Fact]
        public void TopRightNeighborTest()
        {
            var width = 10;
            var height = 10;
            var tiles = Services.MapGeneratorService.GenerateMapCandidate(
                width, height, 50
            );

            var neighbors = Helpers.TileHelper.GetNeighbors(tiles, 9, 0, width, height);
            var expectedCoordinates = new HashSet<(int, int)> {
                (8, 0),
                (8, 1),
                (9, 1)
            };

            Assert.Equal(expectedCoordinates.Count(), neighbors.Count());
            Assert.Equal(expectedCoordinates, neighbors.Keys);

            Assert.Equal(tiles.ElementAt(8), neighbors[(8, 0)]);
            Assert.Equal(tiles.ElementAt(8 + width), neighbors[(8, 1)]);
            Assert.Equal(tiles.ElementAt(9 + width), neighbors[(8, 1)]);
        }

        [Fact]
        public void LeftEdgeNeighborTest ()
        {
            var width = 19;
            var height = 19;
            var tiles = Services.MapGeneratorService.GenerateMapCandidate(
                width, height, 200
            );

            var neighbors = Helpers.TileHelper.GetNeighbors(tiles, 0, 9, width, height);
            var expectedCoordinates = new HashSet<(int, int)> {
                (0, 8),
                (1, 8),
                (1, 9),
                (0, 10)
            };

            Assert.Equal(expectedCoordinates.Count(), neighbors.Count());
            Assert.Equal(expectedCoordinates, neighbors.Keys);

            Assert.Equal(tiles.ElementAt(8 * width), neighbors[(0, 8)]);
            Assert.Equal(tiles.ElementAt(1 + 8 * width), neighbors[(1, 8)]);
            Assert.Equal(tiles.ElementAt(1 + 9 * width), neighbors[(1, 9)]);
            Assert.Equal(tiles.ElementAt(10 * width), neighbors[(0, 10)]);
        }

        [Fact]
        public void RightEdgeNeighborTest ()
        {
            var width = 20;
            var height = 20;
            var tiles = Services.MapGeneratorService.GenerateMapCandidate(
                width, height, 200
            );

            var neighbors = Helpers.TileHelper.GetNeighbors(tiles, 19, 9, width, height);
            var expectedCoordinates = new HashSet<(int, int)> {
                (19, 8),
                (18, 9),
                (18, 10),
                (19, 10)
            };

            Assert.Equal(expectedCoordinates.Count(), neighbors.Count());
            Assert.Equal(expectedCoordinates, neighbors.Keys);

            Assert.Equal(tiles.ElementAt(19 + 8 * width), neighbors[(19, 8)]);
            Assert.Equal(tiles.ElementAt(18 + 9 * width), neighbors[(18, 9)]);
            Assert.Equal(tiles.ElementAt(18 + 10 * width), neighbors[(18, 10)]);
            Assert.Equal(tiles.ElementAt(19 + 10 * width), neighbors[(19, 10)]);
        }

        [Fact]
        public void BottomLeftNeighborTest()
        {
            var width = 4;
            var height = 4;
            var tiles = Services.MapGeneratorService.GenerateMapCandidate(
                width, height, 9
            );
            var neighbors = Helpers.TileHelper.GetNeighbors(tiles, 0, 3, width, height);
            var expectedCoordinates = new HashSet<(int, int)> {
                (0, 2),
                (1, 2),
                (1, 3)
            };

            Assert.Equal(neighbors.Count(), expectedCoordinates.Count());
            Assert.Equal(neighbors.Keys, expectedCoordinates);

            Assert.Equal(tiles.ElementAt(2 * width), neighbors[(0, 2)]);
            Assert.Equal(tiles.ElementAt(1 + 2 * width), neighbors[(1, 2)]);
            Assert.Equal(tiles.ElementAt(1 + 3 * width), neighbors[(1, 3)]);
        }

        [Fact]
        public void FindsIslandTest ()
        {
            // "x" represents and accessible tile, "o" and inaccessible one
            var horizontalIsland = new [] {
                "x", "x", "x", "x",
                "o", "o", "o", "o",
                "o", "o", "o", "o",
                "x", "x", "x", "x"
            }.Select(x => new Models.Tile { IsAccessible = x == "x" });

            var verticalIsland = new [] {
                "x", "o", "x",
                "x", "o", "x",
                "x", "o", "x"
            }.Select(x => new Models.Tile { IsAccessible = x == "x" });

            Assert.Equal(true, Helpers.TileHelper.HasIslands(horizontalIsland, 4, 4));
            Assert.Equal(true, Helpers.TileHelper.HasIslands(verticalIsland, 3, 3));
        }

        [Fact]
        public void CorrectDontHaveIslandsTest ()
        {
            var middleIsland = new [] {
                "o", "o", "o", "o",
                "o", "x", "x", "o",
                "o", "o", "o", "o"
            }.Select(x => new Models.Tile { IsAccessible = x == "x" });

            var accessibleInUpperPart = new [] {
                "x", "x", "x", "o",
                "x", "x", "o", "o",
                "x", "o", "o", "o"
            }.Select(x => new Models.Tile { IsAccessible = x == "x" });

            var accessibleInLowerPart = new [] {
                "o", "o", "o",
                "o", "o", "o",
                "o", "x", "x"
            }.Select(x => new Models.Tile { IsAccessible = x == "x" });

            var withBridge = new [] {
                "x", "x", "x",
                "o", "x", "o",
                "x", "x", "x"
            }.Select(x => new Models.Tile { IsAccessible = x == "x" });

            var completelyAccessible = Enumerable.Range(0, 9)
                .Select(_ => new Models.Tile {
                    IsAccessible = true
                });

            var completelyInaccessible = Enumerable.Range(0, 9)
                .Select(_ => new Models.Tile {
                    IsAccessible = false
                });

            Assert.Equal(false, Helpers.TileHelper.HasIslands(middleIsland, 4, 3));
            Assert.Equal(false, Helpers.TileHelper.HasIslands(accessibleInUpperPart, 4, 3));
            Assert.Equal(false, Helpers.TileHelper.HasIslands(accessibleInLowerPart, 3, 3));
            Assert.Equal(false, Helpers.TileHelper.HasIslands(withBridge, 3, 3));
            Assert.Equal(false, Helpers.TileHelper.HasIslands(completelyAccessible, 3, 3));
            Assert.Equal(false, Helpers.TileHelper.HasIslands(completelyInaccessible, 3, 3));
        }
    }
}
