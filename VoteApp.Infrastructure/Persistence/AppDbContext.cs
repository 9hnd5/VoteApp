using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using VoteApp.Application.Commons.Interfaces;
using VoteApp.Domain.Entities;

namespace VoteApp.Infrastructure.Persistence
{
    public class AppDbContext : DbContext, IAppDbContext
    {
        public AppDbContext()
        {

        }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            Initial();
        }

        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserVoteDetail> UserVoteDetails { get; set; }

        public void Initial()
        {
            if (Users.Count() == 0)
            {
                Users.Add(
                    new User()
                    {
                        Id = 1,
                        Email = "test@gmail.com",
                        Password = "5kOIeXHcGKSjzvjntN7lbJDlMkXaf+onA0sCyvhKN4s="
                    }
                );
            }
            if (Items.Count() == 0)
            {
                for (var i = 1; i <= 500; i++)
                {
                    Items.Add(new Item()
                    {
                        Id = i,
                        CreateBy = 1,
                        CreatedDate = DateTime.Now.Date,
                        Content = $"Content Test {i}",
                        Name = $"Item Test {i}",
                        Vote = 0
                    });
                }
            }
            base.SaveChanges();
        }
    }
}
