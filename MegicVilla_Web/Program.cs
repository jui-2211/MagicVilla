using MagicVilla_Web.Services;
using MegicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace MegicVilla_Web
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddControllersWithViews();

			builder.Services.AddAutoMapper(typeof(MappingConfig));

			builder.Services.AddHttpClient<IVillaServices, VillaService>();
			builder.Services.AddScoped<IVillaServices, VillaService>();

			builder.Services.AddHttpClient<IVillaNumberServices, VillaNumberService>();
			builder.Services.AddScoped<IVillaNumberServices, VillaNumberService>();

			builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

			builder.Services.AddHttpClient<IAuthServices, AuthService>();
			builder.Services.AddScoped<IAuthServices, AuthService>();

			builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
			  .AddCookie(options =>
			  {
				  options.Cookie.HttpOnly = true;
				  options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
				  options.LoginPath = "/Auth/Login";
				  options.AccessDeniedPath = "/Auth/AccessDenied";
				  options.SlidingExpiration = true;
			  });

			builder.Services.AddSession(options =>
			{
				options.IdleTimeout = TimeSpan.FromMinutes(100);
				options.Cookie.HttpOnly = true;
				options.Cookie.IsEssential = true;
			});

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

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");

			app.Run();
		}
	}
}