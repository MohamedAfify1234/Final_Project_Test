﻿using Educational_Platform.DAL.Entities.Learning;
using Educational_Platform.DAL.Entities.Reviews;
using Educational_Platform.DAL.Entities.Subscriptions;
using Educational_Platform.DAL.Enums;
using Microsoft.AspNetCore.Identity;

namespace Educational_Platform.DAL.Entities.Users
{
	public class User : IdentityUser<Guid>
	{ 
		// البيانات الأساسية
		public string FullName { get; set; }              // الاسم  
 		public string ProfilePicture { get; set; }         // صورة البروفايل
		public DateTime RegistrationDate { get; set; } = DateTime.Now; // تاريخ التسجيل
 		public DateTime? LastLoginDate { get; set; } = DateTime.Now;    // آخر تسجيل دخول
		public DateTime? LastProfileUpdate { get; set; }   // آخر تعديل على بيانات الحساب

		public bool IsActive { get; set; } = true;         // حالة تفعيل الحساب
 		public bool CanViewPaidCourses { get; set; } = false;   // هل يمكنه رؤية الكورسات المدفوعة؟ (بعد الاشتراك)
 		public UserType UserType { get; set; }             // نوع المستخدم (طالب، مدرس، أدمن)


		// العلاقات العامة
		public ICollection<CourseReview>? CourseReviews { get; set; } // التقييمات
		public ICollection<Question>? Questions { get; set; }         // الأسئلة
		public ICollection<Answer>? Answers { get; set; }             // الإجابات
		public ICollection<SubscriptionPlan>? Subscribes { get; set; }       // الاشتراكات\
	

	}

}
