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
using pr50_2.Pages;

namespace pr50_2.Elements
{
    /// <summary>
    /// Логика взаимодействия для Item.xaml
    /// </summary>
    public partial class Item : UserControl
    {
        /// <summary> Данные хранилища, отображаемые в этом элементе</summary>
        Storage Storage;

        /// <summary> Ссылка на главную страницу для обновления списка после удаления</summary>
        Main Main;

        /// <summary>
        /// Конструктор элемента
        /// </summary>
        /// <param name="storage">Данные для отображения</param>
        /// <param name="main">Ссылка на главную страницу</param>
        public Item(Storage storage, Main main)
        {
            InitializeComponent(); // Инициализация XAML компонентов

            // Заполняем текстовые поля данными из модели
            tbName.Text = storage.Name;      // Название сервиса
            tbUrl.Text = storage.Url;        // URL сервиса
            tbLogin.Text = storage.Login;    // Логин
            tbPassword.Text = storage.Password; // Пароль

            // Сохраняем ссылки для дальнейшего использования
            this.Main = main;          // Для доступа к StorageList
            this.Storage = storage;    // Для хранения данных
        }

        /// <summary> Обработчик нажатия кнопки "Редактировать"</summary>
        private void Update(object sender, System.Windows.RoutedEventArgs e)
        {
            // Открываем страницу добавления/редактирования
            // Передаем текущий объект Storage для редактирования
            MainWindow.Init.OpenPages(new Pages.Add(Storage));
        }

        /// <summary> Обработчик нажатия кнопки "Удалить"</summary>
        private void Delete(object sender, System.Windows.RoutedEventArgs e)
        {
            // 1. Отправляем DELETE запрос на сервер
            StorageContext.Delete(Storage.Id);

            // 2. Удаляем элемент из визуального списка на главной странице
            this.Main.StorageList.Children.Remove(this);

            // 3. Показываем сообщение об успешном удалении
            MessageBox.Show("Данные удалены");
        }
    }
}
