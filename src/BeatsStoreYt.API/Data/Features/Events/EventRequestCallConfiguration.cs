using BeatsStoreYt.API.Data.Features.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeatsStoreYt.API.Data.Features.Events;

public class EventRequestCallConfiguration : IEntityTypeConfiguration<EventRequestCall>
{
    public void Configure(EntityTypeBuilder<EventRequestCall> builder)
    {
        builder.ToTable("EventRequestCalls");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.CallAt)
            .IsRequired();

        builder.Property(c => c.CallResult)
            .IsRequired();

        builder.Property(c => c.Summary)
            .HasMaxLength(2000);

        // Many calls belong to one event request.
        builder.HasOne(c => c.EventRequest)
            .WithMany(e => e.Calls)
            .HasForeignKey(c => c.EventRequestId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        // Optional admin link allows tracking which staff member made the call.
        builder.HasOne(c => c.AdminUser)
            .WithMany()
            .HasForeignKey(c => c.AdminUserId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(c => c.EventRequestId);
        builder.HasIndex(c => c.CallAt);
        builder.HasIndex(c => c.CallResult);
    }
}
