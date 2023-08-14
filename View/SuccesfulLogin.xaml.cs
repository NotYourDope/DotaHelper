using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Логика взаимодействия для SuccesfulLogin.xaml
    /// </summary>
    public partial class SuccesfulLogin : Window
    {
        public SuccesfulLogin()
        {
            InitializeComponent();
            btnGetStarted.Focus();
        }

        private void btnGetStarted_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

    }
}
