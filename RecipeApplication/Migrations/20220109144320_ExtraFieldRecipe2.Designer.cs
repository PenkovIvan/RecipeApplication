﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RecipeApplication.Data;

namespace RecipeApplication.Migrations
{
    [DbContext(typeof(MyDbContext))]
    [Migration("20220109144320_ExtraFieldRecipe2")]
    partial class ExtraFieldRecipe2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("RecipeApplication.Data.Ingredient", b =>
                {
                    b.Property<int>("IngredientId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Quantity")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("RecipeId")
                        .HasColumnType("int");

                    b.Property<string>("Unit")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IngredientId");

                    b.HasIndex("RecipeId");

                    b.ToTable("Ingredient");
                });

            modelBuilder.Entity("RecipeApplication.Data.Recipe", b =>
                {
                    b.Property<int>("RecipeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsVegan")
                        .HasColumnType("bit");

                    b.Property<bool>("IsVegetarian")
                        .HasColumnType("bit");

                    b.Property<string>("Method")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<TimeSpan>("TimeToCook")
                        .HasColumnType("time");

                    b.HasKey("RecipeId");

                    b.ToTable("Recipes");
                });

            modelBuilder.Entity("RecipeApplication.Data.Ingredient", b =>
                {
                    b.HasOne("RecipeApplication.Data.Recipe", null)
                        .WithMany("Ingredients")
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RecipeApplication.Data.Recipe", b =>
                {
                    b.Navigation("Ingredients");
                });
#pragma warning restore 612, 618
        }
    }
}