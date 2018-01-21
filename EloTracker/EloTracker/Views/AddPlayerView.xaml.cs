using EloTracker.ViewModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EloTracker.Views
{
    /// <summary>
    /// Interaction logic for AddPlayerView.xaml
    /// </summary>
    public partial class AddPlayerView : UserControl
    {
        public AddPlayerVM VM
        {
            get { return (AddPlayerVM)GetValue(VMProperty); }
            set { SetValue(VMProperty, value); }
        }

        // Using a DependencyProperty as the backing store for VM.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty VMProperty =
            DependencyProperty.Register("VM", typeof(AddPlayerVM), typeof(AddPlayerView));



        public AddPlayerView()
        {
            InitializeComponent();
        }
    }
}
