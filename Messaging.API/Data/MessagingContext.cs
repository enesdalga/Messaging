using Messaging.API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Messaging.API.Data
{
    public class MessagingContext : DbContext
    {
        public MessagingContext(DbContextOptions<MessagingContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<BlockList> BlockLists { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<Message>()
            .HasOne(u => u.Sender)
            .WithMany(m => m.MessagesSent)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Message>()
            .HasOne(u => u.Receiver)
            .WithMany(m => m.MessagesReceived)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<BlockList>()
            .HasOne(u => u.BlockingUser)
            .WithMany(b => b.BlockingList)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<BlockList>()
            .HasOne(u => u.BlockedUser)
            .WithMany(b => b.BlockedList)
            .OnDelete(DeleteBehavior.Restrict);
        }
    }
}