using Microsoft.EntityFrameworkCore;
using NewECO5.Models;
using NewECO5.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure PostgreSQL
builder.Services.AddDbContext<NewECO5DBContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// 註冊自訂 Service（加這幾行 ↓↓↓）
builder.Services.AddScoped<Comm_UnitSettingService>();
builder.Services.AddScoped<MeterService>(); // 若 MeterController 用的是類別 MeterService
builder.Services.AddScoped<IMeterService, MeterService>();
builder.Services.AddScoped<PowerMeterReaderService>();
// 如果你是用介面方式注入，可改成這樣：
// builder.Services.AddScoped<IMeterService, MeterService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
