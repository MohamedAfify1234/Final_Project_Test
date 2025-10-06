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
	public class SubCategoryConfiguration : IEntityTypeConfiguration<SubCategory>
	{
		public void Configure(EntityTypeBuilder<SubCategory> builder)
		{ 
			// العلاقات
			builder.HasMany(sc => sc.Courses)
				.WithOne(c => c.SubCategory)
				.HasForeignKey(c => c.SubCategoryId)
				.OnDelete(DeleteBehavior.Restrict);
		}
	}

}
