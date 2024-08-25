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
    public virtual DbSet<Session> Sessions { get; set; }
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
            entity.Property(e => e.CreatedUserId)
                .HasMaxLength(500)
                .IsUnicode(false);
        });
        modelBuilder.Entity<Session>(entity =>
        {
            entity.HasKey(e => e.SessionId).HasName("PK__Session__B3E77E5CCF515C05");

            entity.ToTable("Session");

            entity.Property(e => e.SessionId).ValueGeneratedOnAdd();
            entity.Property(e => e.Token)
                .IsUnicode(false);
            entity.HasIndex(e => e.SystemStatusCodeId, "IndexSessionSystemStatusCodeId");
            entity.HasOne(d => d.SystemStatusCode)
                .WithMany(p => p.Sessions)
                .HasForeignKey(d => d.SystemStatusCodeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SESSION_REFERENCE_SYSTEM_STATUS_CODE");
            entity.HasIndex(e => e.UserId, "IndexSessionUserId");
            entity.HasOne(d => d.User)
                .WithMany(p => p.Sessions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SESSION_REFERENCE_USER");
        });
        modelBuilder.Entity<OTP>(entity =>
        {
            entity.HasKey(e => e.OTPId).HasName("PK__OTP__B3E77E5CCF515C05");
            entity.Property(e => e.OTPCode)
                .IsRequired()
                .HasMaxLength(6);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
            entity.Property(e => e.ExpiresAt)
                .IsRequired();
            entity.Property(e => e.IsUsed)
                .IsRequired()
                .HasDefaultValue(false);
            entity.HasOne(e => e.User)
                .WithMany(u => u.OTPs)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_OTP_REFERENCE_USER");
        });
        SeedRoles(modelBuilder);
        base.OnModelCreating(modelBuilder);
    }
    private static void SeedRoles(ModelBuilder builder)
    {
        builder.Entity<Role>().HasData(
            new Role() { Name = "Admin", ConcurrencyStamp = "1", NormalizedName = "Admin", CreatedUserId = "-99" },
            new Role() { Name = "User", ConcurrencyStamp = "2", NormalizedName = "User", CreatedUserId = "-99" }
        );
        builder.Entity<Privilege>().HasData(
            new Privilege() { PrivilegeId = 1, Name = "forget password", Code = "forgetPassword", BackendURL = "AuthenticationController/forgetPassword", IsGeneral = true, IsSuperAdmin = false },
            new Privilege() { PrivilegeId = 2, Name = "reset password", Code = "resetPassword", BackendURL = "AuthenticationController/resetPassword", IsGeneral = true, IsSuperAdmin = false },
            new Privilege() { PrivilegeId = 3, Name = "Add user", Code = "addUser", BackendURL = "UserManagment/addUser", IsGeneral = false, IsSuperAdmin = false },
            new Privilege() { PrivilegeId = 4, Name = "Get all users", Code = "getAllUsers", BackendURL = "UserManagment/getAllUsers", IsGeneral = false, IsSuperAdmin = true },
            new Privilege() { PrivilegeId = 5, Name = "Add role", Code = "addRole", BackendURL = "RoleManagment/addRole", IsGeneral = false, IsSuperAdmin = false },
            new Privilege() { PrivilegeId = 6, Name = "Update role", Code = "updateRole", BackendURL = "RoleManagment/updateRole", IsGeneral = false, IsSuperAdmin = false },
            new Privilege() { PrivilegeId = 7, Name = "Delete role", Code = "deleteRole", BackendURL = "RoleManagment/deleteRole", IsGeneral = false, IsSuperAdmin = false },
            new Privilege() { PrivilegeId = 8, Name = "Get role", Code = "getRoles", BackendURL = "RoleManagment/getRoles", IsGeneral = false, IsSuperAdmin = false }
        );
        builder.Entity<SystemStatusCode>().HasData(
            new SystemStatusCode() {SystemStatusCodeId = 1 ,Name = "DELETED", Status = "DELETED", Model="GENERAL" },
            new SystemStatusCode() { SystemStatusCodeId = 2, Name = "EXPIRED", Status = "EXPIRED", Model = "GENERAL" },
            new SystemStatusCode() { SystemStatusCodeId = 3, Name = "ACTIVE", Status = "ACTIVE", Model = "GENERAL" }
        );
    }
}
