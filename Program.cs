using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PriceMonitoringService.Data;
using PriceMonitoringService.Service;

var builder = WebApplication.CreateBuilder(args);

// ����������� DbContext ��� SQLite
builder.Services.AddDbContext<PriceMonitorContext>(options =>
    options.UseSqlite("Data Source=pricemonitor.db"));

// ����������� ������������
builder.Services.AddControllers();

// ����������� �������� ������� ��� ���������� ����
builder.Services.AddHostedService<PriceUpdateService>();

var app = builder.Build();

// ���������� �������� �� (�����������, ��� ��������������� �������� ��)
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<PriceMonitorContext>();
    dbContext.Database.Migrate();
}

app.MapControllers();

app.Run();
