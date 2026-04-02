// Создаем билдера веб-приложения с настройками по умолчанию
var builder = WebApplication.CreateBuilder(args);
// Добавляем поддержку MVC (Model-View-Controller)
builder.Services.AddMvc(option => option.EnableEndpointRouting = true);
// Добавляем поддержку Swagger для генерации документации API
builder.Services.AddSwaggerGen(option => {
    // Создаем новую версию документации Swagger
    option.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "v1",      // Версия API
        Title = "Инструментарий"  // Название API в документации
    });
    // Получаем путь к XML-файлу с комментариями из кода
    // AppContext.BaseDirectory - директория, где запущено приложение
    string PathFile = Path.Combine(AppContext.BaseDirectory, "pr50.xml");
    // Добавляем XML-комментарии в Swagger для лучшей документации
    option.IncludeXmlComments(PathFile);
});
// Строим приложение после настройки всех сервисов
var app = builder.Build();
// Включаем middleware для генерации Swagger JSON
app.UseSwagger();
// Включаем маршрутизацию запросов
app.UseRouting();
// Настраиваем конечные точки (endpoints) приложения
app.UseEndpoints(endpoints =>
{
    // Подключаем все контроллеры как конечные точки
    endpoints.MapControllers();
});
// Настраиваем пользовательский интерфейс Swagger UI
app.UseSwaggerUI(c => {
    // Указываем путь к JSON-файлу спецификации Swagger
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Инструментарий");
});
// Запускаем приложение и начинаем обрабатывать запросы
app.Run();