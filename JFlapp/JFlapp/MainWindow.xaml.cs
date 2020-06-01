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

namespace JFlapp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CreateFiniteAutomaton(object sender, RoutedEventArgs e)
        {
            FiniteAutomaton finiteAutomaton = new FiniteAutomaton
            {
                Owner = this
            };
            this.Visibility = Visibility.Hidden;
            finiteAutomaton.Show();
        }

        private void CreateRegularExpressions(object sender, RoutedEventArgs e)
        {
            RegularExpressions regularExpressions = new RegularExpressions
            {
                Owner = this
            };
            this.Visibility = Visibility.Hidden;
            regularExpressions.Show();
        }
    }
}
