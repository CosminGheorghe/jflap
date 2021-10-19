using System.Windows;

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
            Visibility = Visibility.Hidden;
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
