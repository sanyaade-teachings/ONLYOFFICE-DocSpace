// <auto-generated />
using System;
using ASC.Core.Common.EF.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ASC.Core.Common.Migrations.MySql.WebstudioDbContextMySql
{
    [DbContext(typeof(MySqlWebstudioDbContext))]
    [Migration("20220711152838_WebstudioDbContextMySql")]
    partial class WebstudioDbContextMySql
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("ASC.Core.Common.EF.Model.DbTenant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<string>("Alias")
                        .IsRequired()
                        .HasColumnType("varchar(100)")
                        .HasColumnName("alias")
                        .UseCollation("utf8_general_ci")
                        .HasAnnotation("MySql:CharSet", "utf8");

                    b.Property<bool>("Calls")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("calls")
                        .HasDefaultValueSql("'1'");

                    b.Property<DateTime>("CreationDateTime")
                        .HasColumnType("datetime")
                        .HasColumnName("creationdatetime");

                    b.Property<int>("Industry")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("industry")
                        .HasDefaultValueSql("'0'");

                    b.Property<string>("Language")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(10)")
                        .HasColumnName("language")
                        .HasDefaultValueSql("'en-US'")
                        .UseCollation("utf8_general_ci")
                        .HasAnnotation("MySql:CharSet", "utf8");

                    b.Property<DateTime>("LastModified")
                        .HasColumnType("timestamp")
                        .HasColumnName("last_modified");

                    b.Property<string>("MappedDomain")
                        .HasColumnType("varchar(100)")
                        .HasColumnName("mappeddomain")
                        .UseCollation("utf8_general_ci")
                        .HasAnnotation("MySql:CharSet", "utf8");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(255)")
                        .HasColumnName("name")
                        .UseCollation("utf8_general_ci")
                        .HasAnnotation("MySql:CharSet", "utf8");

                    b.Property<string>("OwnerId")
                        .HasColumnType("varchar(38)")
                        .HasColumnName("owner_id")
                        .UseCollation("utf8_general_ci")
                        .HasAnnotation("MySql:CharSet", "utf8");

                    b.Property<string>("PaymentId")
                        .HasColumnType("varchar(38)")
                        .HasColumnName("payment_id")
                        .UseCollation("utf8_general_ci")
                        .HasAnnotation("MySql:CharSet", "utf8");

                    b.Property<bool>("Spam")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("spam")
                        .HasDefaultValueSql("'1'");

                    b.Property<int>("Status")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("status")
                        .HasDefaultValueSql("'0'");

                    b.Property<DateTime?>("StatusChanged")
                        .HasColumnType("datetime")
                        .HasColumnName("statuschanged");

                    b.Property<string>("TimeZone")
                        .HasColumnType("varchar(50)")
                        .HasColumnName("timezone")
                        .UseCollation("utf8_general_ci")
                        .HasAnnotation("MySql:CharSet", "utf8");

                    b.Property<int>("TrustedDomainsEnabled")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("trusteddomainsenabled")
                        .HasDefaultValueSql("'1'");

                    b.Property<string>("TrustedDomainsRaw")
                        .HasColumnType("varchar(1024)")
                        .HasColumnName("trusteddomains")
                        .UseCollation("utf8_general_ci")
                        .HasAnnotation("MySql:CharSet", "utf8");

                    b.Property<int>("Version")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("version")
                        .HasDefaultValueSql("'2'");

                    b.Property<DateTime?>("Version_Changed")
                        .HasColumnType("datetime")
                        .HasColumnName("version_changed");

                    b.HasKey("Id");

                    b.HasIndex("Alias")
                        .IsUnique()
                        .HasDatabaseName("alias");

                    b.HasIndex("LastModified")
                        .HasDatabaseName("last_modified");

                    b.HasIndex("MappedDomain")
                        .HasDatabaseName("mappeddomain");

                    b.HasIndex("Version")
                        .HasDatabaseName("version");

                    b.ToTable("tenants_tenants", (string)null);

                    b.HasAnnotation("MySql:CharSet", "utf8");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Alias = "localhost",
                            Calls = false,
                            CreationDateTime = new DateTime(2021, 3, 9, 17, 46, 59, 97, DateTimeKind.Utc).AddTicks(4317),
                            Industry = 0,
                            LastModified = new DateTime(2022, 7, 8, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Web Office",
                            OwnerId = "66faa6e4-f133-11ea-b126-00ffeec8b4ef",
                            Spam = false,
                            Status = 0,
                            TrustedDomainsEnabled = 0,
                            Version = 0
                        });
                });

            modelBuilder.Entity("ASC.Core.Common.EF.Model.DbWebstudioIndex", b =>
                {
                    b.Property<string>("IndexName")
                        .HasColumnType("varchar(50)")
                        .HasColumnName("index_name")
                        .UseCollation("utf8_general_ci")
                        .HasAnnotation("MySql:CharSet", "utf8");

                    b.Property<DateTime>("LastModified")
                        .HasColumnType("timestamp")
                        .HasColumnName("last_modified");

                    b.HasKey("IndexName")
                        .HasName("PRIMARY");

                    b.ToTable("webstudio_index", (string)null);

                    b.HasAnnotation("MySql:CharSet", "utf8");
                });

            modelBuilder.Entity("ASC.Core.Common.EF.Model.DbWebstudioSettings", b =>
                {
                    b.Property<int>("TenantId")
                        .HasColumnType("int")
                        .HasColumnName("TenantID");

                    b.Property<string>("Id")
                        .HasColumnType("varchar(64)")
                        .HasColumnName("ID")
                        .UseCollation("utf8_general_ci")
                        .HasAnnotation("MySql:CharSet", "utf8");

                    b.Property<string>("UserId")
                        .HasColumnType("varchar(64)")
                        .HasColumnName("UserID")
                        .UseCollation("utf8_general_ci")
                        .HasAnnotation("MySql:CharSet", "utf8");

                    b.Property<string>("Data")
                        .IsRequired()
                        .HasColumnType("mediumtext")
                        .UseCollation("utf8_general_ci")
                        .HasAnnotation("MySql:CharSet", "utf8");

                    b.HasKey("TenantId", "Id", "UserId")
                        .HasName("PRIMARY");

                    b.HasIndex("Id")
                        .HasDatabaseName("ID");

                    b.ToTable("webstudio_settings", (string)null);

                    b.HasAnnotation("MySql:CharSet", "utf8");

                    b.HasData(
                        new
                        {
                            TenantId = 1,
                            Id = "9a925891-1f92-4ed7-b277-d6f649739f06",
                            UserId = "00000000-0000-0000-0000-000000000000",
                            Data = "{\"Completed\":false}"
                        });
                });

            modelBuilder.Entity("ASC.Core.Common.EF.Model.DbWebstudioUserVisit", b =>
                {
                    b.Property<int>("TenantId")
                        .HasColumnType("int")
                        .HasColumnName("tenantid");

                    b.Property<DateTime>("VisitDate")
                        .HasColumnType("datetime")
                        .HasColumnName("visitdate");

                    b.Property<string>("ProductId")
                        .HasColumnType("varchar(38)")
                        .HasColumnName("productid")
                        .UseCollation("utf8_general_ci")
                        .HasAnnotation("MySql:CharSet", "utf8");

                    b.Property<string>("UserId")
                        .HasColumnType("varchar(38)")
                        .HasColumnName("userid")
                        .UseCollation("utf8_general_ci")
                        .HasAnnotation("MySql:CharSet", "utf8");

                    b.Property<DateTime?>("FirstVisitTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasColumnName("firstvisittime")
                        .HasDefaultValueSql("NULL");

                    b.Property<DateTime?>("LastVisitTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasColumnName("lastvisittime")
                        .HasDefaultValueSql("NULL");

                    b.Property<int>("VisitCount")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("visitcount")
                        .HasDefaultValueSql("'0'");

                    b.HasKey("TenantId", "VisitDate", "ProductId", "UserId")
                        .HasName("PRIMARY");

                    b.HasIndex("VisitDate")
                        .HasDatabaseName("visitdate");

                    b.ToTable("webstudio_uservisit", (string)null);

                    b.HasAnnotation("MySql:CharSet", "utf8");
                });
#pragma warning restore 612, 618
        }
    }
}
