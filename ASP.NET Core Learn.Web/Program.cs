using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MVC_ASP.NET_Core_Learn.Data;
using MVC_ASP.NET_Core_Learn.Data.Interfaces;
using MVC_ASP.NET_Core_Learn.Models;
using MVC_ASP.NET_Core_Learn.Repository;
using MVC_ASP.NET_Core_Learn.Services;
using System.Text;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IDepositRepository, DepositTemplateRepository>();
builder.Services.AddScoped<IUserDepositRepository, UserDepositRepository>();
builder.Services.AddHttpClient<ApiService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7130");
});


builder.Services.AddDbContext<AppDbContext>(options =>
{
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddIdentity<AppUser, IdentityRole>()
	.AddEntityFrameworkStores<AppDbContext>();
builder.Services.AddMemoryCache();
builder.Services.AddSession();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login"; // Здесь укажите путь к вашей странице входа
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    var jwtOptions = builder.Configuration.GetSection("JwtOptions");
    var secretKey = jwtOptions.GetValue<string>("SecretKey");
    var issuer = jwtOptions.GetValue<string>("Issuer");
    var audience = jwtOptions.GetValue<string>("Audience");

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            // Получить токен из куки
            var token = context.Request.Cookies["access_cookie"];

            // Если токен отсутствует, вернуть 401 ошибку
            //if (string.IsNullOrEmpty(token))
            //{
            //    context.Response.StatusCode = 401;
            //    context.Response.ContentType = "application/json";
            //    var message = Encoding.UTF8.GetBytes("{\"error\": \"Unauthorized. Token is missing.\"}");
            //    context.Response.Body.WriteAsync(message, 0, message.Length);
            //    return Task.CompletedTask;
            //}

            // Установить токен для дальнейшей обработки
            context.Token = token;

            return Task.CompletedTask;
        },
        OnAuthenticationFailed = context =>
        {
            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
            {
                // Токен истек, перенаправляем пользователя на страницу входа
                context.Response.Redirect("/Account/Login");
            }
            else
            {
                // Другие случаи ошибок аутентификации можно обработать здесь
                // Например, когда токен недействителен или отсутствует
                context.Response.StatusCode = 401; // Неавторизованный доступ
            }

            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

// Filling the database with testing data
//if (args.Length == 1 && args[0].ToLower() == "seeddata")
//{
	//await Seed.SeedUsersAndRolesAsync(app);
	//Seed.Initialize(app);
//}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.Use(async (context, next) =>
{
    await next();
    if (context.Response.StatusCode == 404)
	{
		context.Request.Path = "/NotFound";
		await next();
	}
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
