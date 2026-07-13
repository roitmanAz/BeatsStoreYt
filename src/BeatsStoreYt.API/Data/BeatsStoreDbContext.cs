using BeatsStoreYt.API.Data.Features.Catalog;
using BeatsStoreYt.API.Data.Features.Commerce.Cart;
using BeatsStoreYt.API.Data.Features.Commerce.Coupons;
using BeatsStoreYt.API.Data.Features.Commerce.Favorites;
using BeatsStoreYt.API.Data.Features.Commerce.Orders;
using BeatsStoreYt.API.Data.Features.Content;
using BeatsStoreYt.API.Data.Features.Marketing;
using BeatsStoreYt.API.Data.Features.Events;
using BeatsStoreYt.API.Data.Features.Analytics;
using BeatsStoreYt.API.Data.Features.Support;
using BeatsStoreYt.API.Data.Features.Security;
using BeatsStoreYt.API.Data.Features.Users;
using Microsoft.EntityFrameworkCore;

namespace BeatsStoreYt.API.Data;

public class BeatsStoreDbContext(DbContextOptions<BeatsStoreDbContext> options) : DbContext(options)
{
    public DbSet<Beat> Beats => Set<Beat>();
    public DbSet<BeatSet> BeatSets => Set<BeatSet>();
    public DbSet<BeatSetItem> BeatSetItems => Set<BeatSetItem>();
    public DbSet<CartItem> CartItems => Set<CartItem>();
    public DbSet<Coupon> Coupons => Set<Coupon>();
    public DbSet<CouponProduct> CouponProducts => Set<CouponProduct>();
    public DbSet<CouponRedemption> CouponRedemptions => Set<CouponRedemption>();
    public DbSet<EventRequest> EventRequests => Set<EventRequest>();
    public DbSet<EventRequestCall> EventRequestCalls => Set<EventRequestCall>();
    public DbSet<EventSummary> EventSummaries => Set<EventSummary>();
    public DbSet<Favorite> Favorites => Set<Favorite>();
    public DbSet<FavoriteItem> FavoriteItems => Set<FavoriteItem>();
    public DbSet<KeyboardModel> KeyboardModels => Set<KeyboardModel>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<PaymentTransaction> PaymentTransactions => Set<PaymentTransaction>();
    public DbSet<PaymentWebhook> PaymentWebhooks => Set<PaymentWebhook>();
    public DbSet<MediaAsset> MediaAssets => Set<MediaAsset>();
    public DbSet<NewsletterSubscription> NewsletterSubscriptions => Set<NewsletterSubscription>();
    public DbSet<EmailCampaign> EmailCampaigns => Set<EmailCampaign>();
    public DbSet<EmailCampaignRecipient> EmailCampaignRecipients => Set<EmailCampaignRecipient>();
    public DbSet<EventAudioPlaylist> EventAudioPlaylists => Set<EventAudioPlaylist>();
    public DbSet<EventAudioPlaylistItem> EventAudioPlaylistItems => Set<EventAudioPlaylistItem>();
    public DbSet<WeddingShowcase> WeddingShowcases => Set<WeddingShowcase>();
    public DbSet<SiteReview> SiteReviews => Set<SiteReview>();
    public DbSet<EventComment> EventComments => Set<EventComment>();
    public DbSet<BeatPlayEvent> BeatPlayEvents => Set<BeatPlayEvent>();
    public DbSet<BeatPlayStatsDaily> BeatPlayStatsDaily => Set<BeatPlayStatsDaily>();
    public DbSet<ShoppingCart> ShoppingCarts => Set<ShoppingCart>();
    public DbSet<Style> Styles => Set<Style>();
    public DbSet<SupportTicket> SupportTickets => Set<SupportTicket>();
    public DbSet<TicketMessage> TicketMessages => Set<TicketMessage>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
    public DbSet<SystemLog> SystemLogs => Set<SystemLog>();
    public DbSet<SecurityEvent> SecurityEvents => Set<SecurityEvent>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BeatsStoreDbContext).Assembly);
    }
}
