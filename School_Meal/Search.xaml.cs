using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace School_Meal
{
    public sealed partial class Search : Page
    {
        public Search()
        {
            this.InitializeComponent();
        }

        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {

        }

        private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {

        }

        private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {

        }

        private void AdvancedSearch_Button_Click(object sender, RoutedEventArgs e)
        {
            if (AdvancedSearch_Row.Height == new GridLength(0))
            {
                AdvancedSearch_Row.Height = new GridLength(0,GridUnitType.Auto);
                AdvancedSearch_Icon.Glyph = "\uE971";
            }
            else
            {
                AdvancedSearch_Row.Height = new GridLength(0);
                AdvancedSearch_Icon.Glyph = "\uE972";
            }
        }

        private void Advance_StackPanel_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (Page_Grid.ActualWidth < 700)
            {
                Advance_StackPanel.Orientation = Orientation.Vertical;
            }
            else
            {
                Advance_StackPanel.Orientation = Orientation.Horizontal;
            }
        }
    }
}
