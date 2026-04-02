using Microsoft.EntityFrameworkCore;
using pr50.Models;

namespace pr50.Classes
{
    public class DatabaseManager : DbContext
    {
        /// <summary> Коллекция (таблица) объектов Storage (хранилища паролей)</summary>
        public DbSet<Storage> Storages { get; set; }

        /// <summary> Коллекция (таблица) объектов User (пользователи системы)</summary>
        public DbSet<User> Users { get; set; }

        public DatabaseManager() =>
            Database.EnsureCreated(); // - создает базу данных, если она не существует

        /// <summary>
        /// Переопределенный метод конфигурации подключения к базе данных
        /// </summary>
        /// <param name="optionsBuilder">Строитель опций для настройки контекста</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Настройка подключения к MySQL базе данных
            optionsBuilder.UseMySql(
                // Строка подключения: сервер, пользователь, пароль, база данных
                "server=127.0.0.1;uid=root;pwd=;database=Storage;",
                // Указываем версию MySQL сервера для корректной работы EF
                new MySqlServerVersion(new Version(8, 0, 11)));
        }
    }
}
