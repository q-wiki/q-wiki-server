using System;
using System.Linq;
using System.Collections.Generic;
using Xunit;
using WikidataGame.Backend;
using WikidataGame.Backend.Services;
using Xunit.Abstractions;
using WikidataGame.Backend.Helpers;

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
        public void GenerateMapCandidate_RegularSizeMap_SizeAndAmountOfAccessibleTilesAreCorrect ()
        {
            var mapCandidate = GetRegularMapCandidate();
            Assert.Equal(GameConstants.DefaultMapWidth * GameConstants.DefaultMapHeight, mapCandidate.ToList().Count());
            Assert.Equal(GameConstants.DefaultAccessibleTilesCount, mapCandidate.Where(t => t.IsAccessible).Count());
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
        public void GenerateMapCandidate_RegularSizeMap_AllAccessibleTilesHaveADifficulyInRange ()
        {
            var mapCandidate = GetRegularMapCandidate();

            Assert.Contains(mapCandidate, t => t.IsAccessible && t.Difficulty == 0);
            Assert.Contains(mapCandidate, t => t.IsAccessible && t.Difficulty == 1);
            Assert.Contains(mapCandidate, t => t.IsAccessible && t.Difficulty == 2);
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
        public void GenerateMap_RegularSizeMap_ReturnsMapWithoutIslands ()
        {
            var map = GetRegularMapCandidate();
            Assert.False(TileHelper.HasIslands(map, GameConstants.DefaultMapWidth, GameConstants.DefaultMapHeight));
        }

        [Fact]
        public void SetStartPositions_RegularSizeMap_StartPositionsBeforeAndAfterJoiningAreCorrect ()
        {
            // no tiles should have an owner before two players joined
            var finalMapCandidate = GetRegularMapCandidate();

            Assert.All(finalMapCandidate, tile => Assert.Null(tile.Owner));
            
            var p1 = new Models.User { Id = Guid.NewGuid(), UserName = "user-a" };
            var p2 = new Models.User { Id = Guid.NewGuid(), UserName = "user-b" };
            var players = new List<Models.User>
            {
                p1,
                p2
            };
            MapGeneratorService.SetStartPositions(finalMapCandidate, players.Select(p => p.Id));

            var tileForP1 = finalMapCandidate.Where(t => t.OwnerId == p1.Id).First();            
            var tileForP2 = finalMapCandidate.Where(t => t.OwnerId == p2.Id).First();

            Assert.NotNull(tileForP1);
            Assert.NotNull(tileForP2);
            Assert.NotEqual(tileForP1, tileForP2);
            Assert.Equal(0, tileForP1.Difficulty);
            Assert.Equal(0, tileForP2.Difficulty);
        }

        private IEnumerable<Models.Tile> GetRegularMapCandidate()
        {
            var tiles = MapGeneratorService.GenerateMapCandidate(
                GameConstants.DefaultMapWidth, GameConstants.DefaultMapHeight, GameConstants.DefaultAccessibleTilesCount
            );

            //Id is database generated, therefore we need to add it for tests
            tiles.ToList().ForEach(t => t.Id = Guid.NewGuid());
            return tiles;
        }
    }
}
