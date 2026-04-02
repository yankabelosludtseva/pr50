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

namespace pr50_2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Статическая ссылка на экземпляр главного окна
        /// Используется для доступа к методам навигации из любого места приложения
        /// Паттерн "Одиночка" (Singleton) для простого доступа к главному окну
        /// </summary>
        public static MainWindow Init;

        /// <summary>
        /// Статическое поле для хранения JWT токена аутентификации
        /// Доступно из любой точки приложения после успешного входа
        /// </summary>
        public static string Token;

        /// <summary> Конструктор главного окна</summary>
        public MainWindow()
        {
            InitializeComponent(); // Инициализация XAML компонентов (включая frame)
            Init = this; // Сохраняем ссылку на текущий экземпляр для статического доступа
            // Открываем страницу логина при запуске приложения
            OpenPages(new Pages.Login());
        }

        /// <summary>
        /// Метод навигации между страницами
        /// Отображает переданную страницу внутри фрейма frame
        /// </summary>
        /// <param name="openPage">Страница для отображения (Login, Main, Add)</param>
        public void OpenPages(Page openPage)
        {
            // Используем встроенный механизм навигации WPF
            // Frame - контейнер, который может отображать разные Page
            frame.Navigate(openPage);
        }
    }
}
