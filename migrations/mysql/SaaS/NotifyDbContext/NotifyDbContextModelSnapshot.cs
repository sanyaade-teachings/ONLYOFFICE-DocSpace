// <auto-generated />
using System;
using ASC.Core.Common.EF.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ASC.Migrations.MySql.Migrations
{
    [DbContext(typeof(NotifyDbContext))]
    partial class NotifyDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("ASC.Core.Common.EF.Model.NotifyInfo", b =>
                {
                    b.Property<int>("NotifyId")
                        .HasColumnType("int")
                        .HasColumnName("notify_id");

                    b.Property<int>("Attempts")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("attempts")
                        .HasDefaultValueSql("'0'");

                    b.Property<DateTime>("ModifyDate")
                        .HasColumnType("datetime")
                        .HasColumnName("modify_date");

                    b.Property<int>("Priority")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("priority")
                        .HasDefaultValueSql("'0'");

                    b.Property<int>("State")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("state")
                        .HasDefaultValueSql("'0'");

                    b.HasKey("NotifyId")
                        .HasName("PRIMARY");

                    b.HasIndex("State")
                        .HasDatabaseName("state");

                    b.ToTable("notify_info", (string)null);

                    b.HasAnnotation("MySql:CharSet", "utf8");
                });

            modelBuilder.Entity("ASC.Core.Common.EF.Model.NotifyQueue", b =>
                {
                    b.Property<int>("NotifyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("notify_id");

                    b.Property<string>("Attachments")
                        .HasColumnType("text")
                        .HasColumnName("attachments")
                        .UseCollation("utf8_general_ci")
                        .HasAnnotation("MySql:CharSet", "utf8");

                    b.Property<string>("AutoSubmitted")
                        .HasColumnType("varchar(64)")
                        .HasColumnName("auto_submitted")
                        .UseCollation("utf8_general_ci")
                        .HasAnnotation("MySql:CharSet", "utf8");

                    b.Property<string>("Content")
                        .HasColumnType("text")
                        .HasColumnName("content")
                        .UseCollation("utf8_general_ci")
                        .HasAnnotation("MySql:CharSet", "utf8");

                    b.Property<string>("ContentType")
                        .HasColumnType("varchar(64)")
                        .HasColumnName("content_type")
                        .UseCollation("utf8_general_ci")
                        .HasAnnotation("MySql:CharSet", "utf8");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime")
                        .HasColumnName("creation_date");

                    b.Property<string>("Reciever")
                        .HasColumnType("varchar(255)")
                        .HasColumnName("reciever")
                        .UseCollation("utf8_general_ci")
                        .HasAnnotation("MySql:CharSet", "utf8");

                    b.Property<string>("ReplyTo")
                        .HasColumnType("varchar(1024)")
                        .HasColumnName("reply_to")
                        .UseCollation("utf8_general_ci")
                        .HasAnnotation("MySql:CharSet", "utf8");

                    b.Property<string>("Sender")
                        .HasColumnType("varchar(255)")
                        .HasColumnName("sender")
                        .UseCollation("utf8_general_ci")
                        .HasAnnotation("MySql:CharSet", "utf8");

                    b.Property<string>("SenderType")
                        .HasColumnType("varchar(64)")
                        .HasColumnName("sender_type")
                        .UseCollation("utf8_general_ci")
                        .HasAnnotation("MySql:CharSet", "utf8");

                    b.Property<string>("Subject")
                        .HasColumnType("varchar(1024)")
                        .HasColumnName("subject")
                        .UseCollation("utf8_general_ci")
                        .HasAnnotation("MySql:CharSet", "utf8");

                    b.Property<int>("TenantId")
                        .HasColumnType("int")
                        .HasColumnName("tenant_id");

                    b.HasKey("NotifyId")
                        .HasName("PRIMARY");

                    b.HasIndex("CreationDate")
                        .HasDatabaseName("creation_date");

                    b.ToTable("notify_queue", (string)null);

                    b.HasAnnotation("MySql:CharSet", "utf8");
                });
#pragma warning restore 612, 618
        }
    }
}
