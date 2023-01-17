﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TareasMVC;

#nullable disable

namespace TareasMVC.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TareasMVC.Entidades.ArchivoAdjunto", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("FechaCreacion")
                        .HasColumnType("datetime2");

                    b.Property<int>("Orden")
                        .HasColumnType("int");

                    b.Property<int>("TareaId")
                        .HasColumnType("int");

                    b.Property<string>("Titulo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Url")
                        .IsRequired()
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("TareaId");

                    b.ToTable("ArchivosAdjuntos");
                });

            modelBuilder.Entity("TareasMVC.Entidades.Paso", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Orden")
                        .HasColumnType("int");

                    b.Property<bool>("Realizado")
                        .HasColumnType("bit");

                    b.Property<int>("TareaId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TareaId");

                    b.ToTable("Pasos");
                });

            modelBuilder.Entity("TareasMVC.Entidades.Tarea", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("FechaCreacion")
                        .HasColumnType("datetime2");

                    b.Property<int>("Orden")
                        .HasColumnType("int");

                    b.Property<string>("Titulo")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.HasKey("Id");

                    b.ToTable("Tareas");
                });

            modelBuilder.Entity("TareasMVC.Entidades.ArchivoAdjunto", b =>
                {
                    b.HasOne("TareasMVC.Entidades.Tarea", "Tarea")
                        .WithMany("ArchivosAdjuntos")
                        .HasForeignKey("TareaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Tarea");
                });

            modelBuilder.Entity("TareasMVC.Entidades.Paso", b =>
                {
                    b.HasOne("TareasMVC.Entidades.Tarea", "Tarea")
                        .WithMany("Pasos")
                        .HasForeignKey("TareaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Tarea");
                });

            modelBuilder.Entity("TareasMVC.Entidades.Tarea", b =>
                {
                    b.Navigation("ArchivosAdjuntos");

                    b.Navigation("Pasos");
                });
#pragma warning restore 612, 618
        }
    }
}
