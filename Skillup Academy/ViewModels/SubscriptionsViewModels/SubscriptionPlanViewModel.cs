using Core.Models.Courses;
using Core.Models.Subscriptions;
using Core.Models.Users;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Skillup_Academy.ViewModels.SubscriptionsViewModels
{
    public class SubscriptionPlanViewModel
    {
        public Guid Id { get; set; }                       // المعرف الفريد للاشتراك

        // حالة الاشتراك
        public DateTime StartDate { get; set; }            // تاريخ البدء
        public DateTime EndDate { get; set; }              // تاريخ الانتهاء
        public bool IsActive { get; set; } = true;         // نشط/منتهي

        // الدفع
        public decimal PaidAmount { get; set; }            // المبلغ المدفوع
        public string TransactionId { get; set; }          // رقم المعاملة


        // العلاقات
        public Guid UserId { get; set; }                 // المستخدم
        public SelectList Users { get; set; }                     // المستخدم
        public Guid SubscriptionId { get; set; }           // الخطة
        public SelectList Subscriptions { get; set; }     // الخطة
        public Guid? CourseId { get; set; }                // الكورس (لو كان اشتراك في كورس محدد)
        public SelectList Courses { get; set; }                 // الكورس
    }
}
