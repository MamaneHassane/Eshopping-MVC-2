﻿// <auto-generated />
using System;
using Eshopping_MVC.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Eshopping_MVC.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20231221092343_m7")]
    partial class m7
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.0");

            modelBuilder.Entity("CartProduct", b =>
                {
                    b.Property<int>("ProductsproductId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("cartsCartId")
                        .HasColumnType("INTEGER");

                    b.HasKey("ProductsproductId", "cartsCartId");

                    b.HasIndex("cartsCartId");

                    b.ToTable("CartProduct");
                });

            modelBuilder.Entity("Eshopping_MVC.Models.Cart", b =>
                {
                    b.Property<int>("CartId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ClientId")
                        .HasColumnType("INTEGER");

                    b.HasKey("CartId");

                    b.HasIndex("ClientId");

                    b.ToTable("Carts");
                });

            modelBuilder.Entity("Eshopping_MVC.Models.Client", b =>
                {
                    b.Property<int>("clientId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("CartId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("clientId");

                    b.HasIndex("CartId")
                        .IsUnique();

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("Eshopping_MVC.Models.Product", b =>
                {
                    b.Property<int>("productId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<double>("Price")
                        .HasColumnType("REAL");

                    b.Property<int>("Quantity")
                        .HasColumnType("INTEGER");

                    b.HasKey("productId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("Eshopping_MVC.Models.ProductCopy", b =>
                {
                    b.Property<string>("SerialCode")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<int?>("CartId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ProductId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Quantity")
                        .HasColumnType("INTEGER");

                    b.HasKey("SerialCode");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductCopies");
                });

            modelBuilder.Entity("CartProduct", b =>
                {
                    b.HasOne("Eshopping_MVC.Models.Product", null)
                        .WithMany()
                        .HasForeignKey("ProductsproductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Eshopping_MVC.Models.Cart", null)
                        .WithMany()
                        .HasForeignKey("cartsCartId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Eshopping_MVC.Models.Cart", b =>
                {
                    b.HasOne("Eshopping_MVC.Models.Client", "Client")
                        .WithMany()
                        .HasForeignKey("ClientId");

                    b.Navigation("Client");
                });

            modelBuilder.Entity("Eshopping_MVC.Models.Client", b =>
                {
                    b.HasOne("Eshopping_MVC.Models.Cart", "Cart")
                        .WithOne()
                        .HasForeignKey("Eshopping_MVC.Models.Client", "CartId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Cart");
                });

            modelBuilder.Entity("Eshopping_MVC.Models.ProductCopy", b =>
                {
                    b.HasOne("Eshopping_MVC.Models.Product", null)
                        .WithMany("ProductCopies")
                        .HasForeignKey("ProductId");
                });

            modelBuilder.Entity("Eshopping_MVC.Models.Product", b =>
                {
                    b.Navigation("ProductCopies");
                });
#pragma warning restore 612, 618
        }
    }
}
