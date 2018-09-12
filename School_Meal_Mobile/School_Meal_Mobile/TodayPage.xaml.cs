using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace School_Meal_Mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TodayPage : ContentPage
	{
        SchoolMealClass_Xamarin TodayClass = new SchoolMealClass_Xamarin("E100002238");

        public TodayPage ()
		{
			InitializeComponent ();
            TodayClass.MoveDateCursorToToday();
            ShowMenu();
        }

        public bool ShowMenu()
        {
            try
            {
                var MealDictionary = TodayClass.GetDayMenu(DeviceType.XF);
                Today_Breakfast_Label.Text = MealDictionary["Breakfast"];
                Today_Lunch_Label.Text = MealDictionary["Lunch"];
                Today_Dinner_Label.Text = MealDictionary["Dinner"];
                this.Title = TodayClass.GetDateCursor("MM월 DD일");
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void Refresh_Item_Activated(object sender, System.EventArgs e)
        {
            TodayClass.LoadMonthMenu(DeviceType.XF);
            ShowMenu();
        }

        private void Today_Item_Activated(object sender, System.EventArgs e)
        {
            TodayClass.MoveDateCursorToToday();
            ShowMenu();
        }

        private void Move_Item_Activated(object sender, System.EventArgs e)
        {

        }

        private void Previous_Activated(object sender, System.EventArgs e)
        {
            TodayClass.MoveDateCursor(-1);
            ShowMenu();
        }

        private void Next_Activated(object sender, System.EventArgs e)
        {
            TodayClass.MoveDateCursor(1);
            ShowMenu();
        }
    }
}