using System;
using System.Linq;
using System.Collections.Generic;
using Xunit;
using WikidataGame.Backend;
using WikidataGame.Backend.Services;
using Xunit.Abstractions;

namespace WikidataGame.Backend.Tests
{
    public class MapGeneratorTest
    {
        //Getting started: https://xunit.net/docs/getting-started/netcore/cmdline
        private readonly ITestOutputHelper _output;

        public MapGeneratorTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void GenerateMapCandidate_10x10Map_SizeAndAmountOfAccessibleTilesAreCorrect ()
        {
            var mapCandidate = Services.MapGeneratorService.GenerateMapCandidate(
                10, 10, 5
            );
            Assert.Equal(10 * 10, mapCandidate.ToList().Count());
            Assert.Equal(5, mapCandidate.Where(t => t.IsAccessible).Count());
        }
        

        [Fact]
        public void GenerateMapCandidate_Odd19x19Map_SizeAndAmountOfAccessibleTilesAreCorrect ()
        {
            var mapWidth = 19;
            var mapHeight = 19;
            var accessibleTiles = 5;
            var mapCandidate = Services.MapGeneratorService.GenerateMapCandidate(
                mapWidth, mapHeight, accessibleTiles
            );
            Assert.Equal(mapWidth * mapHeight, mapCandidate.ToList().Count());
            Assert.Equal(accessibleTiles, mapCandidate.Where(t => t.IsAccessible).Count());
        }

        [Fact]
        public void GenerateMapCandidate_10x0Map_AllAccessibleTilesHaveADifficulyInRange ()
        {
            var mapWidth = 10;
            var mapHeight = 10;
            var accessibleTiles = 70;
            var mapCandidate = Services.MapGeneratorService.GenerateMapCandidate(
                mapWidth, mapHeight, accessibleTiles
            );

            Assert.True(mapCandidate.Count(t => t.IsAccessible && t.Difficulty == 0) >= 1);
            Assert.True(mapCandidate.Count(t => t.IsAccessible && t.Difficulty == 1) >= 1);
            Assert.True(mapCandidate.Count(t => t.IsAccessible && t.Difficulty == 2) >= 1);
            Assert.All(mapCandidate, t => Assert.True(!t.IsAccessible || t.Difficulty >= 0 && t.Difficulty <= 2));
        }

        [Fact]
        public void GenerateMapCandidate_NonSquare8x21Map_SizeAndAmountOfAccessibleTilesAreCorrect ()
        {
            var mapWidth = 8;
            var mapHeight = 21;
            var accessibleTiles = 101;
            var mapCandidate = Services.MapGeneratorService.GenerateMapCandidate(
                mapWidth, mapHeight, accessibleTiles
            );
            Assert.Equal(mapWidth * mapHeight, mapCandidate.ToList().Count());
            Assert.Equal(accessibleTiles, mapCandidate.Where(t => t.IsAccessible).Count());
        }

        [Fact]
        public void GenerateMap_10x10Map_ReturnsMapWithoutIslands ()
        {
            var mapWidth = 10;
            var mapHeight = 10;
            var accessibleTiles = 65;
            var map = Services.MapGeneratorService.GenerateMap(
                mapWidth, mapHeight, accessibleTiles
            );

            Assert.False(Helpers.TileHelper.HasIslands(map, mapWidth, mapHeight));
        }

        [Fact]
        public void SetStartPositions_10x10Map_StartPositionsBeforeAndAfterJoiningAreCorrect ()
        {
            // no tiles should have an owner before two players joined
            var finalMapCandidate = Services.MapGeneratorService.GenerateMap(10, 10, 5);

            Assert.All(finalMapCandidate, tile => Assert.Null(tile.Owner));
            
            var p1 = new Models.User { Id="user-a" };
            var p2 = new Models.User { Id="user-b" };
            var players = new List<Models.User>
            {
                p1,
                p2
            };
            Services.MapGeneratorService.SetStartPositions(finalMapCandidate, players.Select(p => p.Id));

            var tileForP1 = finalMapCandidate.Where(t => t.OwnerId == p1.Id).First();            
            var tileForP2 = finalMapCandidate.Where(t => t.OwnerId == p2.Id).First();

            Assert.NotNull(tileForP1);
            Assert.NotNull(tileForP2);
            Assert.NotEqual(tileForP1, tileForP2);
            Assert.Equal(0, tileForP1.Difficulty);
            Assert.Equal(0, tileForP2.Difficulty);
        }
    }
}
