﻿using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.AppCenter.Analytics;
using School_Meal;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace School_Meal
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>

    public sealed partial class Today : Page
    {
        public SchoolMealClass TodayClass = new SchoolMealClass();

        public Today()
        {
            this.InitializeComponent();
            Analytics.TrackEvent("Today Page");
            
            TodayClass.MoveDateCursorToToday();
            TodayHeader_TextBlock.Text = TodayClass.GetDateCursor("MM월 DD일");
        }

        private void Back_ABB_Click(object sender, RoutedEventArgs e)
        {
            TodayClass.MoveDateCursor(-1);
            TodayClass.GetDayMenu("Win10");
            TodayHeader_TextBlock.Text = TodayClass.GetDateCursor("MM월 DD일");
        }

        private void Refresh_ABB_Click(object sender, RoutedEventArgs e)
        {
            TodayClass.LoadMonthMenu("Win10");
            TodayClass.GetDayMenu("Win10");
        }

        private void Forward_ABB_Click(object sender, RoutedEventArgs e)
        {
            TodayClass.MoveDateCursor(1);
            TodayClass.GetDayMenu("Win10");
            TodayHeader_TextBlock.Text = TodayClass.GetDateCursor("MM월 DD일");
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
