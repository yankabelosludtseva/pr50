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
using System.Windows.Navigation;
using System.Windows.Shapes;
using pr50_2.Context;

namespace pr50_2.Pages
{
    /// <summary>
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        /// <summary> Конструктор страницы логина Вызывается при создании экземпляра стр ...
        public Login()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Асинхронный метод аутентификации пользователя
        /// Отправляет логин и пароль на сервер и получает токен
        /// </summary>
        /// <param name="login">Логин пользователя (введенный в текстовое поле)</param>
        /// <param name="password">Пароль пользователя (введенный в поле пароля)</param>
        public async Task Auth(string login, string password)
        {
            string Token = await UserContext.Login(login, password);
            if (Token == null)
            {
                MessageBox.Show("Логин и пароль указаны не верно");
            }
            else
            {
                MainWindow.Token = Token;
                MainWindow.Init.OpenPages(new Pages.Main());
            }
        }

        private async void BtnAuth(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tbLogin.Text))
            {
                MessageBox.Show("Необходимо указать логин пользователя");
                return;
            }
            if (string.IsNullOrEmpty(tbPassword.Password))
            {
                MessageBox.Show("Необходимо указать пароль пользователя");
                return;
            }
            await Auth(tbLogin.Text, tbPassword.Password); // ✅ Добавлен await
        }
    }
}
