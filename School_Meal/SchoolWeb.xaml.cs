using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace School_Meal
{
    public sealed partial class SchoolWeb : Page
    {
        public SchoolWeb()
        {
            InitializeComponent();

            Meal_WebView.Navigate(new Uri(@"http://iasa.icehs.kr/foodlist.do?m=040406&s=isaa"));
        }

        private void Meal_WebView_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            if (args.IsSuccess)
            {
                Header_TextBlock.Text = "학교 급식사이트";
            }
            else
            {
                ShowDialog(args.WebErrorStatus.ToString());
            }
        }

        private async void ShowDialog(string content)
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = "급식 페이지 로딩 실패",
                Content = content,
                CloseButtonText = "Close"
            };
            await dialog.ShowAsync();
        }

        private void Refresh_ABB_Click(object sender, RoutedEventArgs e)
        {
            Meal_WebView.Navigate(new Uri(@"http://iasa.icehs.kr/foodlist.do?m=040406&s=isaa"));
        }
    }

    
}
