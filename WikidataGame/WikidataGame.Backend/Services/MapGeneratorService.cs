using System;
using WikidataGame.Backend.Models;

namespace WikidataGame.Backend.Services
{
   public class MapGeneratorService {
        public static Tile[] GenerateMap() {
            // TODO: Research map generation algorithms
            var rnd = new Random();
            var mapSize = 20;

            var tiles = new Tile[mapSize];

            for (int i = 0; i < mapSize; i++)
            {
                tiles[i] = new Tile {
                    IsAccessible = rnd.NextDouble() < 0.5
                };
            }

            return tiles;
        }
   } 
}