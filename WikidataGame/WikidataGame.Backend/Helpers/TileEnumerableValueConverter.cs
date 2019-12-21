using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WikidataGame.Backend.Dto;
using WikidataGame.Backend.Models;
using WikidataGame.Backend.Services;

namespace WikidataGame.Backend.Helpers
{
    public class TileEnumerableValueConverter : IValueConverter<Models.Game, IEnumerable<IEnumerable<Dto.Tile>>>
    {
        public IEnumerable<IEnumerable<Dto.Tile>> Convert(Models.Game sourceMember, ResolutionContext context)
        {
            return Enumerable.Range(0, sourceMember.MapHeight)
                .Select(yCoord =>
                    sourceMember.Tiles.Skip(yCoord * sourceMember.MapWidth)
                        .Take(sourceMember.MapWidth)
                        // inaccessible tiles are represented as `null`
                        .Select(t => t.IsAccessible ? context.Mapper.Map<Dto.Tile>(t) : null)
                );
        }
    }
}
