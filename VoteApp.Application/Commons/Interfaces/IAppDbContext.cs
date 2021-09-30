using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using VoteApp.Domain.Entities;

namespace VoteApp.Application.Commons.Interfaces
{
    public interface IAppDbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<UserVoteDetail> UserVoteDetails { get; set; }

        public int SaveChanges();
        public EntityEntry Entry(object entity);
        public DatabaseFacade Database { get; }
        public EntityEntry Remove(object entity);
    }
}
