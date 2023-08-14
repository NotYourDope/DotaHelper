using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
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

using static DotaHelper.md5;

namespace DotaHelper.View
{
    /// <summary>
    /// Логика взаимодействия для RegistrationForm.xaml
    /// </summary>
    public partial class RegistrationForm : Window
    {
        ConnectionDB dataBase = new ConnectionDB();
        public RegistrationForm()
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
            Application.Current.Shutdown();
        }

        

        private void btnRegistration_Click(object sender, RoutedEventArgs e)
        {
            
            var login = txtLogin.Text;
            var password1 = md5.hashPassword(txtPasswordSet.Text);
            var password = md5.hashPassword(password1);
            string querystring = $"insert into users(login_user, password_user) values('{login}','{password}')";
            SqlCommand command = new SqlCommand(querystring, dataBase.GetConnection());
            dataBase.openConnection();
            if (checkuser())
            {
                return;
            }
            if (command.ExecuteNonQuery() == 1)
            {
                SuccesfulRegistration succesfulRegistration = new SuccesfulRegistration();
                this.Close();
                succesfulRegistration.ShowDialog();
            }
            else
            {
                MessageBox.Show("Аккаунт не создан!");
            }
        }

        private bool checkuser()
        {
            var loginUser = txtLogin.Text;
            var passUser = txtPasswordSet.Text;

            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();
            string querystring = $"select id_user, login_user, password_user from users where login_user = '{loginUser}'";

            SqlCommand command = new SqlCommand(querystring, dataBase.GetConnection());
            adapter.SelectCommand = command;
            adapter.Fill(table);

            if (table.Rows.Count > 0)
            {
                UnsuccesfulRegistration unsuccesfulRegistration = new UnsuccesfulRegistration();
                unsuccesfulRegistration.ShowDialog();
                unsuccesfulRegistration.Focus();
                return true;
            }
            else
            {
                return false;
            }
        }

        private void btnBack_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            LoginView loginView = new LoginView(); 
            this.Close();
            loginView.Show();
        }
        private void txtLogin_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtLogin.Text.Length < 6)
            {
                btnRegistration.IsEnabled = false;
                loginerror.Text = "Слишком мало символов в логине!";
            }
            else
            {
                btnRegistration.IsEnabled = true;
                loginerror.Text = "";
            }
        }
        private void txtPasswordSet_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtPasswordSet.Text.Length < 6) { 
                btnRegistration.IsEnabled = false;
                passerror.Text = "Слишком мало символов в пароле!";
            }
            else
            {
                btnRegistration.IsEnabled = true;
                passerror.Text = "";
            }
        }
    }
}
