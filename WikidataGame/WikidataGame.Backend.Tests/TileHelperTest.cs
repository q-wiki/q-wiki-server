using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using Xunit;
using WikidataGame.Backend;
using WikidataGame.Backend.Models;
using WikidataGame.Backend.Repos;
using Microsoft.EntityFrameworkCore;
using WikidataGame.Backend.Services;
using Xunit.Abstractions;
using System.Threading.Tasks;

namespace WikidataGame.Backend.Tests
{
    public class TileHelperTest
    {
        private readonly ITestOutputHelper _output;

        public TileHelperTest(ITestOutputHelper output)
        {
            _output = output;
        }
        
        private IRepository<Category, Guid> CategoryRepo()
        {
            var Builderoptions = new DbContextOptionsBuilder<Helpers.DataContext>();
            Builderoptions.UseLazyLoadingProxies().UseInMemoryDatabase(Guid.NewGuid().ToString());
            var context = new Helpers.DataContext(Builderoptions.Options);
            context.Database.EnsureCreated();
            return new Repository<Category, Guid>(context);
        }

        [Fact]
        public async Task GetCategoriesForTile_SingleTile_GeneratesSameCategoriesWhenAskedRepeatedly()
        {
            // we should get stable categories for a tile
            var tile = new Tile { Id = Guid.NewGuid() };
            var categoryRepo = CategoryRepo();
            var categoryService = new CategoryCacheService(categoryRepo);
            await categoryService.InitializeAsync();
            var categoriesForFirstDraw = Helpers.TileHelper.GetCategoriesForTile(categoryService, tile.Id);
            var categoriesForSecondDraw = Helpers.TileHelper.GetCategoriesForTile(categoryService, tile.Id);

            Assert.Equal(categoriesForFirstDraw, categoriesForSecondDraw);
        }

        [Fact]
        public async Task GetCategoriesForTile_SingleTile_GeneratesDifferentCategoriesForDifferentTiles()
        {
            var tileOne = new Tile { Id = new Guid("b32b5e31-20f7-4c5d-971b-c7b558049e03") };
            var tileTwo = new Tile { Id = new Guid("d3d4e3eb-a90c-4dde-96c9-870f19547529") };
            var categoryRepo = CategoryRepo();

            var categoryService = new CategoryCacheService(categoryRepo);
            await categoryService.InitializeAsync();
            var categoriesForFirstDraw = Helpers.TileHelper.GetCategoriesForTile(categoryService, tileOne.Id);
            var categoriesForSecondDraw = Helpers.TileHelper.GetCategoriesForTile(categoryService, tileTwo.Id);

            Assert.NotEqual(categoriesForFirstDraw, categoriesForSecondDraw);
        }

        [Fact]
        public void GetNeighbors_TopLeftCorner_GivesValidNeighbors()
        {
            var width = 10;
            var height = 10;
            var tiles = MapGeneratorService.GenerateMapCandidate(
                width, height, 50
            );
            var neighbors = Helpers.TileHelper.GetNeighbors(tiles, 0, 0, width, height);
            var expectedCoordinates = new List<(int, int)> {
                (1, 0),
                (0, 1)
            };

            Assert.Equal(neighbors.Count(), expectedCoordinates.Count());
            Assert.Equal(neighbors.Keys, expectedCoordinates);

            Assert.Equal(tiles.ElementAt(1), neighbors[(1, 0)]);
            Assert.Equal(tiles.ElementAt(width), neighbors[(0, 1)]);
        }

        [Fact]
        public void GetNeighbors_TopRowOddX_GivesValidNeighbors ()
        {
            var width = 10;
            var height = 10;
            var tiles = MapGeneratorService.GenerateMapCandidate(
                width, height, 50
            );

            var neighbors = Helpers.TileHelper.GetNeighbors(tiles, 1, 0, width, height);
            var expectedCoordinates = new List<(int, int)> {
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
        public void GetNeighbors_TopRowEvenX_GivesValidNeighbors ()
        {
            var width = 10;
            var height = 10;
            var tiles = MapGeneratorService.GenerateMapCandidate(
                width, height, 50
            );

            var neighbors = Helpers.TileHelper.GetNeighbors(tiles, 2, 0, width, height);
            var expectedCoordinates = new List<(int, int)> {
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
        public void GetNeighbors_TopRightCorner_GivesValidNeighbors ()
        {
            var width = 10;
            var height = 10;
            var tiles = MapGeneratorService.GenerateMapCandidate(
                width, height, 50
            );

            var neighbors = Helpers.TileHelper.GetNeighbors(tiles, 9, 0, width, height);
            var expectedCoordinates = new List<(int, int)> {
                (8, 0),
                (8, 1),
                (9, 1)
            };

            Assert.Equal(expectedCoordinates.Count(), neighbors.Count());
            Assert.Equal(expectedCoordinates, neighbors.Keys);

            Assert.Equal(tiles.ElementAt(8), neighbors[(8, 0)]);
            Assert.Equal(tiles.ElementAt(8 + width), neighbors[(8, 1)]);
            Assert.Equal(tiles.ElementAt(9 + width), neighbors[(9, 1)]);
        }

        [Fact]
        public void GetNeighbors_LeftEdge_GivesValidNeighbors ()
        {
            var width = 19;
            var height = 19;
            var tiles = MapGeneratorService.GenerateMapCandidate(
                width, height, 200
            );

            var neighbors = Helpers.TileHelper.GetNeighbors(tiles, 0, 9, width, height);
            var expectedCoordinates = new List<(int, int)> {
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
        public void GetNeighbors_RightEdge_GivesValidNeighbors ()
        {
            var width = 20;
            var height = 20;
            var tiles = MapGeneratorService.GenerateMapCandidate(
                width, height, 200
            );

            var neighbors = Helpers.TileHelper.GetNeighbors(tiles, 19, 9, width, height);
            var expectedCoordinates = new List<(int, int)> {
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
        public void GetNeighbors_BottomLeftCorner_GivesValidNeighbors ()
        {
            var width = 4;
            var height = 4;
            var tiles = MapGeneratorService.GenerateMapCandidate(
                width, height, 9
            );
            var neighbors = Helpers.TileHelper.GetNeighbors(tiles, 0, 3, width, height);
            var expectedCoordinates = new List<(int, int)> {
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
        public void GetNeighbors_BottomRightCorner_GivesValidNeighbors ()
        {
            var width = 4;
            var height = 4;
            var tiles = MapGeneratorService.GenerateMapCandidate(
                width, height, 6
            );
            var neighbors = Helpers.TileHelper.GetNeighbors(tiles, 3, 3, width, height);
            var expectedCoordinates = new List<(int, int)> {
                (3, 2),
                (2, 3)
            };

            Assert.Equal(neighbors.Count(), expectedCoordinates.Count());
            Assert.Equal(neighbors.Keys, expectedCoordinates);

            Assert.Equal(tiles.ElementAt(3 + 2 * width), neighbors[(3, 2)]);
            Assert.Equal(tiles.ElementAt(2 + 3 * width), neighbors[(2, 3)]);
        }

        [Fact]
        public void HasIslands_MapsWithIslands_IdentifiedCorrectly ()
        {
            // "x" represents an accessible tile, "o" and inaccessible one
            var horizontalIsland = new [] {
                "x", "x", "x", "x",
                "o", "o", "o", "o",
                "o", "o", "o", "o",
                "x", "x", "x", "x"
            }.Select(x => new Tile { IsAccessible = x == "x" });

            var verticalIsland = new [] {
                "x", "o", "o", "x",
                "x", "o", "o", "x",
                "x", "o", "o", "x",
                "x", "o", "o", "x"
            }.Select(x => new Tile { IsAccessible = x == "x" });

            var smallSeparatedPart = new [] {
                "x", "o", "x",
                "o", "o", "x",
                "o", "x", "x"
            }.Select(x => new Tile { IsAccessible = x == "x" });

            Assert.True(Helpers.TileHelper.HasIslands(horizontalIsland, 4, 4));
            Assert.True(Helpers.TileHelper.HasIslands(verticalIsland, 4, 4));
            Assert.True(Helpers.TileHelper.HasIslands(smallSeparatedPart, 3, 3));
        }

        [Fact]
        public void HasIslands_MapsWithOnlyContinuousTiles_IdentifiedCorrectly ()
        {
            var middleIsland = new [] {
                "o", "o", "o", "o",
                "o", "x", "x", "o",
                "o", "o", "o", "o"
            }.Select(x => new Tile { IsAccessible = x == "x" });

            var accessibleInUpperPart = new [] {
                "x", "x", "x", "o",
                "x", "x", "o", "o",
                "x", "o", "o", "o"
            }.Select(x => new Tile { IsAccessible = x == "x" });

            var accessibleInLowerPart = new [] {
                "o", "o", "o",
                "o", "o", "o",
                "o", "x", "x"
            }.Select(x => new Tile { IsAccessible = x == "x" });

            var withBridge = new [] {
                "x", "x", "x",
                "o", "x", "o",
                "x", "x", "x"
            }.Select(x => new Tile { IsAccessible = x == "x" });

            var completelyAccessible = Enumerable.Range(0, 9)
                .Select(_ => new Tile {
                    IsAccessible = true
                });

            var completelyInaccessible = Enumerable.Range(0, 9)
                .Select(_ => new Tile {
                    IsAccessible = false
                });

            Assert.False(Helpers.TileHelper.HasIslands(middleIsland, 4, 3));
            Assert.False(Helpers.TileHelper.HasIslands(accessibleInUpperPart, 4, 3));
            Assert.False(Helpers.TileHelper.HasIslands(accessibleInLowerPart, 3, 3));
            Assert.False(Helpers.TileHelper.HasIslands(withBridge, 3, 3));
            Assert.False(Helpers.TileHelper.HasIslands(completelyAccessible, 3, 3));
            Assert.False(Helpers.TileHelper.HasIslands(completelyInaccessible, 3, 3));
        }

        [Fact]
        public void HasIslands_AdvancedCasesFromRealUsage_IdentifiedCorrectly ()
        {
            var ex1 = new [] {
                "x", "o", "o", "x", "x", "x", "x", "o", "x", "x",
                "o", "x", "x", "o", "x", "o", "o", "x", "o", "x",
                "x", "x", "o", "x", "x", "x", "o", "x", "x", "x",
                "x", "o", "x", "o", "x", "o", "x", "x", "x", "x",
                "x", "x", "o", "x", "o", "x", "x", "x", "x", "x",
                "x", "x", "x", "x", "o", "x", "x", "x", "o", "x",
                "x", "x", "x", "o", "x", "x", "x", "x", "x", "o",
                "x", "x", "x", "x", "o", "x", "x", "x", "o", "x",
                "o", "o", "o", "x", "o", "x", "o", "x", "o", "o",
                "x", "o", "x", "x", "x", "o", "x", "x", "x", "x"
            }.Select(x => new Models.Tile { IsAccessible = x == "x" });
            
            var ex2 = new [] {
                "x", "o", "x", "o", "x", "x", "o", "x",
                "o", "x", "x", "x", "x", "x", "x", "x",
                "x", "x", "x", "x", "x", "o", "o", "x",
                "x", "x", "x", "x", "x", "x", "x", "o",
                "x", "x", "x", "x", "o", "x", "x", "x",
                "x", "x", "x", "x", "o", "x", "x", "x",
                "o", "x", "x", "o", "x", "x", "x", "x",
                "x", "x", "x", "o", "x", "o", "x", "o"
            }.Select(x => new Models.Tile { IsAccessible = x == "x" });

            Assert.True(Helpers.TileHelper.HasIslands(ex1, 10, 10));
            Assert.True(Helpers.TileHelper.HasIslands(ex2, 8, 8));
        }
    }
}
