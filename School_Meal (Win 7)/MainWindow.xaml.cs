using System;
using System.Windows;

namespace School_Meal__Win_7_
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var Date = DateTime.Now;
            TodayDate_TextBlock.Text = Date.Year.ToString() + "년 " + Date.Month.ToString() + "월 " + Date.Day.ToString() + "일";

            Main_Frame.Navigate(new MainWindow());
        }
    }
}
