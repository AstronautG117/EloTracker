using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EloTracker.Models
{
    public class PenaltySettings
    {
        private const int GAMES_TO_CHECK = 3;

        private Dictionary<Conditions, double> penalties;

        public PenaltySettings()
        {
            penalties = new Dictionary<Conditions, double>();

            //TEMPORARY FIX
            penalties.Add(new Conditions(PlayerWinState.Win, 1), 0.5);
            penalties.Add(new Conditions(PlayerWinState.Win, 2), 0.25);
            penalties.Add(new Conditions(PlayerWinState.Win, 3), 0.125);

            penalties.Add(new Conditions(PlayerWinState.Loss, 1), 1.25);
            penalties.Add(new Conditions(PlayerWinState.Loss, 2), 1.25);
            penalties.Add(new Conditions(PlayerWinState.Loss, 3), 1.25);
        }

        public double GetPenalty(Game game, Player player, History history)
        {
            History playerHistory = history.filter(player);
            playerHistory.SortByDate();

            int i = 0, gamesPlayed = 0;
            Player opponent = game.GetOtherPlayer(player);
            foreach (Game g in history.GameHistory)
            {
                if (i >= GAMES_TO_CHECK)
                {
                    break;
                }
                else if (g != game)
                {
                    if (opponent == g.GetOtherPlayer(player))
                    {
                        gamesPlayed++;
                    }
                    i++;
                }
            }

            PlayerWinState state = 0;
            if (player == game.PlayerWinner)
            {
                state = PlayerWinState.Win;
            }
            else if (game.Winner == GameWinState.Stalemate)
            {
                state = PlayerWinState.Stalemate;
            }
            else
            {
                state = PlayerWinState.Loss;
            }

            Conditions conditions = new Conditions(state, gamesPlayed);
            if (penalties.ContainsKey(conditions))
            {
                double penalty = penalties[conditions];

                string message = "";
                if (state == PlayerWinState.Win)
                {
                    message = string.Format(
                        "{0} has had winning points multiplied by {1} for playing {2} out of the last 3 " +
                        "games with {3}.",
                        player.Name,
                        penalty,
                        gamesPlayed,
                        opponent.Name);
                }
                else if (state == PlayerWinState.Loss)
                {
                    message = string.Format(
                        "{0} has had losing points multiplied by {1} for playing {2} out of the last 3 " +
                        "games with {3}.",
                        player.Name,
                        penalty,
                        gamesPlayed,
                        opponent.Name);
                }

                MessageBox.Show(message, "Penalty Applied", MessageBoxButton.OK, MessageBoxImage.Exclamation);

                return penalty;
            }
            else
            {
                return 1.0;
            }
        }

        private struct Conditions
        {
            PlayerWinState winner;
            int numPlays;

            public Conditions(PlayerWinState winner, int numPlays)
            {
                this.winner = winner;
                this.numPlays = numPlays;
            }
        }
    }


}
