using System.Windows;
using System.Windows.Controls;

namespace School_Meal__Win_7_
{
    public partial class TodayPage : Page
    {
        public SchoolMealClass_WPF TodayClass = new SchoolMealClass_WPF("E100002238");

        public TodayPage()
        {
            InitializeComponent();

            TodayClass.MoveDateCursorToToday();
            ShowMenu();
            TodayHeader_TextBlock.Text = TodayClass.GetDateCursor("MM월 DD일");
        }
        
        private void Back_Button_Click(object sender, RoutedEventArgs e)
        {
            TodayClass.MoveDateCursor(-1);
            ShowMenu();
            TodayHeader_TextBlock.Text = TodayClass.GetDateCursor("MM월 DD일");
        }

        private void Refresh_Button_Click(object sender, RoutedEventArgs e)
        {
            TodayClass.LoadMonthMenu(DeviceType.Win10);
            ShowMenu();
        }

        private void Forward_Button_Click(object sender, RoutedEventArgs e)
        {
            TodayClass.MoveDateCursor(1);
            ShowMenu();
            TodayHeader_TextBlock.Text = TodayClass.GetDateCursor("MM월 DD일");
        }

        private bool ShowMenu()
        {
            try
            {
                var Menu = TodayClass.GetDayMenu(DeviceType.Win7);
                Today_Breakfast_TextBlock.Text = Menu["Breakfast"];
                Today_Lunch_TextBlock.Text = Menu["Lunch"];
                Today_Dinner_TextBlock.Text = Menu["Dinner"];
                TodayHeader_TextBlock.Text = TodayClass.GetDateCursor("MM월 DD일");
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
