using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace School_Meal_Mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : MasterDetailPage
	{
		public MainPage()
		{
			InitializeComponent();

            NavPage.Today_Button.Clicked += Today_Button_Clicked;
            NavPage.Week_Button.Clicked += Week_Button_Clicked;
            NavPage.Search_Button.Clicked += Search_Button_Clicked;
            NavPage.Web_Button.Clicked += SchoolWeb_Button_Clicked;
        }
        
        public void Today_Button_Clicked(object sender, EventArgs e)
        {
            Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(TodayPage)));
            IsPresented = false;
        }

        public void Week_Button_Clicked(object sender, EventArgs e)
        {
            Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(WeekPage)));
            IsPresented = false;
        }

        public void Search_Button_Clicked(object sender, EventArgs e)
        {
            Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(SearchPage)));
            IsPresented = false;
        }

        public void SchoolWeb_Button_Clicked(object sender, EventArgs e)
        {
            Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(SchoolWebPage)));
            IsPresented = false;
        }
    }
}
