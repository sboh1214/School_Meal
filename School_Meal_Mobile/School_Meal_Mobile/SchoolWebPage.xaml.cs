using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace School_Meal_Mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SchoolWebPage : ContentPage
	{
		public SchoolWebPage ()
		{
			InitializeComponent ();
            School_WebView.Source = "http://iasa.icehs.kr/foodlist.do?m=040406&s=isaa";

        }

        private void Refresh_Item_Activated(object sender, EventArgs e)
        {
            School_WebView.Source = "http://iasa.icehs.kr/foodlist.do?m=040406&s=isaa";
        }
    }
}