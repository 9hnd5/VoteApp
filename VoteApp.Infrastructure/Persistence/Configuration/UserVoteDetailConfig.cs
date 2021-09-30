using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VoteApp.Domain.Entities;

namespace VoteApp.Infrastructure.Persistence.Configuration
{
    public class UserVoteDetailConfig : IEntityTypeConfiguration<UserVoteDetail>
    {
        public void Configure(EntityTypeBuilder<UserVoteDetail> builder)
        {
        }
    }
}
