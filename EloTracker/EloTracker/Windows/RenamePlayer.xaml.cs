using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace EloTracker.Windows
{
    /// <summary>
    /// Interaction logic for RenamePlayer.xaml
    /// </summary>
    public partial class RenamePlayer : Window
    {
        public string OldPlayerName { get; }
        public string NewPlayerName { get; set; }

        public RenamePlayer(string oldPlayerName)
        {
            this.OldPlayerName = oldPlayerName;
            this.NewPlayerName = "";
            InitializeComponent();
        }

        private void Rename_Click(object sender, RoutedEventArgs e)
        {
            if (NewPlayerName != "")
            {
                this.Close();
            }
            else
            {
                MessageBox.Show("You must enter a name.", "Enter name!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
