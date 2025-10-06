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
	public class LessonConfiguration : IEntityTypeConfiguration<Lesson>
	{
		public void Configure(EntityTypeBuilder<Lesson> builder)
		{ 
			// العلاقات
			builder.HasMany(l => l.CourseLessons)
				.WithOne(cl => cl.Lesson)
				.HasForeignKey(cl => cl.LessonId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(l => l.Questions)
				.WithOne(q => q.Lesson)
				.HasForeignKey(q => q.LessonId)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
