using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using System.Linq;
using System;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace School_Meal
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            var date = DateTime.Now;
            
            App_NavigationView.SelectedItem = Today_NavPaneItem;
            ContentFrame.Navigate(typeof(Today));
        }

        private void App_NavigationView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                ContentFrame.Navigate(typeof(Settings));
            }
            else
            {
                // find NavigationViewItem with Content that equals InvokedItem
                var item = sender.MenuItems.OfType<NavigationViewItem>().First(x => (string)x.Content == (string)args.InvokedItem);
                NavView_Navigate(item as NavigationViewItem);
            }
        }

        private void NavView_Navigate(NavigationViewItem item)
        {
            switch (item.Tag)
            {
                case "Today":
                    ContentFrame.Navigate(typeof(Today));
                    break;

                case "Week":
                    ContentFrame.Navigate(typeof(Week));
                    break;

                case "Month":
                    ContentFrame.Navigate(typeof(Month));
                    break;

                case "Search":
                    ContentFrame.Navigate(typeof(Search));
                    break;

                case "SchoolWeb":
                    ContentFrame.Navigate(typeof(SchoolWeb));
                    break;
            }
        }
    }
}
