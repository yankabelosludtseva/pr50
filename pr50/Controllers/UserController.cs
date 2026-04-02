using Microsoft.AspNetCore.Mvc;
using pr50.Classes;
using pr50.Models;

namespace pr50.Controllers
{
    [Route("/user")]
    public class UserController : Controller
    {
        /// <summary>
        /// Приватное поле для хранения экземпляра DatabaseManager
        /// Используется для работы с базой данных
        /// </summary>
        private DatabaseManager databaseManager;

        /// <summary> Конструктор контроллера</summary>
        public UserController()
        {
            // Сохраняем полученный экземпляр DatabaseManager в приватное поле
            this.databaseManager = new DatabaseManager();
        }

        /// <summary>
        /// Метод для аутентификации пользователя
        /// </summary>
        /// <param name="login">Логин пользователя</param>
        /// <param name="password">Пароль пользователя</param>
        /// <returns>JWT токен или код ошибки</returns>
        [Route("login")] // Дополнительный маршрут: /user/login
        [HttpPost] // Только POST запросы
        public ActionResult Login([FromForm] string login, [FromForm] string password)
        {
            try
            {
                // Ищем пользователя в базе данных по логину и паролю
                // FirstOrDefault() вернет первого найденного пользователя или null
                User? AuthUser = databaseManager.Users
                    .Where(x => x.Login == login && x.Password == password)
                    .FirstOrDefault();

                // Если пользователь не найден (логин/пароль неверные)
                if (AuthUser == null)
                {
                    // Возвращаем 401 Unauthorized (не авторизован)
                    return StatusCode(401);
                }
                else
                {
                    // Генерируем JWT токен для аутентифицированного пользователя
                    string Token = JwtToken.Generate(AuthUser);
                    // Обновляем дату последней авторизации
                    AuthUser.LastAuth = DateTime.Now;
                    // Сохраняем изменения в базе данных
                    databaseManager.SaveChanges();
                    // Возвращаем успешный ответ с токеном
                    return Ok(new { token = Token });
                }
            }
            catch (Exception exp)
            {
                // В случае любой ошибки возвращаем 501 Not Implemented
                // с текстом ошибки
                return StatusCode(501, exp.Message);
            }
        }
    }
}
