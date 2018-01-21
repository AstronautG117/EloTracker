using EloTracker.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EloTracker.ViewModel
{
    public class HistoryVM : ViewModelBase
    {
        private readonly History totalHistory;

        private History _activeHistory;
        public History ActiveHistory
        {
            get
            {
                return _activeHistory;
            }
            set
            {
                _activeHistory = value;
                RaisePropertyChanged("ActiveHistory");
            }
        }

        private Player _selectedPlayer;
        public Player SelectedPlayer
        {
            get { return _selectedPlayer; }
            set
            {
                _selectedPlayer = value;
                RaisePropertyChanged("SelectedPlayer");
                handleSelectedPlayerChanged(_selectedPlayer);
            }
        }

        public ReadOnlyObservableCollection<Player> Players { get; }

        public ICommand TotalHistoryCommand { get; private set; }

        public HistoryVM(History history, ReadOnlyObservableCollection<Player> players)
        {
            totalHistory = history;
            ActiveHistory = totalHistory;
            Players = players;
            _selectedPlayer = null;
            TotalHistoryCommand = new RelayCommand(totalHistoryExecute);
        }

        private void handleSelectedPlayerChanged(Player selectedPlayer)
        {
            ActiveHistory = totalHistory.filter(selectedPlayer);
        }

        private void totalHistoryExecute()
        {
            ActiveHistory = totalHistory;
        }
    }
}
