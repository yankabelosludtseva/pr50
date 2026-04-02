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
using System.Xml.Linq;

namespace pr50_2.Pages
{
    /// <summary>
    /// Логика взаимодействия для Main.xaml
    /// </summary>
    public partial class Main : Page
    {
        /// <summary> Конструктор главной страницы Инициализирует компоненты и загружает ...
        public Main()
        {
            InitializeComponent();  // Инициализация XAML компонентов
            GetStorage();           // Загрузка списка хранилищ при открытии страницы
        }

        /// <summary>
        /// Асинхронный метод получения списка хранилищ с сервера
        /// Отправляет GET запрос через StorageContext и отображает элементы на странице
        /// </summary>
        public async Task GetStorage()
        {
            // Получаем список хранилищ с сервера
            // GET /storage/get с токеном в заголовке
            List<Storage> Storages = await StorageContext.Get();
            // Очищаем контейнер перед добавлением новых элементов
            StorageList.Children.Clear();
            // Для каждого хранилища создаем визуальный элемент Item
            foreach (Storage Storage in Storages)
            {
                // Добавляем элемент в контейнер
                // Item - пользовательский элемент управления, отображающий одно хранилище
                StorageList.Children.Add(new Elements.Item(Storage, this));
            }
        }

        /// <summary>
        /// Обработчик нажатия кнопки добавления нового хранилища
        /// Открывает страницу добавления
        /// </summary>
        private void OpenPageAdd(object sender, System.Windows.RoutedEventArgs e) =>
            // Открываем страницу добавления нового хранилища
            // Используется метод навигации из главного окна приложения
            MainWindow.Init.OpenPages(new Pages.Add());
    }
}
