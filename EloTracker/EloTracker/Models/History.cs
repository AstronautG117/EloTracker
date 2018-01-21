using EloTracker.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EloTracker.Models
{
    public class History
    {
        private ObservableCollection<Game> _gameHistory;
        public ReadOnlyObservableCollection<Game> GameHistory
        {
            get
            {
                return new ReadOnlyObservableCollection<Game>(_gameHistory);
            }
        }

        public History(IEnumerable<Game> history = null)
        {
            if (history == null)
            {
                _gameHistory = new ObservableCollection<Game>();
            }
            else
            {
                _gameHistory = new ObservableCollection<Game>(history);
            }
        }

        public void Refresh(IEnumerable<Game> history)
        {
            while (_gameHistory.Count > 0)
            {
                _gameHistory.RemoveAt(0);
            }
            foreach (Game game in history)
            {
                _gameHistory.Add(game);
            }
        }

        public void AddGame(Game game)
        {
            _gameHistory.Add(game);
        }

        public History filter(Player player)
        {
            List<Game> playerHistory = new List<Game>();
            foreach (Game game in this.GameHistory)
            {
                if (game.White == player || game.Black == player)
                {
                    playerHistory.Add(game);
                }
            }
            return new History(playerHistory);
        }

        public void SortByDate()
        {
            _gameHistory.Sort(gameDateComparison);

            int gameDateComparison(Game game1, Game game2)
            {
                if (game1.TimePlayed > game2.TimePlayed)
                {
                    return 1;
                }
                else if (game1.TimePlayed < game2.TimePlayed)
                {
                    return -1;
                }
                else
                {
                    return 0;
                }
            }
        }
    }
}
