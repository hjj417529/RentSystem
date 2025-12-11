using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
string connStr = "server=localhost;database=mysqldb;user=root;password=root;";
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connStr, ServerVersion.AutoDetect(connStr))
);

using var conn = new MySqlConnection(connStr);
conn.Open();

builder.Services.AddRazorPages();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}



app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.Use(
    async (context, next) =>
    {
        string? userId = context.Session.GetString("UserId");
        var path = context.Request.Path;
        var allowPaths = new[] { "/", "/Account", "/Home"};
        bool isAllowed = allowPaths.Any(p => path.StartsWithSegments(p));
        if (!isAllowed && userId == null)
        {
            context.Response.Redirect("/Account/Login");
            return;
        }
        await next();
    }
);
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
