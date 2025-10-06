using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Educational_Platform.DAL.Entities.Lessons;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Educational_Platform.DAL.Configurations.Lessons
{
	public class CourseLessonConfiguration : IEntityTypeConfiguration<CourseLesson>
	{
		public void Configure(EntityTypeBuilder<CourseLesson> builder)
		{ 
			// العلاقات
			builder.HasOne(cl => cl.Course)
				.WithMany(c => c.CourseLessons)
				.HasForeignKey(cl => cl.CourseId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(cl => cl.Lesson)
				.WithMany(l => l.CourseLessons)
				.HasForeignKey(cl => cl.LessonId)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
