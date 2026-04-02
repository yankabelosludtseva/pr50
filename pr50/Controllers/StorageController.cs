using Microsoft.AspNetCore.Mvc;
using pr50.Classes;
using pr50.Models;

namespace pr50.Controllers
{
    /// <summary> Контроллер для управления хранилищами паролей пользователя</summary>
    [Route("/storage")]
    public class StorageController : Controller
    {
        /// <summary> Менеджер базы данных для работы с хранилищами</summary>
        private DatabaseManager databaseManager;

        /// <summary> Конструктор контроллера</summary>
        public StorageController() =>
            this.databaseManager = new DatabaseManager();

        /// <summary>
        /// Получение всех записей хранилища для авторизованного пользователя
        /// </summary>
        /// <param name="token">JWT токен из заголовка запроса</param>
        /// <returns>Список записей хранилища в формате DTO (без информации о пользователе)</returns>
        [Route("get")]
        [HttpGet]
        public ActionResult Get([FromHeader] string token)
        {
            try
            {
                // Извлекаем ID пользователя из JWT токена
                int? IdUser = JwtToken.GetUserIdFromToken(token);
                // Если токен недействителен или ID не получен - возвращаем 401 Unauthorized
                if (IdUser == null)
                    return StatusCode(401);

                // Получаем все записи текущего пользователя и преобразуем их в DTO
                // DTO используется чтобы скрыть информацию о пользователе из ответа
                List<StorageDto> Storages = databaseManager.Storages
                    .Where(x => x.User.Id == IdUser) // Фильтруем по ID пользователя
                    .Select(s => new StorageDto
                    {
                        // Проецируем в StorageDto
                        Id = s.Id,
                        Name = s.Name,
                        Url = s.Url,
                        Login = s.Login,
                        Password = s.Password,
                    })
                    .ToList();

                // Возвращаем 200 OK со списком записей
                return Ok(Storages);
            }
            catch (Exception exp)
            {
                // В случае ошибки возвращаем 501 с текстом ошибки
                return StatusCode(501, exp.Message);
            }
        }

        /// <summary>
        /// Добавление новой записи в хранилище
        /// </summary>
        /// <param name="token">JWT токен из заголовка</param>
        /// <param name="storage">Данные новой записи (JSON в теле запроса)</param>
        /// <returns>Добавленная запись</returns>
        [Route("add")]
        [HttpPost]
        public ActionResult Add([FromHeader] string token, [FromBody] Storage storage)
        {
            try
            {
                // Валидация токена
                int? IdUser = JwtToken.GetUserIdFromToken(token);
                if (IdUser == null)
                    return StatusCode(401);

                // Находим пользователя в БД и привязываем к новой записи
                storage.User = databaseManager.Users
                    .Where(x => x.Id == IdUser)
                    .First(); // First() выбросит исключение, если пользователь не найден

                // Добавляем запись в БД
                databaseManager.Add(storage);
                databaseManager.SaveChanges();

                // Обнуляем ссылку на пользователя, чтобы избежать циклической ссылки в JSON
                storage.User = null;
                // Возвращаем созданную запись
                return StatusCode(200, storage);
            }
            catch (Exception exp)
            {
                return StatusCode(501, exp.Message);
            }
        }

        /// <summary>
        /// Обновление существующей записи
        /// </summary>
        /// <param name="token">JWT токен из заголовка</param>
        /// <param name="storage">Обновленные данные записи</param>
        /// <returns>Обновленная запись</returns>
        [Route("update")]
        [HttpPut]
        public ActionResult Update([FromHeader] string token, [FromBody] Storage storage)
        {
            try
            {
                // Валидация токена
                int? IdUser = JwtToken.GetUserIdFromToken(token);
                // Ищем существующую запись в БД по ID
                Storage? uStorage = databaseManager.Storages
                    .Where(x => x.Id == storage.Id)
                    .FirstOrDefault();

                // Проверки прав доступа
                if (IdUser == null)
                    return StatusCode(401); // Не авторизован
                if (uStorage == null)
                    return StatusCode(404); // Запись не найдена

                // Обновляем поля существующей записи
                uStorage.Name = storage.Name;
                uStorage.Url = storage.Url;
                uStorage.Login = storage.Login;
                uStorage.Password = storage.Password; // Пароль обновляется целиком

                // Сохраняем изменения
                databaseManager.SaveChanges();

                // Очищаем навигационное свойство для JSON ответа
                storage.User = null;
                return StatusCode(200, storage);
            }
            catch (Exception exp)
            {
                return StatusCode(501, exp.Message);
            }
        }

        /// <summary>
        /// Удаление записи из хранилища
        /// </summary>
        /// <param name="token">JWT токен из заголовка</param>
        /// <param name="id">ID удаляемой записи (из формы)</param>
        /// <returns>Статус выполнения операции</returns>
        [Route("delete")]
        [HttpDelete]
        public ActionResult Delete([FromHeader] string token, [FromForm] int id)
        {
            try
            {
                // Валидация токена
                int? IdUser = JwtToken.GetUserIdFromToken(token);
                // Поиск записи, которую хочет удалить пользователь
                Storage? Storage = databaseManager.Storages
                    .Where(x => x.Id == id && x.User.Id == IdUser) // Проверяем принадлежность
                    .FirstOrDefault();

                // Проверки
                if (IdUser == null)
                    return StatusCode(401);
                if (Storage == null)
                    return StatusCode(404);

                // Удаляем запись
                databaseManager.Storages.Remove(Storage);
                databaseManager.SaveChanges();

                return StatusCode(200);
            }
            catch (Exception exp)
            {
                return StatusCode(501, exp.Message);
            }
        }
    }
}
