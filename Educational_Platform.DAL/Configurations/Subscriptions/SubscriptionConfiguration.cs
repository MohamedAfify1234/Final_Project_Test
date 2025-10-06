using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Educational_Platform.DAL.Entities.Subscriptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Educational_Platform.DAL.Configurations.Subscriptions
{
	public class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
	{
		public void Configure(EntityTypeBuilder<Subscription> builder)
		{
			// العلاقات
			builder.HasMany(s => s.Subscribes)
				.WithOne(s => s.Subscription)
				.HasForeignKey(s => s.SubscriptionId)
				.OnDelete(DeleteBehavior.Restrict);
		}
	}
}
