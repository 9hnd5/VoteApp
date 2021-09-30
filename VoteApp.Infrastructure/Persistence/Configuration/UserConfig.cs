using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VoteApp.Domain.Entities;

namespace VoteApp.Infrastructure.Persistence.Configuration
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(e => e.Name).HasMaxLength(50);
            builder.Property(e => e.Email).HasMaxLength(100);
            builder.Property(e => e.Password).HasMaxLength(100);

            builder.HasMany(x => x.Items)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.CreateBy)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasMany(x => x.UserVoteDetails)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
