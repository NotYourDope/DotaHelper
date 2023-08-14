using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DotaHelper.View
{
    /// <summary>
    /// Логика взаимодействия для DraftWindow.xaml
    /// </summary>
    public partial class DraftWindow : Window
    {

        public DraftWindow()
        {
            InitializeComponent();
            string text = "Пример текста for ChatGPT";

            foreach (var word in text.Split(' '))
            {
                var isRussian = word.All(c => c >= 'а' && c <= 'я' || c >= 'А' && c <= 'Я');
                var color = isRussian ? Brushes.White : Brushes.Pink;
                testTxt.Inlines.Add(new Run(word + " ") { Foreground = color });
            }
        }
    
        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


    }
}
