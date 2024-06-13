using System;
using System.Collections.Generic;
using IGT.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IGT.Data.DatabaseContext;

public class AkramDbContext : IdentityDbContext<User>
{
    public AkramDbContext()
    {
    }

    public AkramDbContext(DbContextOptions<AkramDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Role> Roles { get; set; }
    public virtual DbSet<Privilege> Privileges { get; set; }
    public virtual DbSet<SystemStatusCode> SystemStatusCodes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {


        modelBuilder.Entity<Privilege>(entity =>
        {
            entity.HasKey(e => e.PrivilegeId).HasName("PK__Privileg__B3E77E5CCF515C05");

            entity.ToTable("Privilege");

            entity.Property(e => e.PrivilegeId).ValueGeneratedNever();
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.BackendURL)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.IsGeneral)
                .GetType();
            entity.Property(e => e.IsSuperAdmin)
                .GetType();
            entity.Property(e => e.IsAdmin)
                .GetType();
        });
        modelBuilder.Entity<SystemStatusCode>(entity =>
        {
            entity.HasKey(e => e.SystemStatusCodeId).HasName("PK__SystemStatusCode__B3E77E5CCF515C05");

            entity.ToTable("SystemStatusCode");

            entity.Property(e => e.SystemStatusCodeId).ValueGeneratedNever();
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Model)
                .HasMaxLength(255)
                .IsUnicode(false);
        });
        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasIndex(e => e.SystemStatusCodeId, "IndexRoleSystemStatusCodeId");
            entity.HasOne(d => d.SystemStatusCode)
                .WithMany(p => p.Roles)
                .HasForeignKey(d => d.SystemStatusCodeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ROLE_REFERENCE_SYSTEM_STATUS_CODE");
            entity.HasMany(d => d.Privileges).WithMany(p => p.Roles)
                .UsingEntity<Dictionary<string, object>>(
                    "RolePrivilege",
                    r => r.HasOne<Privilege>().WithMany()
                        .HasForeignKey("Id")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_RolePrivilege_Privilege"),
                    l => l.HasOne<Role>().WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_RolePrivilege_Role"),
                    j =>
                    {
                        j.HasKey("RoleId", "Id");
                        j.ToTable("RolePrivilege");
                    });
        });
        SeedRoles(modelBuilder);
        base.OnModelCreating(modelBuilder);
    }
    private static void SeedRoles(ModelBuilder builder)
    {
        builder.Entity<Role>().HasData(
            new Role() { Name = "Admin", ConcurrencyStamp = "1", NormalizedName = "Admin" },
            new Role() { Name = "User", ConcurrencyStamp = "2", NormalizedName = "User" }
        );
        builder.Entity<Privilege>().HasData(
            new Privilege() { PrivilegeId = 1, Name = "Add user", Code = "addUser", BackendURL = "AuthenticationController/addUser", IsGeneral = false },
            new Privilege() { PrivilegeId = 2, Name = "Add machine", Code = "addMachine", IsSuperAdmin = true, BackendURL = "", IsGeneral = false },
            new Privilege() { PrivilegeId = 3, Name = "Machine status", Code = "Machine status", BackendURL = "", IsGeneral = false },
            new Privilege() { PrivilegeId = 4, Name = "Bussines analytics", Code = "bussinesAnalitics", BackendURL = "", IsGeneral = false },
            new Privilege() { PrivilegeId = 5, Name = "EditDelete user", Code = "editDeleteUser", BackendURL = "", IsGeneral = false }
        );
        builder.Entity<SystemStatusCode>().HasData(
            new SystemStatusCode() {SystemStatusCodeId = 1 ,Name = "DELETED", Status = "DELETED", Model="GENERAL"}
        );
    }
}
