using Elo_Tracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Elo_Tracker.ObjectSerializers
{
    [Serializable]
    public class GameSerializer : ISerializable
    {
        public readonly Guid Guid;
        public readonly Guid White;
        public readonly Guid Black;

        public readonly int WhiteStartingScore;
        public readonly int BlackStartingScore;

        public readonly DateTime TimePlayed;

        public readonly GameWinState Winner;

        public GameSerializer() { }

        public GameSerializer(Game game)
        {
            Guid = game.Guid;
            White = game.White.Guid;
            Black = game.Black.Guid;
            WhiteStartingScore = game.WhiteStartingScore;
            BlackStartingScore = game.BlackStartingScore;
            TimePlayed = game.TimePlayed;
            Winner = game.Winner;
        }

        public GameSerializer(SerializationInfo info, StreamingContext context)
        {
            Guid = (Guid)info.GetValue("Guid", typeof(Guid));
            White = (Guid)info.GetValue("White", typeof(Guid));
            Black = (Guid)info.GetValue("Black", typeof(Guid));
            WhiteStartingScore = (int)info.GetValue("WhiteScore", typeof(int));
            BlackStartingScore = (int)info.GetValue("BlackScore", typeof(int));
            string timePlayed = (string)info.GetValue("TimePlayed", typeof(string));
            TimePlayed = Convert.ToDateTime(timePlayed);
            Winner = (GameWinState)info.GetValue("Winner", typeof(GameWinState));
        }

        public Game GetGame(IEnumerable<Player> players)
        {
            Player white = null;
            Player black = null;

            foreach (Player player in players)
            {
                if (player.Guid == White)
                {
                    white = player;
                }
                else if (player.Guid == Black)
                {
                    black = player;
                }
                if (white != null && black != null) break;
            }

            if (white == null || black == null)
            {
                throw new DataMisalignedException("Player not found.");
            }

            return Game.CreateExistingGame(white, black, Winner, Guid, WhiteStartingScore, BlackStartingScore, TimePlayed);
        }

        public static List<GameSerializer> SerializeList(IEnumerable<Game> games)
        {
            List<GameSerializer> gSerials = new List<GameSerializer>();
            foreach (Game game in games)
            {
                gSerials.Add(new GameSerializer(game));
            }
            return gSerials;
        }

        public static List<Game> UnserializeList(IEnumerable<GameSerializer> gSerials, IEnumerable<Player> players)
        {
            List<Game> games = new List<Game>();
            foreach (GameSerializer gSerial in gSerials)
            {
                games.Add(gSerial.GetGame(players));
            }
            return games;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Guid", Guid, typeof(Guid));
            info.AddValue("White", White, typeof(Guid));
            info.AddValue("Black", Black, typeof(Guid));
            info.AddValue("WhiteScore", WhiteStartingScore, typeof(int));
            info.AddValue("BlackScore", BlackStartingScore, typeof(int));
            string timePlayed = Convert.ToString(TimePlayed);
            info.AddValue("TimePlayed", timePlayed, typeof(string));
            info.AddValue("Winner", Winner, typeof(GameWinState));
        }
    }
}
