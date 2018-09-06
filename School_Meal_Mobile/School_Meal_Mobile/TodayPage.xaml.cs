using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace School_Meal_Mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TodayPage : ContentPage
	{
        public DateTime TodayPageDateTime = DateTime.Now;

        public TodayPage ()
		{
			InitializeComponent ();
        }

        
    }
}