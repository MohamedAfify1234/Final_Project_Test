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
			builder.HasOne(q => q.Course)
				.WithMany(e => e.Lessons)
				.HasForeignKey(q => q.CourseId)
				.OnDelete(DeleteBehavior.NoAction);
			 
		}
	}
}
