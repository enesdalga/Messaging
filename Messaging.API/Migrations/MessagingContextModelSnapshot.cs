﻿// <auto-generated />
using System;
using Messaging.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Messaging.API.Migrations
{
    [DbContext(typeof(MessagingContext))]
    partial class MessagingContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846");

            modelBuilder.Entity("Messaging.API.Models.BlockList", b =>
                {
                    b.Property<int>("BlockListId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("BlockedUserId");

                    b.Property<int>("BlockingUserId");

                    b.HasKey("BlockListId");

                    b.HasIndex("BlockedUserId");

                    b.HasIndex("BlockingUserId");

                    b.ToTable("BlockLists");
                });

            modelBuilder.Entity("Messaging.API.Models.Message", b =>
                {
                    b.Property<int>("MessageId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Content");

                    b.Property<DateTime?>("Date");

                    b.Property<bool>("IsRead");

                    b.Property<int>("ReceiverId");

                    b.Property<int>("SenderId");

                    b.HasKey("MessageId");

                    b.HasIndex("ReceiverId");

                    b.HasIndex("SenderId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("Messaging.API.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd();

                    b.Property<byte[]>("PasswordHash");

                    b.Property<byte[]>("PasswordSalt");

                    b.Property<string>("Username");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Messaging.API.Models.BlockList", b =>
                {
                    b.HasOne("Messaging.API.Models.User", "BlockedUser")
                        .WithMany("BlockedList")
                        .HasForeignKey("BlockedUserId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Messaging.API.Models.User", "BlockingUser")
                        .WithMany("BlockingList")
                        .HasForeignKey("BlockingUserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Messaging.API.Models.Message", b =>
                {
                    b.HasOne("Messaging.API.Models.User", "Receiver")
                        .WithMany("MessagesReceived")
                        .HasForeignKey("ReceiverId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Messaging.API.Models.User", "Sender")
                        .WithMany("MessagesSent")
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}
