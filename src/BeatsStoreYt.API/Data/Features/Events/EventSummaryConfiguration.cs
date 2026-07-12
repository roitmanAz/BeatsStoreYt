using BeatsStoreYt.API.Data.Features.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeatsStoreYt.API.Data.Features.Events;

public class EventSummaryConfiguration : IEntityTypeConfiguration<EventSummary>
{
    public void Configure(EntityTypeBuilder<EventSummary> builder)
    {
        builder.ToTable("EventSummaries");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.ConfirmedAt)
            .IsRequired(false);

        builder.Property(s => s.FinalStatus)
            .IsRequired();

        builder.Property(s => s.AgreedPrice)
            .HasPrecision(18, 2)
            .IsRequired(false);

        builder.Property(s => s.SummaryNotes)
            .HasMaxLength(4000);

        builder.Property(s => s.CreatedAt)
            .IsRequired();

        builder.Property(s => s.UpdatedAt)
            .IsRequired();

        // One summary belongs to one event request.
        builder.HasOne(s => s.EventRequest)
            .WithOne(e => e.Summary)
            .HasForeignKey<EventSummary>(s => s.EventRequestId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        // Optional admin link records who finalized the request.
        builder.HasOne(s => s.ConfirmedByAdmin)
            .WithMany()
            .HasForeignKey(s => s.ConfirmedByAdminId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(s => s.FinalStatus);
        builder.HasIndex(s => s.ConfirmedAt);
    }
}
