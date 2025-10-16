using Core.Models.Courses;
using Core.Models.Exams;
using Core.Models.Learning;
using Core.Models.Lessons;
using Core.Models.Reviews;
using Core.Models.Subscriptions;
using Core.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext()
        {
            
        }
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=EducationalPlatform;Integrated Security=True;Encrypt=False;Trust Server Certificate=True");
        //}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Apply all configurations automatically

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }

        // DbSets
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseCategory> CourseCategories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<ExamAttempt> ExamAttempts { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<SubscriptionPlan> SubscriptionPlans { get; set; }
        public DbSet<CourseReview> CourseReviews { get; set; }




        // تطبيق جميع الـ Configurations تلقائياً من الـ Assembly
        //builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());



        //protected override void OnModelCreating(ModelBuilder builder)
        //{
        //	base.OnModelCreating(builder);

        //	// تكوين TPH للوراثة
        //	builder.Entity<User>()
        //		.HasDiscriminator<string>("UserType")
        //		.HasValue<User>("User")
        //		.HasValue<Student>("Student")
        //		.HasValue<Teacher>("Teacher")
        //		.HasValue<Admin>("Admin");

        //	// التكوينات الإضافية
        //	builder.Entity<CourseReview>()
        //		.HasIndex(cr => new { cr.CourseId, cr.UserId })
        //		.IsUnique();

        //	builder.Entity<Subscribe>()
        //		.HasIndex(s => new { s.UserId, s.CourseId, s.SubscriptionId })
        //		.IsUnique();

        //	builder.Entity<CourseLesson>()
        //		.HasIndex(cl => new { cl.CourseId, cl.LessonId })
        //		.IsUnique();

        //	// المزيد من التكوينات...
        //}



    }
}
