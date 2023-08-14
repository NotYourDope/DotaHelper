using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static DotaHelper.md5;

namespace DotaHelper.View
{
    /// <summary>
    /// Логика взаимодействия для LoginView.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        ConnectionDB dataBase = new ConnectionDB();
        public LoginView()
        {
            InitializeComponent();
            txtLogin.Focus();
        }
        
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void txtLogin_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            var loginUser = txtLogin.Text;
            var password1 = md5.hashPassword(txtPassword.Text);
            var passUser = md5.hashPassword(password1);

            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();

            string querystring = $"select login_user, password_user from users where login_user ='{loginUser}' and password_user = '{passUser}'";

            SqlCommand sqlCommand = new SqlCommand(querystring, dataBase.GetConnection());

            adapter.SelectCommand = sqlCommand;
            adapter.Fill(table);

            if (table.Rows.Count == 1 )
            {
                MainWindow window = new MainWindow();
                SuccesfulLogin succesfulLogin = new SuccesfulLogin();
                window.Login = loginUser;
                window.Show();
                window.ShowLogin();
                window.IsEnabled = false;
                succesfulLogin.ShowDialog();
                succesfulLogin.Focus();
                window.IsEnabled = true;
                this.Close();
            }
            else
            {
                this.IsEnabled = false;

                UnsuccesfulLogin unsuccesfulLogin = new UnsuccesfulLogin();

                unsuccesfulLogin.ShowDialog();
                unsuccesfulLogin.Focus();
                this.IsEnabled = true;
            }

        }

        private void registrationBtn_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            RegistrationForm form = new RegistrationForm();
            this.Close();
            form.Show();
        }

        private void NoAuthorisationBtn_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow window = new MainWindow();
            this.Close();
            window.Show();
        }

    }
}
