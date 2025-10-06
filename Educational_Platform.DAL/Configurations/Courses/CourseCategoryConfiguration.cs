using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Educational_Platform.DAL.Entities.Courses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Educational_Platform.DAL.Configurations.Courses
{
	public class CourseCategoryConfiguration : IEntityTypeConfiguration<CourseCategory>
	{
		public void Configure(EntityTypeBuilder<CourseCategory> builder)
		{
			// العلاقات
			builder.HasMany(cc => cc.SubCategories)
				.WithOne(sc => sc.Category)
				.HasForeignKey(sc => sc.CategoryId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(cc => cc.Courses)
				.WithOne(c => c.Category)
				.HasForeignKey(c => c.CategoryId)
				.OnDelete(DeleteBehavior.Restrict);
		}
	}
}
