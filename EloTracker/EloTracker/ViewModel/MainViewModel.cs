using EloTracker.Models;
using EloTracker.ObjectSerializers;
using EloTracker.Utilities;
using GalaSoft.MvvmLight;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace EloTracker.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private const string PLAYERS_FILE_NAME = "players.elo";
        private const string GAMES_FILE_NAME = "games.elo";

        private PenaltySettings settings;

        public History History { get; }

        private ObservableCollection<Player> _players;
        public ReadOnlyObservableCollection<Player> Players
        {
            get
            {
                return new ReadOnlyObservableCollection<Player>(_players);
            }
        }

        public AddPlayerVM AddPlayerVM { get; private set; }
        public AddGameVM AddGameVM { get; private set; }
        public HistoryVM HistoryVM { get; private set; }

        public MainViewModel()
        {
            settings = new PenaltySettings();
            this.History = new History();
            this._players = new ObservableCollection<Player>();
            AddPlayerVM = new AddPlayerVM();
            AddPlayerVM.PlayerAdded += addNewPlayer;
            AddGameVM = new AddGameVM(Players);
            AddGameVM.GameAdded += addNewGame;
            HistoryVM = new HistoryVM(this.History, Players);
            loadExecute();
        }

        private void addNewPlayer(Player player)
        {
            _players.Add(player);
            _players.Sort(Player.compareScores);
            saveExecute();
        }

        private void addNewGame(Game game)
        {
            History.AddGame(game);
            game.White.SetScore(game.CalculateWhiteScore(settings, History));
            game.Black.SetScore(game.CalculateBlackScore(settings, History));
            saveExecute();
        }

        private void saveExecute()
        {
            string playerSaveFile = Path.Combine(dataDir, PLAYERS_FILE_NAME);
            List<PlayerSerializer> pSerials = PlayerSerializer.SerializeList(Players);
            Serializer<PlayerSerializer>.Save(pSerials, playerSaveFile);

            string gameSaveFile = Path.Combine(dataDir, GAMES_FILE_NAME);
            List<GameSerializer> gSerials = GameSerializer.SerializeList(History.GameHistory);
            Serializer<GameSerializer>.Save(gSerials, gameSaveFile);

            backup();
        }
        private void loadExecute()
        {
            string playerSaveFile = Path.Combine(dataDir, "players.elo");
            if (!File.Exists(playerSaveFile)) return;
            IEnumerable<PlayerSerializer> pSerials = Serializer<PlayerSerializer>.Load(playerSaveFile);
            refreshPlayers(PlayerSerializer.UnserializeList(pSerials));

            string gameSaveFile = Path.Combine(dataDir, "games.elo");
            if (!File.Exists(gameSaveFile)) return;
            IEnumerable<GameSerializer> gSerials = Serializer<GameSerializer>.Load(gameSaveFile);
            History.Refresh(GameSerializer.UnserializeList(gSerials, Players));
        }

        private void refreshPlayers(IEnumerable<Player> players)
        {
            List<Player> playersToRemove = new List<Player>(Players);
            foreach (Player player in playersToRemove)
            {
                _players.Remove(player);
            }
            foreach (Player player in players)
            {
                _players.Add(player);
            }
            _players.Sort(Player.compareScores);
        }

        private void backup()
        {
            string backupDirectory = Path.Combine(dataDir, "Backups\\");
            if (!Directory.Exists(backupDirectory))
            {
                Directory.CreateDirectory(backupDirectory);
            }
            string thisBackup = Path.Combine(backupDirectory, string.Format("{0}\\", DateTime.Now.ToLongDateString()));
            if (!Directory.Exists(thisBackup))
            {
                Directory.CreateDirectory(thisBackup);
            }
            string playersFilePath = Path.Combine(dataDir, PLAYERS_FILE_NAME);
            string playersBackupPath = Path.Combine(thisBackup, PLAYERS_FILE_NAME);
            File.Copy(playersFilePath, playersBackupPath, true);
            string gamesFilePath = Path.Combine(dataDir, GAMES_FILE_NAME);
            string gamesBackupPath = Path.Combine(thisBackup, GAMES_FILE_NAME);
            File.Copy(gamesFilePath, gamesBackupPath, true);
        }

        private static string dataDir
        {
            get
            {
                string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                string dir = Path.Combine(appData, "ELO\\");
                return dir;
            }
        }
    }
}