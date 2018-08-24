using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace School_Meal
{
    public sealed partial class SchoolWeb : Page
    {
        public SchoolWeb()
        {
            this.InitializeComponent();

            Meal_WebView.Navigate(new System.Uri(@"http://iasa.icehs.kr/foodlist.do?m=040406&s=isaa"));
        }

        private void Meal_WebView_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            if (args.IsSuccess == true)
            {
                Header_TextBlock.Text = "학교 급식사이트";
            }
            else
            {
                Header_TextBlock.Text = "급식 페이지 로딩 실패" + args.WebErrorStatus.ToString();
            }
        }

        private void Refresh_ABB_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Meal_WebView.Refresh();
        }
    }
}
