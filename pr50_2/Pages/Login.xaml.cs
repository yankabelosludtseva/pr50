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
            // Вызов статического метода Login из класса UserContext
            // Отправляет запрос на сервер и ожидает ответ
            string? Token = await UserContext.Login(login, password);
            // Проверка успешности авторизации
            if (Token == null)
            {
                // Если токен не получен - показываем сообщение об ошибке
                MessageBox.Show("Логин и пароль указаны не верно");
            }
            else
            {
                // Сохраняем полученный токен в статическом поле MainWindow
                // Токен будет использоваться для всех последующих запросов к API
                MainWindow.Token = Token;
                // Открываем главную страницу приложения
                // OpenPages - метод для навигации между страницами
                MainWindow.Init.OpenPages(new Pages.Main());
            }
        }

        /// <summary>
        /// Обработчик нажатия кнопки авторизации
        /// </summary>
        private void BthAuth(object sender, RoutedEventArgs e)
        {
            // Валидация поля логина
            if (string.IsNullOrEmpty(tbLogin.Text))
            {
                MessageBox.Show("Необходимо указать логин пользователя");
                return; // Прерываем выполнение метода
            }

            // Валидация поля пароля
            if (string.IsNullOrEmpty(tbPassword.Password))
            {
                MessageBox.Show("Необходимо указать пароль пользователя");
                return; // Прерываем выполнение метода
            }

            // Если все поля заполнены - вызываем метод авторизации
            // Передаем введенные логин и пароль
            Auth(tbLogin.Text, tbPassword.Password);
        }
    }
}
