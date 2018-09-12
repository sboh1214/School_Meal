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
		}

        private void Refresh_Item_Activated(object sender, EventArgs e)
        {

        }
    }
}