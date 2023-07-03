﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Lesson5_EFCore_Querys.Migrations
{
    [DbContext(typeof(ApplicationDBContext))]
    [Migration("20230703063137_mig-1")]
    partial class mig1
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Parca", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ParcaAdi")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UrunId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UrunId");

                    b.ToTable("Parca");
                });

            modelBuilder.Entity("Urun", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<float>("Fiyat")
                        .HasColumnType("real");

                    b.Property<string>("UrunAdi")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Urunler");
                });

            modelBuilder.Entity("UrunParca", b =>
                {
                    b.Property<int>("UrunId")
                        .HasColumnType("int");

                    b.Property<int>("ParcaId")
                        .HasColumnType("int");

                    b.HasKey("UrunId", "ParcaId");

                    b.HasIndex("ParcaId");

                    b.ToTable("UrunParca");
                });

            modelBuilder.Entity("Parca", b =>
                {
                    b.HasOne("Urun", null)
                        .WithMany("Parcalar")
                        .HasForeignKey("UrunId");
                });

            modelBuilder.Entity("UrunParca", b =>
                {
                    b.HasOne("Parca", "Parca")
                        .WithMany()
                        .HasForeignKey("ParcaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Urun", "Urun")
                        .WithMany()
                        .HasForeignKey("UrunId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Parca");

                    b.Navigation("Urun");
                });

            modelBuilder.Entity("Urun", b =>
                {
                    b.Navigation("Parcalar");
                });
#pragma warning restore 612, 618
        }
    }
}
