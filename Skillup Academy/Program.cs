using Core.Interfaces;
using Core.Interfaces.Courses;
using Core.Interfaces.Exams;
using Core.Interfaces.Reviews;
using Core.Interfaces.Subscriptions;
using Core.Interfaces.Users;
using Core.Models.Users;
using Infrastructure.Data;
using Infrastructure.Data.Seed;
using Infrastructure.Repositories.Courses;
using Infrastructure.Repositories.Exams;
using Infrastructure.Repositories.Implementation;
using Infrastructure.Repositories.Reviews;
using Infrastructure.Repositories.Subscriptions;
using Infrastructure.Repositories.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Skillup_Academy.AppSettingsImages;
using Skillup_Academy.Helper;
using Skillup_Academy.Mappings;

namespace Skillup_Academy
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<ICourseCategoryRepsitory, CourseCategoryRepository>();
            builder.Services.AddScoped<ISubCategoryRepository, SubCategoryRepository>();
            builder.Services.AddScoped<ICourseReviewRepository, CourseReviewRepository>();
            builder.Services.AddScoped<IExamAttemptRepository, ExamAttemptRepository>();
            builder.Services.AddScoped<ISubscriptionRepository,SubscriptionRepository>();
            builder.Services.AddScoped<ISubscriptionPlanRepository,SubscriptionPlanRepository>();
            builder.Services.AddScoped<ITeacherRepository, TeacherRepository>();

            builder.Services.AddDbContext<AppDbContext>(Options =>
            { Options.UseSqlServer(builder.Configuration.GetConnectionString("cs"));
            });
              
			builder.Services.AddIdentity<User, Role>(options =>
			{
				options.Password.RequireNonAlphanumeric = false;
				options.Password.RequireUppercase = false;
			})
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

			builder.Services.AddScoped<DbInitializer>();


			builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped<DeleteImage>();
            builder.Services.AddScoped<SaveImage>();
            builder.Services.AddScoped<FileService>();

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

			using (var scope = app.Services.CreateScope())
			{
				var services = scope.ServiceProvider;
				var adminSeeder = services.GetRequiredService<DbInitializer>();
				await adminSeeder.SeedAdminAsync();
			}


			app.Run();
        }
    }
}
