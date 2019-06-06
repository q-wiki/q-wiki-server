using System;
using System.Linq;
using System.Collections.Generic;
using Xunit;
using WikidataGame.Backend;

namespace WikidataGame.Backend.Tests
{
    public class MapGeneratorTest
    {
        //Getting started: https://xunit.net/docs/getting-started/netcore/cmdline

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
        public void SetStartPositions_10x10Map_StartPositionsBeforeAndAfterJoiningAreCorrect ()
        {
            // no tiles should have an owner before two players joined
            var finalMapCandidate = Services.MapGeneratorService.GenerateMap(10, 10, 5);
            Assert.All(finalMapCandidate, tile => Assert.Null(tile.Owner));
            
            var p1 = new Models.User { Id="user-a" };
            var p2 = new Models.User { Id="user-b" };
            var players = new List<Models.User>();
            players.Add(p1);
            players.Add(p2);
            Services.MapGeneratorService.SetStartPositions(finalMapCandidate, players.Select(p => p.Id));

            var tileForP1 = finalMapCandidate.Where(t => t.OwnerId == p1.Id).First();            
            var tileForP2 = finalMapCandidate.Where(t => t.OwnerId == p2.Id).First();

            Assert.NotNull(tileForP1);
            Assert.NotNull(tileForP2);
            Assert.NotEqual(tileForP1, tileForP2);
        }
    }
}
