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
    /// Логика взаимодействия для Add.xaml
    /// </summary>
    public partial class Add : Page
    {
        /// <summary> Хранит редактируемую запись (если не null - режим редактирования) ...
        Storage ChangeStorage;

        public Add(Storage storage = null)
        {
            InitializeComponent();
            ChangeStorage = storage; // Сохраняем переданную запись
            // Если передан объект для редактирования - заполняем поля формы
            if (ChangeStorage != null)
            {
                tbName.Text = ChangeStorage.Name;      // Название сервиса
                tbUrl.Text = ChangeStorage.Url;        // URL сервиса
                tbLogin.Text = ChangeStorage.Login;    // Логин
                tbPassword.Text = ChangeStorage.Password; // Пароль
            }
        }

        /// <summary> Обработчик нажатия кнопки "Сохранить" Сохраняет новую запись или о ...
        private void Save(object sender, RoutedEventArgs e)
        {
            // РЕЖИМ 1: Создание новой записи
            if (ChangeStorage == null)
            {
                // Создаем новый объект Storage с данными из полей ввода
                Storage storage = new Storage()
                {
                    Name = tbName.Text,         // Название из текстового поля
                    Url = tbUrl.Text,           // URL из текстового поля
                    Login = tbLogin.Text,       // Логин из текстового поля
                    Password = tbPassword.Text, // Пароль из текстового поля
                };

                // Отправляем POST запрос на сервер для создания новой записи
                StorageContext.Add(storage);
            }
            // РЕЖИМ 2: Редактирование существующей записи
            else
            {
                // Обновляем поля существующего объекта данными из формы
                ChangeStorage.Name = tbName.Text;
                ChangeStorage.Url = tbUrl.Text;
                ChangeStorage.Login = tbLogin.Text;
                ChangeStorage.Password = tbPassword.Text;

                // Отправляем PUT запрос на сервер для обновления записи
                StorageContext.Update(ChangeStorage);
            }

            // Показываем сообщение об успешном сохранении
            MessageBox.Show("Данные сохранены");
            // Возвращаемся на главную страницу
            MainWindow.Init.OpenPages(new Pages.Main());
        }

        /// <summary> Обработчик нажатия кнопки "Назад" Возвращает пользователя на глав ...
        private void Back(object sender, RoutedEventArgs e) =>
            MainWindow.Init.OpenPages(new Pages.Main());
    }
}
