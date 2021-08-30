using LeftRightNet.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LeftRightNet.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<HeadLine> HeadLines { get; set; }
        public DbSet<NewsSite> NewsSites { get; set; }
        public DbSet<SnapShot> SnapShots { get; set; }
        public DbSet<Config> Configs { get; set; }
        public DbSet<Sentiment> Sentiments { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<SnapShot>()
            .HasOne<NewsSite>(x => x.NewsSite)
            .WithMany(x => x.SnapShots)
            .HasForeignKey(x => x.NewsSiteId)
            .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<SnapShot>().HasIndex(x => x.CreatedAt);

            builder.Entity<HeadLine>()
            .HasOne<SnapShot>(x => x.SnapShot)
            .WithMany(x => x.HeadLines)
            .HasForeignKey(x => x.SnapShotId)
            .OnDelete(DeleteBehavior.Cascade);
            

            builder.Entity<HeadLine>()
                .HasOne(x => x.Sentiment)
                .WithOne(x => x.HeadLine)
                .HasForeignKey<Sentiment>(x => x.HeadLineId);
            
            builder.Entity<NewsSite>().HasIndex(x => x.Name);
            builder.Entity<NewsSite>().HasIndex(x => x.Url);
            builder.Entity<HeadLine>().HasIndex(x => x.CreatedAt);
            builder.Entity<SnapShot>().HasIndex(x => x.CreatedAt);
            builder.Entity<SnapShot>().HasIndex(x => x.ImageHashId);
            builder.Entity<Sentiment>().HasIndex(x => x.pos);
            builder.Entity<Sentiment>().HasIndex(x => x.compound);
            builder.Entity<Sentiment>().HasIndex(x => x.neu);
            builder.Entity<Sentiment>().HasIndex(x => x.neg);
            builder.Entity<Sentiment>().HasIndex(x => x.compound);
        }
    }
}
