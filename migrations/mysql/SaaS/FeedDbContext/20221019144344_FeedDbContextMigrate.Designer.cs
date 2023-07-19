// <auto-generated />
using System;
using ASC.Feed.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ASC.Migrations.MySql.Migrations
{
    [DbContext(typeof(FeedDbContext))]
    [Migration("20221019144344_FeedDbContextMigrate")]
    partial class FeedDbContextMigrate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("ASC.Feed.Model.FeedAggregate", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(88)")
                        .HasColumnName("id")
                        .UseCollation("utf8_general_ci")
                        .HasAnnotation("MySql:CharSet", "utf8");

                    b.Property<DateTime>("AggregateDate")
                        .HasColumnType("datetime")
                        .HasColumnName("aggregated_date");

                    b.Property<string>("Author")
                        .IsRequired()
                        .HasColumnType("char(38)")
                        .HasColumnName("author")
                        .UseCollation("utf8_general_ci")
                        .HasAnnotation("MySql:CharSet", "utf8");

                    b.Property<string>("ContextId")
                        .HasColumnType("text")
                        .HasColumnName("context_id")
                        .UseCollation("utf8_general_ci")
                        .HasAnnotation("MySql:CharSet", "utf8");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime")
                        .HasColumnName("created_date");

                    b.Property<string>("GroupId")
                        .HasColumnType("varchar(70)")
                        .HasColumnName("group_id")
                        .UseCollation("utf8_general_ci")
                        .HasAnnotation("MySql:CharSet", "utf8");

                    b.Property<string>("Json")
                        .IsRequired()
                        .HasColumnType("mediumtext")
                        .HasColumnName("json")
                        .UseCollation("utf8_general_ci")
                        .HasAnnotation("MySql:CharSet", "utf8");

                    b.Property<string>("Keywords")
                        .HasColumnType("text")
                        .HasColumnName("keywords")
                        .UseCollation("utf8_general_ci")
                        .HasAnnotation("MySql:CharSet", "utf8");

                    b.Property<string>("ModifiedBy")
                        .IsRequired()
                        .HasColumnType("char(38)")
                        .HasColumnName("modified_by")
                        .UseCollation("utf8_general_ci")
                        .HasAnnotation("MySql:CharSet", "utf8");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime")
                        .HasColumnName("modified_date");

                    b.Property<string>("Module")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasColumnName("module")
                        .UseCollation("utf8_general_ci")
                        .HasAnnotation("MySql:CharSet", "utf8");

                    b.Property<string>("Product")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasColumnName("product")
                        .UseCollation("utf8_general_ci")
                        .HasAnnotation("MySql:CharSet", "utf8");

                    b.Property<int>("Tenant")
                        .HasColumnType("int")
                        .HasColumnName("tenant");

                    b.HasKey("Id");

                    b.HasIndex("Tenant", "AggregateDate")
                        .HasDatabaseName("aggregated_date");

                    b.HasIndex("Tenant", "ModifiedDate")
                        .HasDatabaseName("modified_date");

                    b.HasIndex("Tenant", "Product")
                        .HasDatabaseName("product");

                    b.ToTable("feed_aggregate", (string)null);

                    b.HasAnnotation("MySql:CharSet", "utf8");
                });

            modelBuilder.Entity("ASC.Feed.Model.FeedLast", b =>
                {
                    b.Property<string>("LastKey")
                        .HasColumnType("varchar(128)")
                        .HasColumnName("last_key")
                        .UseCollation("utf8_general_ci")
                        .HasAnnotation("MySql:CharSet", "utf8");

                    b.Property<DateTime>("LastDate")
                        .HasColumnType("datetime")
                        .HasColumnName("last_date");

                    b.HasKey("LastKey")
                        .HasName("PRIMARY");

                    b.ToTable("feed_last", (string)null);

                    b.HasAnnotation("MySql:CharSet", "utf8");
                });

            modelBuilder.Entity("ASC.Feed.Model.FeedReaded", b =>
                {
                    b.Property<int>("Tenant")
                        .HasColumnType("int")
                        .HasColumnName("tenant_id");

                    b.Property<string>("UserId")
                        .HasColumnType("varchar(38)")
                        .HasColumnName("user_id")
                        .UseCollation("utf8_general_ci")
                        .HasAnnotation("MySql:CharSet", "utf8");

                    b.Property<string>("Module")
                        .HasColumnType("varchar(50)")
                        .HasColumnName("module")
                        .UseCollation("utf8_general_ci")
                        .HasAnnotation("MySql:CharSet", "utf8");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("datetime")
                        .HasColumnName("timestamp");

                    b.HasKey("Tenant", "UserId", "Module")
                        .HasName("PRIMARY");

                    b.ToTable("feed_readed", (string)null);

                    b.HasAnnotation("MySql:CharSet", "utf8");
                });

            modelBuilder.Entity("ASC.Feed.Model.FeedUsers", b =>
                {
                    b.Property<string>("FeedId")
                        .HasColumnType("varchar(88)")
                        .HasColumnName("feed_id")
                        .UseCollation("utf8_general_ci")
                        .HasAnnotation("MySql:CharSet", "utf8");

                    b.Property<string>("UserId")
                        .HasColumnType("char(38)")
                        .HasColumnName("user_id")
                        .UseCollation("utf8_general_ci")
                        .HasAnnotation("MySql:CharSet", "utf8");

                    b.HasKey("FeedId", "UserId")
                        .HasName("PRIMARY");

                    b.HasIndex("UserId")
                        .HasDatabaseName("user_id");

                    b.ToTable("feed_users", (string)null);

                    b.HasAnnotation("MySql:CharSet", "utf8");
                });
#pragma warning restore 612, 618
        }
    }
}