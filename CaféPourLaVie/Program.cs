using CaféPourLaVie.Data;
using CaféPourLaVie.Services;
using CaféPourLaVie.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Add authentication services
builder.Services.AddAuthentication("CookieAuth")
    .AddCookie("CookieAuth", options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });

// Add authorization services
builder.Services.AddHttpContextAccessor();

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add session services
builder.Services.AddSession();

// Add CartService as a scoped service
builder.Services.AddScoped<CartService>();

// Add OrderService as a scoped service
builder.Services.AddScoped<IOrderService, OrderService>();

// Add ProductService as a scoped service
builder.Services.AddScoped<IDashboardService, DashboardService>();

// Add EmployeeService as a scoped service
builder.Services.AddScoped<IEmployeeService, EmployeeService>();

// Add ImportService as a scoped service
builder.Services.AddScoped<IImportService, ImportService>();

// Add InventoryService as a scoped service
builder.Services.AddScoped<IInventoryService, InventoryService>();

// Add ReportService as a scoped service
builder.Services.AddScoped<IReportService, ReportService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

// Use authentication and authorization middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
