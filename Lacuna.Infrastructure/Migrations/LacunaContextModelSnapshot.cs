// <auto-generated />
using System;
using Lacuna.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Lacuna.Infrastructure.Migrations
{
    [DbContext(typeof(LacunaContext))]
    partial class LacunaContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Lacuna.Domain.Users.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("VARCHAR(100)")
                        .HasColumnName("E-Mail");

                    b.Property<string>("HashedPassword")
                        .IsRequired()
                        .HasColumnType("VARCHAR(64)")
                        .HasColumnName("HashPassword");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("VARCHAR(32)")
                        .HasColumnName("UserName");

                    b.HasKey("Id");

                    b.ToTable("LacunaUsers");
                });
#pragma warning restore 612, 618
        }
    }
}
