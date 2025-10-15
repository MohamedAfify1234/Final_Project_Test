using Core.Interfaces;
using Core.Interfaces.Courses;
using Core.Interfaces.Reviews;
using Infrastructure.Data;
using Infrastructure.Repositories.Courses;
using Infrastructure.Repositories.Implementation;
using Core.Interfaces;
using Infrastructure.Repositories.Reviews;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Skillup_Academy.AppSettingsImages;
using Skillup_Academy.Mappings;

namespace Skillup_Academy
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<ICourseCategoryRepsitory, CourseCategoryRepository>();
            builder.Services.AddScoped<ISubCategoryRepository, SubCategoryRepository>();
            builder.Services.AddScoped<ICourseReviewRepository, CourseReviewRepository>();
            builder.Services.AddDbContext<AppDbContext>(Options =>
            { Options.UseSqlServer(builder.Configuration.GetConnectionString("cs"));
            });
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped<DeleteImage>();
            builder.Services.AddScoped<SaveImage>();

            builder.Services.AddAutoMapper(config =>
            { config.AddProfile<MappingProfile>(); });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
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
