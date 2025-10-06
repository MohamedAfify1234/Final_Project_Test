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
	public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
	{
		public void Configure(EntityTypeBuilder<Teacher> builder)
		{
			// العلاقات
			builder.HasMany(t => t.Courses)
				.WithOne(c => c.Teacher)
				.HasForeignKey(c => c.TeacherId)
				.OnDelete(DeleteBehavior.Restrict);
		}
	}
}
