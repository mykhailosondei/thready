﻿// <auto-generated />
using ApplicationDAL.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ApplicationDAL.IndexerMigrations
{
    [DbContext(typeof(IndexerContext))]
    [Migration("20231018045256_IndexedWordPostCountColumnAdded")]
    partial class IndexedWordPostCountColumnAdded
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ApplicationDAL.Entities.IndexedWord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("PostsCount")
                        .HasColumnType("integer");

                    b.Property<string>("Word")
                        .IsRequired()
                        .HasMaxLength(45)
                        .HasColumnType("character varying(45)");

                    b.HasKey("Id");

                    b.HasIndex("Word")
                        .IsUnique();

                    b.ToTable("IndexedWords");
                });

            modelBuilder.Entity("ApplicationDAL.Entities.WordCountInPostId", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("IndexedWordId")
                        .HasColumnType("integer");

                    b.Property<int>("PostId")
                        .HasColumnType("integer");

                    b.Property<int>("WordCount")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("IndexedWordId");

                    b.ToTable("WordCountInPostIds");
                });

            modelBuilder.Entity("ApplicationDAL.Entities.WordCountInPostId", b =>
                {
                    b.HasOne("ApplicationDAL.Entities.IndexedWord", "IndexedWord")
                        .WithMany("WordCountInPostId")
                        .HasForeignKey("IndexedWordId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("IndexedWord");
                });

            modelBuilder.Entity("ApplicationDAL.Entities.IndexedWord", b =>
                {
                    b.Navigation("WordCountInPostId");
                });
#pragma warning restore 612, 618
        }
    }
}
