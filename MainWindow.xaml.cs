using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace ValuteConverter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //DataContext = new ViewModel();
        }
        private Regex regex = new Regex("^[0-9,.]*$");
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if(!regex.IsMatch(e.Text))
            {
                e.Handled = true;
                return;
            }
        }

        private void TextBox_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Copy ||
         e.Command == ApplicationCommands.Cut ||
         e.Command == ApplicationCommands.Paste)
            {
                e.Handled = true;
            }
        }
    }
}
