﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Educational_Platform.DAL.Entities.Exams;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Educational_Platform.DAL.Configurations.Exams
{
	public class ExamAttemptConfiguration : IEntityTypeConfiguration<ExamAttempt>
	{
		public void Configure(EntityTypeBuilder<ExamAttempt> builder)
		{ 
			// العلاقات
			builder.HasOne(ea => ea.Exam)
				.WithMany(e => e.ExamAttempts)
				.HasForeignKey(ea => ea.ExamId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(ea => ea.Student)
				.WithMany(s => s.ExamAttempt)
				.HasForeignKey(ea => ea.StudentId)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
