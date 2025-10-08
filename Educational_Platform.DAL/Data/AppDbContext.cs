using Educational_Platform.DAL.Entities.Courses;
using Educational_Platform.DAL.Entities.Exams;
using Educational_Platform.DAL.Entities.Learning;
using Educational_Platform.DAL.Entities.Lessons;
using Educational_Platform.DAL.Entities.Reviews;
using Educational_Platform.DAL.Entities.Subscriptions;
using Educational_Platform.DAL.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Educational_Platform.DAL.Data
{
	public class AppDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
	{
		public AppDbContext(DbContextOptions<AppDbContext> options)
		 : base(options)
		{
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



		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// Apply all configurations automatically

			modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
		}

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
