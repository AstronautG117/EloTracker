using EloTracker.Models;
using EloTracker.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EloTracker.Views
{
    /// <summary>
    /// Interaction logic for PlayerList.xaml
    /// </summary>
    public partial class PlayerList : UserControl
    {
        public IEnumerable<Player> Players
        {
            get { return (ObservableCollection<Player>)GetValue(PlayersProperty); }
            set { SetValue(PlayersProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Players.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlayersProperty =
            DependencyProperty.Register("Players", typeof(IEnumerable<Player>), typeof(PlayerList));



        public Player SelectedPlayer
        {
            get { return (Player)GetValue(SelectedPlayerProperty); }
            set { SetValue(SelectedPlayerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedPlayer.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedPlayerProperty =
            DependencyProperty.Register("SelectedPlayer", typeof(Player), typeof(PlayerList));

        public PlayerList()
        {
            InitializeComponent();
        }

        private void Rename_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedPlayer != null)
            {
                RenamePlayer renameWindow = new RenamePlayer(SelectedPlayer.Name);
                renameWindow.ShowDialog();

                if (renameWindow.NewPlayerName != "")
                {
                    SelectedPlayer.Name = renameWindow.NewPlayerName;
                }
            }
            
        }
    }
}
