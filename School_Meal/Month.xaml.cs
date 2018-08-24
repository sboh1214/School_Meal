using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace School_Meal
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Month : Page
    {
        public Month()
        {
            this.InitializeComponent();
        }
        
        private void Back_ABB_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Refresh_ABB_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Forward_ABB_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Breakfast_ABB_Click(object sender, RoutedEventArgs e)
        {
            Lunch_ABB.IsChecked = false;
            Dinner_ABB.IsChecked = false;
        }

        private void Lunch_ABB_Click(object sender, RoutedEventArgs e)
        {
                Breakfast_ABB.IsChecked = false;
                Dinner_ABB.IsChecked = false;
        }

        private void Dinner_ABB_Click(object sender, RoutedEventArgs e)
        {
            Breakfast_ABB.IsChecked = false;
            Lunch_ABB.IsChecked = false;

        }
    }
}
