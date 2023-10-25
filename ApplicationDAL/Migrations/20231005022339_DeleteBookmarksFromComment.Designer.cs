﻿// <auto-generated />
using System;
using System.Collections.Generic;
using ApplicationDAL.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ApplicationDAL.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20231005022339_DeleteBookmarksFromComment")]
    partial class DeleteBookmarksFromComment
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ApplicationDAL.Entities.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("CommentId")
                        .HasColumnType("integer");

                    b.Property<List<int>>("CommentsIds")
                        .IsRequired()
                        .HasColumnType("integer[]");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<List<int>>("LikesIds")
                        .IsRequired()
                        .HasColumnType("integer[]");

                    b.Property<int?>("ParentCommentId")
                        .HasColumnType("integer");

                    b.Property<int?>("PostId")
                        .HasColumnType("integer");

                    b.Property<string>("TextContent")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<List<int>>("ViewedBy")
                        .IsRequired()
                        .HasColumnType("integer[]");

                    b.HasKey("Id");

                    b.HasIndex("ParentCommentId");

                    b.HasIndex("PostId");

                    b.HasIndex("UserId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("ApplicationDAL.Entities.Image", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("CommentId")
                        .HasColumnType("integer");

                    b.Property<int?>("PostId")
                        .HasColumnType("integer");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CommentId");

                    b.HasIndex("PostId");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("ApplicationDAL.Entities.Post", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Bookmarks")
                        .HasColumnType("integer");

                    b.Property<List<int>>("CommentsIds")
                        .IsRequired()
                        .HasColumnType("integer[]");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<List<int>>("LikesIds")
                        .IsRequired()
                        .HasColumnType("integer[]");

                    b.Property<List<int>>("RepostersIds")
                        .IsRequired()
                        .HasColumnType("integer[]");

                    b.Property<string>("TextContent")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<List<int>>("ViewedBy")
                        .IsRequired()
                        .HasColumnType("integer[]");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("ApplicationDAL.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Bio")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<List<int>>("BookmarkedPostsIds")
                        .IsRequired()
                        .HasColumnType("integer[]");

                    b.Property<DateOnly>("DateOfBirth")
                        .HasColumnType("date");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<List<int>>("FollowersIds")
                        .IsRequired()
                        .HasColumnType("integer[]");

                    b.Property<List<int>>("FollowingIds")
                        .IsRequired()
                        .HasColumnType("integer[]");

                    b.Property<int?>("ImageId")
                        .HasColumnType("integer");

                    b.Property<string>("Location")
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<List<int>>("RepostsIds")
                        .IsRequired()
                        .HasColumnType("integer[]");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("character varying(15)");

                    b.HasKey("Id");

                    b.HasIndex("ImageId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ApplicationDAL.Entities.Comment", b =>
                {
                    b.HasOne("ApplicationDAL.Entities.Comment", "ParentComment")
                        .WithMany()
                        .HasForeignKey("ParentCommentId");

                    b.HasOne("ApplicationDAL.Entities.Post", "Post")
                        .WithMany()
                        .HasForeignKey("PostId");

                    b.HasOne("ApplicationDAL.Entities.User", "Author")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");

                    b.Navigation("ParentComment");

                    b.Navigation("Post");
                });

            modelBuilder.Entity("ApplicationDAL.Entities.Image", b =>
                {
                    b.HasOne("ApplicationDAL.Entities.Comment", null)
                        .WithMany("Images")
                        .HasForeignKey("CommentId");

                    b.HasOne("ApplicationDAL.Entities.Post", null)
                        .WithMany("Images")
                        .HasForeignKey("PostId");
                });

            modelBuilder.Entity("ApplicationDAL.Entities.Post", b =>
                {
                    b.HasOne("ApplicationDAL.Entities.User", "Author")
                        .WithMany("Posts")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");
                });

            modelBuilder.Entity("ApplicationDAL.Entities.User", b =>
                {
                    b.HasOne("ApplicationDAL.Entities.Image", "Avatar")
                        .WithMany()
                        .HasForeignKey("ImageId");

                    b.Navigation("Avatar");
                });

            modelBuilder.Entity("ApplicationDAL.Entities.Comment", b =>
                {
                    b.Navigation("Images");
                });

            modelBuilder.Entity("ApplicationDAL.Entities.Post", b =>
                {
                    b.Navigation("Images");
                });

            modelBuilder.Entity("ApplicationDAL.Entities.User", b =>
                {
                    b.Navigation("Posts");
                });
#pragma warning restore 612, 618
        }
    }
}
