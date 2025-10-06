using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Educational_Platform.DAL.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Educational_Platform.DAL.Configurations.Users
{
	public class UserConfiguration : IEntityTypeConfiguration<User>
	{
		public void Configure(EntityTypeBuilder<User> builder)
		{
		 
			// العلاقات
			builder.HasMany(u => u.CourseReviews)
				.WithOne(cr => cr.User)
				.HasForeignKey(cr => cr.UserId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(u => u.Questions)
				.WithOne(q => q.User)
				.HasForeignKey(q => q.UserId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(u => u.Answers)
				.WithOne(a => a.User)
				.HasForeignKey(a => a.UserId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(u => u.Subscribes)
				.WithOne(s => s.User)
				.HasForeignKey(s => s.UserId)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}

}
