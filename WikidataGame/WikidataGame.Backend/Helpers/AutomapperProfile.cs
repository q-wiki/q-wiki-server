using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WikidataGame.Backend.Services;

namespace WikidataGame.Backend.Helpers
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile(IServiceProvider services)
        {
            CreateMap<Models.Category, Dto.Category>();

            CreateMap<Models.User, Dto.Player>()
                .ForMember(dto => dto.Name, opt => opt.MapFrom(model => model.UserName))
                .ForMember(dto => dto.ProfileImage, opt => opt.MapFrom(model => model.ProfileImageUrl));

            CreateMap<Models.Game, Dto.GameInfo>()
                .ForMember(dto => dto.GameId, opt => opt.MapFrom(model => model.Id))
                .ForMember(dto => dto.IsAwaitingOpponentToJoin, opt => opt.MapFrom(
                    model => model.GameUsers.Count() < 2))
                .ForMember(dto => dto.Message, opt => opt.MapFrom(
                    model => model.GameUsers.Count() < 2 ? "Waiting for opponent to join." : "You matched with someone else!"));

            CreateMap<Models.Tile, Dto.Tile>()
                .ForMember(dto => dto.AvailableCategories, opt => opt.MapFrom(
                    model => (TileHelper.GetCategoriesForTile(services.GetService<CategoryCacheService>(), model.Id)).ToList()));
            
            CreateMap<Models.Game, Dto.Game>()
                .ForMember(dto => dto.AwaitingOpponentToJoin, opt => opt.MapFrom(
                    model => model.GameUsers.Count() < 2))
                .ForMember(dto => dto.WinningPlayerIds, opt => opt.MapFrom(
                    model => model.GameUsers.Where(gu => gu.IsWinner).Select(gu => gu.UserId).ToList()))
                .ForMember(dto => dto.MoveExpiry, opt => opt.MapFrom(
                    model => model.MoveStartedAt.HasValue ? model.MoveStartedAt.Value.Add(Models.Game.MaxMoveDuration) : (DateTime?)null))
                .ForMember(dto => dto.Tiles, opt => opt.ConvertUsing(new TileEnumerableValueConverter(), src => src))
                .ForMember(dto => dto.Me, opt => opt.MapFrom(
                    (model, dto, player, resContext) =>
                        model.GameUsers.SingleOrDefault(gu => gu.UserId == ((Guid)resContext.Items[nameof(Models.GameUser.UserId)]))
                    ?.User))
                .ForMember(dto => dto.Opponent, opt => opt.MapFrom(
                    (model, dto, player, resContext) =>
                        model.GameUsers.SingleOrDefault(gu => gu.UserId != ((Guid)resContext.Items[nameof(Models.GameUser.UserId)]))
                    ?.User));

            CreateMap<Models.MiniGame, Dto.MiniGameResult>()
                .ForMember(dto => dto.IsWin, opt => opt.MapFrom(
                    model => model.Status == Models.MiniGameStatus.Win))
                .ForMember(dto => dto.NextMovePlayerId, opt => opt.MapFrom(
                    model => model.Game.NextMovePlayerId))
                .ForMember(
                    dto => dto.Tiles,
                    opt => opt.ConvertUsing(
                        new TileEnumerableValueConverter(),
                        model => model.Game
                    )
                );

            CreateMap<Models.MiniGame, Dto.MiniGame>()
                .ForMember(dto => dto.Type, opt => opt.MapFrom(
                    model => (Dto.MiniGameType)Enum.Parse(typeof(Dto.MiniGameType), model.Type.ToString())));

            CreateMap<Models.MiniGame, Dto.DetailedMiniGame>()
                .IncludeBase<Models.MiniGame, Dto.MiniGame>()
                .ForMember(dto => dto.Question, opt => opt.MapFrom(model => model.Question));

            CreateMap<Models.Question, Dto.Question>()
                .ForMember(dto => dto.Category, opt => opt.MapFrom(model => model.Category))
                .ForMember(dto => dto.MiniGameType, opt => opt.MapFrom(
                    model => (Dto.MiniGameType)Enum.Parse(typeof(Dto.MiniGameType), model.MiniGameType.ToString())))
                .ForMember(dto => dto.Rating, opt => opt.MapFrom(
                    model => model.Ratings.Any() ? model.Ratings.Average(qr => qr.Rating) : 0))
                .ReverseMap()
                .ForMember(model => model.Id, opt => opt.Ignore())
                .ForMember(model => model.Ratings, opt => opt.Ignore())
                .ForMember(model => model.Status, opt => opt.Ignore())
                .ForMember(model => model.Category, opt => opt.Ignore())
                .ForMember(model => model.CategoryId, opt => opt.Ignore());

            CreateMap<Models.GameRequest, Dto.GameRequest>()
                .ForMember(dto => dto.Sender, opt => opt.MapFrom(model => model.Sender))
                .ForMember(dto => dto.Recipient, opt => opt.MapFrom(model => model.Recipient));
        }
    }
}
