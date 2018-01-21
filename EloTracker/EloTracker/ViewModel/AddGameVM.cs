using Elo_Tracker.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static Elo_Tracker.Models.Game;

namespace Elo_Tracker.ViewModel
{
    public class AddGameVM : ViewModelBase
    {
        private Player _black;
        private Player _white;
        private GameWinState _winner;

        public Player White
        {
            get
            {
                return _white;
            }
            set
            {
                _white = value;
                RaisePropertyChanged("White");
            }
        }
        public Player Black
        {
            get
            {
                return _black;
            }
            set
            {
                _black = value;
                RaisePropertyChanged("Black");
            }
        }

        public GameWinState Winner
        {
            get
            {
                return _winner;
            }
            set
            {
                _winner = value;
                RaisePropertyChanged("Winner");
            }
        }

        public ReadOnlyObservableCollection<Player> Players { get; }

        public ICommand AddGameCommand { get; private set; }

        public event Action<Game> GameAdded;

        public AddGameVM(ReadOnlyObservableCollection<Player> players)
        {
            Players = players;
            White = null;
            Black = null;
            Winner = 0;
            AddGameCommand = new RelayCommand(addGameExecute, addGameCanExecute);
        }

        private void addGameExecute()
        {
            Game newGame = Game.CreateNewGame(White, Black, Winner);
            GameAdded?.Invoke(newGame);
            White = null;
            Black = null;
        }
        private bool addGameCanExecute()
        {
            return (White != null
                && Black != null
                && Black != White
                && Winner != 0);
        }
    }
}
