using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WikidataGame.Backend.Models;

namespace WikidataGame.Backend.Dto
{
    public class GameInfo
    {
        public Guid GameId { get; set; }
        public bool IsAwaitingOpponentToJoin { get; set; }
        public string Message { get; set; }


        public static GameInfo FromGame(Models.Game game)
        {
            if (game == null)
                return null;

            return new GameInfo
            {
                GameId = game.Id,
                IsAwaitingOpponentToJoin = game.GameUsers.Count() < 2,
                Message = game.GameUsers.Count() < 2 ? "Waiting for opponent to join." : "You matched with someone else!"
            };
        }
    }
}
