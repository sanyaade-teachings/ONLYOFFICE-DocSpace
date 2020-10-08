﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASC.Core.Common.EF.Model
{
    [Table("tenants_iprestrictions")]
    public class TenantIpRestrictions
    {
        public int Id { get; set; }
        public int Tenant { get; set; }
        public string Ip { get; set; }
    }
    public static class TenantIpRestrictionsExtension
    {
        public static ModelBuilderWrapper AddTenantIpRestrictions(this ModelBuilderWrapper modelBuilder)
        {
            modelBuilder
                .Add(MySqlAddTenantIpRestrictions, Provider.MySql)
                .Add(PgSqlAddTenantIpRestrictions, Provider.Postgre);
            return modelBuilder;
        }
        public static void MySqlAddTenantIpRestrictions(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TenantIpRestrictions>(entity =>
            {
                entity.ToTable("tenants_iprestrictions");

                entity.HasIndex(e => e.Tenant)
                    .HasName("tenant");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Ip)
                    .IsRequired()
                    .HasColumnName("ip")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Tenant).HasColumnName("tenant");
            });

        }
        public static void PgSqlAddTenantIpRestrictions(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TenantIpRestrictions>(entity =>
            {
                entity.ToTable("tenants_iprestrictions", "onlyoffice");

                entity.HasIndex(e => e.Tenant)
                    .HasName("tenant_tenants_iprestrictions");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Ip)
                    .IsRequired()
                    .HasColumnName("ip")
                    .HasMaxLength(50);

                entity.Property(e => e.Tenant).HasColumnName("tenant");
            });

        }
    }
}
