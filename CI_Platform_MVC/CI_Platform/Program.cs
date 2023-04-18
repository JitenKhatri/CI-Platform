using CI_Platform;
using CI_Platform.DataAccess;
using CI_Platform.DataAccess.Repository;
using CI_Platform.DataAccess.Repository.IRepository;
using CI_Platform.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
); ;
builder.Services.AddAuthentication("AuthCookie").AddCookie("AuthCookie", options =>
{
    options.ExpireTimeSpan = TimeSpan.FromDays(2);
    options.Cookie.Name = "AuthCookie";
    options.LoginPath = "/Auth/Login";
    options.LogoutPath = "/Auth/LogOut";
});
builder.Services.AddDbContext<CiPlatformContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IAllRepository, AllRepository>();
builder.Services.AddSession();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
//app.MapControllerRoute(
//name: "area",
//pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
name: "default",
pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "UserAuthentication",
    pattern: "{controller=UserAuthentication}/{action=ResetPassword}/{id?}");

app.Run();
