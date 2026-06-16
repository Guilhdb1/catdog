using Microsoft.EntityFrameworkCore;
using CatDog.Api.Modules.Authentication.Entities;

namespace CatDog.Api.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<ConfirmationToken> ConfirmationTokens => Set<ConfirmationToken>();
    public DbSet<PasswordResetToken> PasswordResetTokens => Set<PasswordResetToken>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(builder =>
        {
            builder.ToTable("users");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Nome).HasMaxLength(180).IsRequired();
            builder.Property(x => x.Email).HasMaxLength(320).IsRequired();
            builder.HasIndex(x => x.Email).IsUnique();
            builder.Property(x => x.PasswordHash).IsRequired();
            builder.Property(x => x.EmailConfirmed).HasDefaultValue(false);
            builder.Property(x => x.Role).HasConversion<string>().HasMaxLength(20).IsRequired();
            builder.Property(x => x.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.Property(x => x.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.Property(x => x.FailedLoginAttempts).HasDefaultValue(0);
        });

        modelBuilder.Entity<RefreshToken>(builder =>
        {
            builder.ToTable("refresh_tokens");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.TokenHash).HasMaxLength(512).IsRequired();
            builder.Property(x => x.IssuedAt).IsRequired();
            builder.Property(x => x.ExpiresAt).IsRequired();
            builder.Property(x => x.Revoked).HasDefaultValue(false);
            builder.HasOne(x => x.User)
                .WithMany(x => x.RefreshTokens)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ConfirmationToken>(builder =>
        {
            builder.ToTable("confirmation_tokens");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.TokenHash).HasMaxLength(512).IsRequired();
            builder.Property(x => x.ExpiresAt).IsRequired();
            builder.Property(x => x.UsedAt);
            builder.HasOne(x => x.User)
                .WithMany(x => x.ConfirmationTokens)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<PasswordResetToken>(builder =>
        {
            builder.ToTable("password_reset_tokens");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.TokenHash).HasMaxLength(512).IsRequired();
            builder.Property(x => x.ExpiresAt).IsRequired();
            builder.Property(x => x.UsedAt);
            builder.HasOne(x => x.User)
                .WithMany(x => x.PasswordResetTokens)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
