using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Educational_Platform.DAL.Entities.Reviews;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Educational_Platform.DAL.Configurations.Reviews
{
	public class CourseReviewConfiguration : IEntityTypeConfiguration<CourseReview>
	{
		public void Configure(EntityTypeBuilder<CourseReview> builder)
		{ 
			// العلاقات
			builder.HasOne(cr => cr.Course)
				.WithMany(c => c.CourseReviews)
				.HasForeignKey(cr => cr.CourseId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(cr => cr.User)
				.WithMany(u => u.CourseReviews)
				.HasForeignKey(cr => cr.UserId)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
