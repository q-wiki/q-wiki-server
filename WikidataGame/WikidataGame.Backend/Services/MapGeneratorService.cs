using System;
using System.Collections.Generic;
using System.Linq;
using WikidataGame.Backend.Models;

namespace WikidataGame.Backend.Services
{
    public class MapGeneratorService {
        public static IEnumerable<Tile> GenerateMap() {
            // TODO: Research map generation algorithms
            var rnd = new Random();
            var mapSize = 5 * 5;

            // we said we'd have about 18 tiles that you can access
            var propability = 18.0 / mapSize;

            var tiles = new Tile[mapSize];

            for (int i = 0; i < mapSize; i++)
            {
                tiles[i] = new Tile {
                    IsAccessible = rnd.NextDouble() <= propability
                };
            }

            return tiles;
        }

        public static void Debug (IEnumerable<Tile> tiles) {
            var t = tiles.ToArray();
            for (int i = 0; i < tiles.Count(); i++)
            {
                System.Console.Write(t[i]);
                if (i % 5 == 0) System.Console.WriteLine();
            }
            System.Console.WriteLine();
            System.Console.WriteLine($"Accessible tiles: {tiles.Count(x => x.IsAccessible)}");
            System.Console.WriteLine();
        }
    } 
}