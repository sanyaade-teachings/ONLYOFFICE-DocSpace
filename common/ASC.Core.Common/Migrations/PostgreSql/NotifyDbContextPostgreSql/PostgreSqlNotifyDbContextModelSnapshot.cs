// <auto-generated />
using System;
using ASC.Core.Common.EF.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ASC.Core.Common.Migrations.PostgreSql.NotifyDbContextPostgreSql
{
    [DbContext(typeof(PostgreSqlNotifyDbContext))]
    partial class PostgreSqlNotifyDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.10")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("ASC.Core.Common.EF.Model.NotifyInfo", b =>
                {
                    b.Property<int>("NotifyId")
                        .HasColumnType("integer")
                        .HasColumnName("notify_id");

                    b.Property<int>("Attempts")
                        .HasColumnType("integer")
                        .HasColumnName("attempts");

                    b.Property<DateTime>("ModifyDate")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("modify_date");

                    b.Property<int>("Priority")
                        .HasColumnType("integer")
                        .HasColumnName("priority");

                    b.Property<int>("State")
                        .HasColumnType("integer")
                        .HasColumnName("state");

                    b.HasKey("NotifyId")
                        .HasName("notify_info_pkey");

                    b.HasIndex("State")
                        .HasDatabaseName("state");

                    b.ToTable("notify_info", "onlyoffice");
                });

            modelBuilder.Entity("ASC.Core.Common.EF.Model.NotifyQueue", b =>
                {
                    b.Property<int>("NotifyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("notify_id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Attachments")
                        .HasColumnType("text")
                        .HasColumnName("attachments");

                    b.Property<string>("AutoSubmitted")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)")
                        .HasColumnName("auto_submitted")
                        .HasDefaultValueSql("NULL");

                    b.Property<string>("Content")
                        .HasColumnType("text")
                        .HasColumnName("content");

                    b.Property<string>("ContentType")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)")
                        .HasColumnName("content_type")
                        .HasDefaultValueSql("NULL");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("creation_date");

                    b.Property<string>("Reciever")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("reciever")
                        .HasDefaultValueSql("NULL");

                    b.Property<string>("ReplyTo")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(1024)
                        .HasColumnType("character varying(1024)")
                        .HasColumnName("reply_to")
                        .HasDefaultValueSql("NULL");

                    b.Property<string>("Sender")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("sender")
                        .HasDefaultValueSql("NULL");

                    b.Property<string>("SenderType")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)")
                        .HasColumnName("sender_type")
                        .HasDefaultValueSql("NULL");

                    b.Property<string>("Subject")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(1024)
                        .HasColumnType("character varying(1024)")
                        .HasColumnName("subject")
                        .HasDefaultValueSql("NULL");

                    b.Property<int>("TenantId")
                        .HasColumnType("integer")
                        .HasColumnName("tenant_id");

                    b.HasKey("NotifyId")
                        .HasName("notify_queue_pkey");

                    b.ToTable("notify_queue", "onlyoffice");
                });
#pragma warning restore 612, 618
        }
    }
}
