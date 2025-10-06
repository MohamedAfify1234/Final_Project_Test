using Educational_Platform.AppSettingsImages;
using Educational_Platform.DAL.Data;
using Educational_Platform.DAL.Repositorys.Implementations;
using Educational_Platform.DAL.Repositorys.Interfaces;
using Educational_Platform.Mappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NuGet.Protocol.Core.Types;

namespace Educational_Platform
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

			builder.Services.AddDbContext<AppDbContext>(options =>
	        options.UseSqlServer(builder.Configuration.GetConnectionString("MyDb")));

			builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

			builder.Services.AddScoped<DeleteImage>();
			builder.Services.AddScoped<SaveImage>();

		    builder.Services.AddAutoMapper(config => 
            { config.AddProfile<MappingProfile>(); });


			// Add services to the container.
			builder.Services.AddControllersWithViews();

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

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
