using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PriceMonitoringService.Data;
using PriceMonitoringService.Service;

var builder = WebApplication.CreateBuilder(args);

// Регистрация DbContext для SQLite
builder.Services.AddDbContext<PriceMonitorContext>(options =>
    options.UseSqlite("Data Source=pricemonitor.db"));

// Регистрация контроллеров
builder.Services.AddControllers();

// Регистрация фонового сервиса для обновления цены
builder.Services.AddHostedService<PriceUpdateService>();

var app = builder.Build();

// Применение миграций БД (опционально, для автоматического создания БД)
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<PriceMonitorContext>();
    dbContext.Database.Migrate();
}

app.MapControllers();

app.Run();
