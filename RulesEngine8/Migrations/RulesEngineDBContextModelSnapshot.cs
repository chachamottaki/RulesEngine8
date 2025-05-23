﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using RulesEngine8.Models;

#nullable disable

namespace RulesEngine8.Migrations
{
    [DbContext(typeof(RulesEngineDBContext))]
    partial class RulesEngineDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("RulesEngine8.Models.ConfigItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("AssetID")
                        .HasColumnType("text");

                    b.Property<string>("DeviceID")
                        .HasColumnType("text");

                    b.Property<string>("DigitalInputsJson")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("ConfigItems");
                });

            modelBuilder.Entity("RulesEngine8.Models.DI", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("ConfigItemID")
                        .HasColumnType("integer");

                    b.Property<int>("DIIndex")
                        .HasColumnType("integer");

                    b.Property<bool>("Error")
                        .HasColumnType("boolean");

                    b.Property<string>("ErrorMsg")
                        .HasColumnType("text");

                    b.Property<string>("InstallationKey")
                        .HasColumnType("text");

                    b.Property<string>("InstallationType")
                        .HasColumnType("text");

                    b.Property<bool>("Invert")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsAlarm")
                        .HasColumnType("boolean");

                    b.Property<string>("LastTime")
                        .HasColumnType("text");

                    b.Property<long>("NumChannels")
                        .HasColumnType("bigint");

                    b.Property<string>("OrderNumber")
                        .HasColumnType("text");

                    b.Property<bool>("Send")
                        .HasColumnType("boolean");

                    b.Property<bool>("SendOnChange")
                        .HasColumnType("boolean");

                    b.Property<string>("SensorKey")
                        .HasColumnType("text");

                    b.Property<string>("SensorType")
                        .HasColumnType("text");

                    b.Property<string>("alarmId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("isActive")
                        .HasColumnType("boolean");

                    b.Property<bool>("isTPST")
                        .HasColumnType("boolean");

                    b.Property<string>("longDescription")
                        .HasColumnType("text");

                    b.Property<bool>("sendEmail")
                        .HasColumnType("boolean");

                    b.Property<string>("shortDescription")
                        .HasColumnType("text");

                    b.Property<long?>("topColor")
                        .HasColumnType("bigint");

                    b.Property<int>("topIndex")
                        .HasColumnType("integer");

                    b.Property<bool>("topIsActive")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.HasIndex("ConfigItemID");

                    b.ToTable("DigitalInputs");
                });

            modelBuilder.Entity("RulesEngine8.Models.HistoryTable", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("DeviceId")
                        .HasColumnType("text");

                    b.Property<string>("alarmId")
                        .HasColumnType("text");

                    b.Property<string>("assetId")
                        .HasColumnType("text");

                    b.Property<string>("emailContent")
                        .HasColumnType("text");

                    b.Property<string>("emailRecipient")
                        .HasColumnType("text");

                    b.Property<bool>("emailSent")
                        .HasColumnType("boolean");

                    b.Property<bool>("isAlarm")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("timestamp")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("HistoryTables");
                });

            modelBuilder.Entity("RulesEngine8.Models.RuleChain", b =>
                {
                    b.Property<int>("RuleChainId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("RuleChainId"));

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("NodesJson")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("RuleChainId");

                    b.ToTable("RuleChains");
                });

            modelBuilder.Entity("RulesEngine8.Models.RuleNode", b =>
                {
                    b.Property<int>("RuleNodeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("RuleNodeId"));

                    b.Property<string>("ConfigurationJson")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("NodeType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("NodeUUID")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("RuleChainID")
                        .HasColumnType("integer");

                    b.HasKey("RuleNodeId");

                    b.HasIndex("RuleChainID");

                    b.ToTable("RuleNodes");
                });

            modelBuilder.Entity("RulesEngine8.Models.ConfigItem", b =>
                {
                    b.OwnsOne("RulesEngine8.Models.ConfigJson", "Config", b1 =>
                        {
                            b1.Property<int>("ConfigItemId")
                                .HasColumnType("integer");

                            b1.Property<string>("email")
                                .HasColumnType("text");

                            b1.Property<string>("sender")
                                .HasColumnType("text");

                            b1.HasKey("ConfigItemId");

                            b1.ToTable("ConfigItems");

                            b1.ToJson("Config");

                            b1.WithOwner()
                                .HasForeignKey("ConfigItemId");
                        });

                    b.Navigation("Config");
                });

            modelBuilder.Entity("RulesEngine8.Models.DI", b =>
                {
                    b.HasOne("RulesEngine8.Models.ConfigItem", null)
                        .WithMany("DigitalInputs")
                        .HasForeignKey("ConfigItemID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RulesEngine8.Models.RuleNode", b =>
                {
                    b.HasOne("RulesEngine8.Models.RuleChain", null)
                        .WithMany("Nodes")
                        .HasForeignKey("RuleChainID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RulesEngine8.Models.ConfigItem", b =>
                {
                    b.Navigation("DigitalInputs");
                });

            modelBuilder.Entity("RulesEngine8.Models.RuleChain", b =>
                {
                    b.Navigation("Nodes");
                });
#pragma warning restore 612, 618
        }
    }
}
