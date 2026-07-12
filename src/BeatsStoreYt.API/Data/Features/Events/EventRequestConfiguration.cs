using BeatsStoreYt.API.Data.Features.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeatsStoreYt.API.Data.Features.Events;

public class EventRequestConfiguration : IEntityTypeConfiguration<EventRequest>
{
    public void Configure(EntityTypeBuilder<EventRequest> builder)
    {
        builder.ToTable("EventRequests");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.FullName)
            .IsRequired()
            .HasMaxLength(120);

        builder.Property(e => e.PhoneNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(e => e.Email)
            .HasMaxLength(256);

        builder.Property(e => e.EventType)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.City)
            .HasMaxLength(100);

        builder.Property(e => e.Notes)
            .HasMaxLength(2000);

        builder.Property(e => e.Status)
            .IsRequired();

        builder.Property(e => e.CreatedAt)
            .IsRequired();

        builder.Property(e => e.UpdatedAt)
            .IsRequired();

        // Optional user link allows logged-in customers to submit event forms.
        builder.HasOne(e => e.User)
            .WithMany()
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.SetNull);

        // One event request can later have many phone calls or follow-ups.
        builder.HasMany(e => e.Calls)
            .WithOne(c => c.EventRequest)
            .HasForeignKey(c => c.EventRequestId)
            .OnDelete(DeleteBehavior.Cascade);

        // One event request can later have one summary record.
        builder.HasOne(e => e.Summary)
            .WithOne(s => s.EventRequest)
            .HasForeignKey<EventSummary>(s => s.EventRequestId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => e.Status);
        builder.HasIndex(e => e.CreatedAt);
        builder.HasIndex(e => e.PhoneNumber);
    }
}
