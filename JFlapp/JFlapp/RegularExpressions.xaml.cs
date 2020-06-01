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
using System.Windows.Shapes;

namespace JFlapp
{
    public partial class RegularExpressions : Window
    {
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            this.Owner.Show();
        }

        public RegularExpressions()
        {
            InitializeComponent();
        }

        private void ShowResult(string text)
        {
            this.Scroller.Visibility = Visibility.Visible;
            this.ResultTextBlock.Text = text;
            this.Result.Visibility = Visibility.Visible;
            this.ResultTextBlock.Visibility = Visibility.Visible;
        }

        private void ApplyRegex(object sender, RoutedEventArgs e)
        {
            string text = "";

            if (this.Pattern.Text == string.Empty && this.String.Text == string.Empty)
            {
                text = "Pattern and String fields are empty!";
                this.ShowResult(text);
                return;
            }
            else if(this.Pattern.Text == string.Empty)
            {
                text = "Pattern field is empty!";
                this.ShowResult(text);
                return;
            }
            else if(this.String.Text == string.Empty)
            {
                text = "String field is empty!";
                this.ShowResult(text);
                return;
            }

            int i = 1;
            Regex regex = new Regex(this.Pattern.Text, RegexOptions.IgnoreCase);
            MatchCollection matches = regex.Matches(this.String.Text);

            foreach (Match match in matches)
            {
                text += i++ + ". " + match.Value + "\n";
            }

            if(text == string.Empty)
            {
                text = "Your regular expression does not match the subject string.";
            }

            this.ShowResult(text);
        }
    }
}
