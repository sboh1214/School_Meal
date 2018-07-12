using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.AppCenter.Analytics;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace School_Meal
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>

    public sealed partial class Today : Page
    {
        public DateTime TodayPageDateTime = DateTime.Now;

        public Today()
        {
            this.InitializeComponent();
            Analytics.TrackEvent("Today Page");

            TodayHeader_TextBlock.Text = TodayPageDateTime.Month.ToString()+"월 "+TodayPageDateTime.Day.ToString()+"일 급식";
            TodayProgressBar_Row.Height = new GridLength(12);
            ShowMenu();
            TodayProgressBar_Row.Height = new GridLength(0);
        }

        public bool ShowMenu()
        {
            
            return true;
        }

        private void Back_ABB_Click(object sender, RoutedEventArgs e)
        {
            TodayProgressBar_Row.Height = new GridLength(12);
            TodayPageDateTime = TodayPageDateTime.AddDays(-1);
            TodayHeader_TextBlock.Text = TodayPageDateTime.Month.ToString()+"월 "+TodayPageDateTime.Day.ToString()+"일 급식";
            ShowMenu();
            TodayProgressBar_Row.Height = new GridLength(0);
        }

        private void Refresh_ABB_Click(object sender, RoutedEventArgs e)
        {
            TodayHeader_TextBlock.Text = TodayPageDateTime.Month.ToString()+"월 "+TodayPageDateTime.Day.ToString()+"일 급식";
            TodayProgressBar_Row.Height = new GridLength(12);
            ShowMenu();
            TodayProgressBar_Row.Height = new GridLength(0);
        }

        private void Forward_ABB_Click(object sender, RoutedEventArgs e)
        {
            TodayProgressBar_Row.Height = new GridLength(12);
            TodayPageDateTime = TodayPageDateTime.AddDays(1);
            TodayHeader_TextBlock.Text = TodayPageDateTime.Month.ToString()+"월 "+TodayPageDateTime.Day.ToString()+"일 급식";
            ShowMenu();
            TodayProgressBar_Row.Height = new GridLength(0);
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.Width>=1008)
            {
                Today_Breakfast_TextBlock.FontSize = 26;
                Today_Lunch_TextBlock.FontSize = 26;
                Today_Dinner_TextBlock.FontSize = 26;
            }
            else if (e.NewSize.Width>=641)
            {
                Today_Breakfast_TextBlock.FontSize = 18;
                Today_Lunch_TextBlock.FontSize = 18;
                Today_Dinner_TextBlock.FontSize = 18;
            }
            else
            {
                Today_Breakfast_TextBlock.FontSize = 12;
                Today_Lunch_TextBlock.FontSize = 12;
                Today_Dinner_TextBlock.FontSize = 12;
            }
        }
    }
}
