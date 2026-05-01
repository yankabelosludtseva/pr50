using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using pr50_2.Models;

namespace pr50_2.Context
{
    /// <summary> Контекст для работы с API хранилищ (Storage) Содержит методы для С ...
    public class StorageContext
    {
        /// <summary> Базовый URL API для операций с хранилищами</summary>
        static string url = "https://localhost:7291/storage/";

        /// <summary>
        /// Получение всех записей хранилища для текущего пользователя
        /// GET /storage/get
        /// </summary>
        /// <returns>Список записей или null при ошибке</returns>
        public static async Task<List<Storage>> Get()
        {
            // Создаем HTTP клиент
            using (HttpClient Client = new HttpClient())
            {
                // Создаем GET запрос
                using (HttpRequestMessage Request = new HttpRequestMessage(HttpMethod.Get, url + "get"))
                {
                    // Добавляем токен авторизации в заголовок
                    Request.Headers.Add("token", MainWindow.Token);

                    // Отправляем запрос
                    var Response = await Client.SendAsync(Request);

                    // Если сервер вернул 200 OK
                    if (Response.StatusCode == HttpStatusCode.OK)
                    {
                        // Читаем JSON ответ
                        string sResponse = await Response.Content.ReadAsStringAsync();
                        // Десериализуем JSON в список объектов Storage
                        List<Storage> Storages = JsonConvert.DeserializeObject<List<Storage>>(sResponse);
                        return Storages;
                    }
                }
            }
            // Возвращаем null при ошибке
            return null;
        }

        /// <summary>
        /// Добавление новой записи в хранилище
        /// POST /storage/add
        /// </summary>
        /// <param name="storage">Объект для добавления (без ID)</param>
        /// <returns>Созданный объект с присвоенным ID или null при ошибке</returns>
        public static async Task<Storage> Add(Storage storage)
        {
            using (HttpClient Client = new HttpClient())
            {
                using (HttpRequestMessage Request = new HttpRequestMessage(HttpMethod.Post, url + "add"))
                {
                    // Добавляем токен в заголовок
                    Request.Headers.Add("token", MainWindow.Token);

                    // Сериализуем объект Storage в JSON
                    string JsonStorage = JsonConvert.SerializeObject(storage);

                    // Создаем контент с JSON (указываем кодировку и тип контента)
                    var Content = new StringContent(JsonStorage, Encoding.UTF8, "application/json");
                    Request.Content = Content;

                    // Отправляем запрос
                    var Response = await Client.SendAsync(Request);

                    // Если успешно
                    if (Response.StatusCode == HttpStatusCode.OK)
                    {
                        string sResponse = await Response.Content.ReadAsStringAsync();
                        // Десериализуем ответ (сервер возвращает созданный объект с ID)
                        Storage Storage = JsonConvert.DeserializeObject<Storage>(sResponse);
                        return Storage;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Обновление существующей записи
        /// PUT /storage/update
        /// </summary>
        /// <param name="storage">Объект с обновленными данными (должен содержать ID)</param>
        /// <returns>Обновленный объект или null при ошибке</returns>
        public static async Task<Storage> Update(Storage storage)
        {
            using (HttpClient Client = new HttpClient())
            {
                using (HttpRequestMessage Request = new HttpRequestMessage(HttpMethod.Put, url + "update"))
                {
                    // Добавляем токен в заголовок
                    Request.Headers.Add("token", MainWindow.Token);

                    // Сериализуем объект в JSON
                    string JsonStorage = JsonConvert.SerializeObject(storage);
                    var Content = new StringContent(JsonStorage, Encoding.UTF8, "application/json");
                    Request.Content = Content;

                    // Отправляем запрос
                    var Response = await Client.SendAsync(Request);

                    // Если успешно
                    if (Response.StatusCode == HttpStatusCode.OK)
                    {
                        string sResponse = await Response.Content.ReadAsStringAsync();
                        // Десериализуем ответ
                        Storage Storage = JsonConvert.DeserializeObject<Storage>(sResponse);
                        return Storage;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Удаление записи по ID
        /// DELETE /storage/delete
        /// </summary>
        /// <param name="id">ID удаляемой записи</param>
        public static async Task Delete(int id)
        {
            using (HttpClient Client = new HttpClient())
            {
                using (HttpRequestMessage Request = new HttpRequestMessage(HttpMethod.Delete, url + "delete"))
                {
                    // Добавляем токен в заголовок
                    Request.Headers.Add("token", MainWindow.Token);

                    // Для DELETE запроса данные отправляются как form-data
                    Dictionary<string, string> FormData = new Dictionary<string, string>
                    {
                        ["id"] = id.ToString() // ID записи для удаления
                    };

                    // Создаем контент как форму
                    FormUrlEncodedContent Content = new FormUrlEncodedContent(FormData);
                    Request.Content = Content;

                    // Отправляем запрос
                    var Response = await Client.SendAsync(Request);

                    // Если успешно (можно проверить статус)
                    if (Response.StatusCode == HttpStatusCode.OK)
                    {
                        // Читаем ответ (обычно пустой или подтверждение)
                        string sResponse = await Response.Content.ReadAsStringAsync();
                        // Можно залогировать или проигнорировать
                    }
                }
            }
        }
    }
}
