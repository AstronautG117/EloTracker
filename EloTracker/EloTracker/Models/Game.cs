using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elo_Tracker.Models
{
    public enum GameWinState { White = 1, Black, Stalemate };

    public class Game
    {
        public readonly Guid Guid;

        public int WhiteStartingScore { get; }
        public int BlackStartingScore { get; }

        public Player White { get; }
        public Player Black { get; }

        public DateTime TimePlayed { get; private set; }
        public GameWinState Winner { get; private set; }

        public Player PlayerWinner
        {
            get
            {
                if (Winner == GameWinState.White)
                {
                    return White;
                }
                else if (Winner == GameWinState.Black)
                {
                    return Black;
                }
                else
                {
                    return null;
                }
            }
        }

        private Game(Player white, Player black, GameWinState winner, Guid? guid = null, int? whiteStartScore = null, int? blackStartScore = null, DateTime? timePlayed = null)
        {
            this.White = white;
            this.Black = black;
            this.Winner = winner;
            if (guid.HasValue)
            {
                this.Guid = guid.Value;
            }
            else
            {
                this.Guid = Guid.NewGuid();
            }
            if (whiteStartScore.HasValue)
            {
                this.WhiteStartingScore = whiteStartScore.Value;
            }
            else
            {
                this.WhiteStartingScore = white.Score;

            }
            if (blackStartScore.HasValue)
            {
                this.BlackStartingScore = blackStartScore.Value;
            }
            else
            {
                this.BlackStartingScore = black.Score;
            }
            if (timePlayed.HasValue)
            {
                this.TimePlayed = timePlayed.Value;
            }
            else
            {
                this.TimePlayed = DateTime.Now;
            }
        }

        public int CalculateWhiteScore(PenaltySettings settings, History history)
        {
            int winLoss = 0;
            if (Winner == GameWinState.White)
            {
                winLoss = 1;
            }
            else if (Winner == GameWinState.Black)
            {
                winLoss = -1;
            }

            double scoreChange = CalculateScoreChange(WhiteStartingScore, BlackStartingScore, winLoss);
            double penalty = settings.GetPenalty(this, White, history);
            return WhiteStartingScore + (int)Math.Round(scoreChange * penalty);
        }
        public int CalculateBlackScore(PenaltySettings settings, History history)
        {
            int winLoss = 0;
            if (Winner == GameWinState.White)
            {
                winLoss = -1;
            }
            else if (Winner == GameWinState.Black)
            {
                winLoss = 1;
            }

            double scoreChange = CalculateScoreChange(BlackStartingScore, WhiteStartingScore, winLoss);
            double penalty = settings.GetPenalty(this, Black, history);
            return BlackStartingScore + (int)Math.Round(scoreChange * penalty);
        }

        public Player GetOtherPlayer(Player player)
        {
            if (player == White)
            {
                return Black;
            }
            else if (player == Black)
            {
                return White;
            }
            else
            {
                return null;
            }
        }

        public static Game CreateNewGame(Player white, Player black, GameWinState winner)
        {
            return new Game(white, black, winner);
        }
        public static Game CreateExistingGame(Player white, Player black, GameWinState winner, Guid guid, int whiteStartScore, int blackStartScore, DateTime timePlayed)
        {
            return new Game(white, black, winner, guid, whiteStartScore, blackStartScore, timePlayed);
        }
        public static double CalculateExpectedScore(int playerScore, int otherScore)
        {
            double expectedScore = (1 / (1 + (Math.Pow(10, ((otherScore - playerScore) / 400)))));
            return expectedScore;
        }
        public static double CalculateScoreChange(int playerScore, int otherScore, int winLoss)
        {
            double modifier = (winLoss + 1) / 2.0;
            double expectedScore = CalculateExpectedScore(playerScore, otherScore);
            double scoreChange = 32 * (modifier - expectedScore);
            return scoreChange;
        }
    }
}
