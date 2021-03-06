﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PaymentGateway.Persistence.SqlLite;

namespace PaymentGateway.Persistence.SqlLite.Migrations
{
    [DbContext(typeof(PaymentGatewayDbContext))]
    [Migration("20210214161836_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.12");

            modelBuilder.Entity("PaymentGateway.Domain.Merchant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Merchants");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Apple"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Microsoft"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Amazon"
                        });
                });

            modelBuilder.Entity("PaymentGateway.Domain.Payment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("AcquiringBankPaymentId")
                        .HasColumnType("TEXT");

                    b.Property<string>("CardHolderName")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ExpiryDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("MaskedCardNumber")
                        .HasColumnType("TEXT");

                    b.Property<int>("MerchantId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("MerchantId");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("PaymentGateway.Domain.Payment", b =>
                {
                    b.HasOne("PaymentGateway.Domain.Merchant", null)
                        .WithMany()
                        .HasForeignKey("MerchantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("PaymentGateway.Domain.Money", "Money", b1 =>
                        {
                            b1.Property<int>("PaymentId")
                                .HasColumnType("INTEGER");

                            b1.Property<decimal>("Amount")
                                .HasColumnType("TEXT");

                            b1.Property<string>("Currency")
                                .HasColumnType("TEXT");

                            b1.HasKey("PaymentId");

                            b1.ToTable("Payments");

                            b1.WithOwner()
                                .HasForeignKey("PaymentId");
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
