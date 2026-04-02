using Microsoft.IdentityModel.Tokens;
using pr50.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace pr50.Classes
{
    /// <summary>
    /// Отвечает за генерацию и валидацию токенов
    /// </summary>
    public class JwtToken
    {
        /// <summary>
        /// Секретный ключ для подписи токенов
        /// static означает, что ключ общий для всех экземпляров класса
        /// </summary>
        static byte[] Key = Encoding.UTF8.GetBytes("PERMAVIAT_THE_BEST!!!!!!!!!!!!!!!");

        /// <summary>
        /// Генерирует JWT токен для пользователя
        /// </summary>
        /// <param name="user">Пользователь, для которого создается токен</param>
        /// <returns>Строка с JWT токеном</returns>
        public static string Generate(User user)
        {
            // Создаем обработчик JWT токенов
            JwtSecurityTokenHandler TokenHandler = new JwtSecurityTokenHandler();
            // Описываем содержимое токена
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                // Добавляем Claims (утверждения) в токен
                // Claim - это пары ключ-значение с информацией о пользователе
                Subject = new ClaimsIdentity(new[] {
                    new Claim("UserId", user.Id.ToString()) // Сохраняем ID пользователя
                }),
                // Время истечения токена (7 дней с момента создания)
                Expires = DateTime.UtcNow.AddDays(7),
                // Подписываем токен нашим секретным ключом
                // Это гарантирует, что токен не был подделан
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Key),          // Используем симметричное шифрование
                    SecurityAlgorithms.HmacSha256Signature  // Алгоритм подписи HMAC-SHA256
                )
            };
            // Создаем токен на основе описания
            SecurityToken Token = TokenHandler.CreateToken(tokenDescriptor);
            // Возвращаем токен в виде строки
            return TokenHandler.WriteToken(Token);
        }

        /// <summary>
        /// Извлекает ID пользователя из JWT токена
        /// </summary>
        /// <param name="token">JWT токен в виде строки</param>
        /// <returns>ID пользователя или null, если токен недействителен</returns>
        public static int? GetUserIdFromToken(string token)
        {
            try
            {
                // Создаем обработчик JWT токенов
                JwtSecurityTokenHandler TokenHandler = new JwtSecurityTokenHandler();
                // Валидируем токен и извлекаем данные
                TokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    // Проверяем, что токен подписан нашим ключом
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Key),
                    // Отключаем проверку издателя и аудитории
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // Убираем временную погрешность при проверке срока действия
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken ValidatedToken);
                // Преобразуем валидированный токен в JwtSecurityToken
                JwtSecurityToken JwtToken = (JwtSecurityToken)ValidatedToken;
                // Извлекаем значение claim с именем "UserId"
                string UserId = JwtToken.Claims.First(x => x.Type == "UserId").Value;
                // Преобразуем строку в число и возвращаем
                return int.Parse(UserId);
            }
            catch
            {
                // Если произошла ошибка (токен недействителен), возвращаем null
                return null;
            }
        }
    }
}
