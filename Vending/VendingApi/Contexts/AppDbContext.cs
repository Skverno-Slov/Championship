using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using VendingApi.Models;

namespace VendingApi.Contexts;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<MachineOperator> MachineOperators { get; set; }

    public virtual DbSet<MachinePlace> MachinePlaces { get; set; }

    public virtual DbSet<MachineProduct> MachineProducts { get; set; }

    public virtual DbSet<Maintenance> Maintenances { get; set; }

    public virtual DbSet<Model> Models { get; set; }

    public virtual DbSet<News> News { get; set; }

    public virtual DbSet<Operator> Operators { get; set; }

    public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<RefrashToken> RefrashTokens { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Sale> Sales { get; set; }

    public virtual DbSet<ServicePriority> ServicePriorities { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<Template> Templates { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<VendingMachine> VendingMachines { get; set; }

    public virtual DbSet<WorkDescription> WorkDescriptions { get; set; }

    public virtual DbSet<WorkMode> WorkModes { get; set; }

    public virtual DbSet<Worker> Workers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Championship;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Company>(entity =>
        {
            entity.ToTable("Company");

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<MachineOperator>(entity =>
        {
            entity.HasKey(e => e.Phone);

            entity.ToTable("MachineOperator");

            entity.Property(e => e.Phone)
                .HasMaxLength(11)
                .IsUnicode(false);
            entity.Property(e => e.Balance).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.MachineId).HasMaxLength(36);

            entity.HasOne(d => d.Machine).WithMany(p => p.MachineOperators)
                .HasForeignKey(d => d.MachineId)
                .HasConstraintName("FK_MachineOperator_VendingMachine");

            entity.HasOne(d => d.Operator).WithMany(p => p.MachineOperators)
                .HasForeignKey(d => d.OperatorId)
                .HasConstraintName("FK_MachineOperator_Operator");
        });

        modelBuilder.Entity<MachinePlace>(entity =>
        {
            entity.HasKey(e => e.PlaceId);

            entity.ToTable("MachinePlace");

            entity.Property(e => e.PlaceId).ValueGeneratedOnAdd();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<MachineProduct>(entity =>
        {
            entity.HasKey(e => new { e.MachineId, e.ProductId });

            entity.ToTable("MachineProduct");

            entity.Property(e => e.MachineId).HasMaxLength(36);
            entity.Property(e => e.ProductId).HasMaxLength(36);
            entity.Property(e => e.Price).HasColumnType("decimal(19, 4)");

            entity.HasOne(d => d.Machine).WithMany(p => p.MachineProducts)
                .HasForeignKey(d => d.MachineId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MachineProduct_VendingMachine");

            entity.HasOne(d => d.Product).WithMany(p => p.MachineProducts)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MachineProduct_Product");
        });

        modelBuilder.Entity<Maintenance>(entity =>
        {
            entity.ToTable("Maintenance");

            entity.Property(e => e.IssuesFound).HasMaxLength(255);
            entity.Property(e => e.MachineId).HasMaxLength(36);
            entity.Property(e => e.WorkerId).HasMaxLength(36);

            entity.HasOne(d => d.Machine).WithMany(p => p.Maintenances)
                .HasForeignKey(d => d.MachineId)
                .HasConstraintName("FK_Maintenance_VendingMachine");

            entity.HasOne(d => d.WorkDescription).WithMany(p => p.Maintenances)
                .HasForeignKey(d => d.WorkDescriptionId)
                .HasConstraintName("FK_Maintenance_WorkDescription");
        });

        modelBuilder.Entity<Model>(entity =>
        {
            entity.ToTable("Model");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<News>(entity =>
        {
            entity.Property(e => e.EventDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Message).HasMaxLength(150);
            entity.Property(e => e.UserId).HasMaxLength(36);

            entity.HasOne(d => d.User).WithMany(p => p.News)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_News_User");
        });

        modelBuilder.Entity<Operator>(entity =>
        {
            entity.ToTable("Operator");

            entity.Property(e => e.OperatorId).ValueGeneratedOnAdd();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<PaymentMethod>(entity =>
        {
            entity.ToTable("PaymentMethod");

            entity.Property(e => e.PaymentMethodId).ValueGeneratedOnAdd();
            entity.Property(e => e.Name).HasMaxLength(20);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Product");

            entity.Property(e => e.ProductId).HasMaxLength(36);
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<RefrashToken>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("RefrashToken");

            entity.Property(e => e.StringToken)
                .HasMaxLength(44)
                .IsFixedLength();
            entity.Property(e => e.TokenId).ValueGeneratedOnAdd();
            entity.Property(e => e.UserId).HasMaxLength(36);

            entity.HasOne(d => d.User).WithMany()
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_RefrashToken_User");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Role");

            entity.Property(e => e.RoleId).ValueGeneratedOnAdd();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Sale>(entity =>
        {
            entity.ToTable("Sale");

            entity.Property(e => e.MachineId).HasMaxLength(36);
            entity.Property(e => e.ProductId).HasMaxLength(36);
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(19, 4)");

            entity.HasOne(d => d.Machine).WithMany(p => p.Sales)
                .HasForeignKey(d => d.MachineId)
                .HasConstraintName("FK_Sale_VendingMachine");

            entity.HasOne(d => d.PaymentMethod).WithMany(p => p.Sales)
                .HasForeignKey(d => d.PaymentMethodId)
                .HasConstraintName("FK_Sale_PaymentMethod");

            entity.HasOne(d => d.Product).WithMany(p => p.Sales)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_Sale_Product");
        });

        modelBuilder.Entity<ServicePriority>(entity =>
        {
            entity.HasKey(e => e.PriorityId);

            entity.ToTable("ServicePriority");

            entity.Property(e => e.PriorityId).ValueGeneratedOnAdd();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.ToTable("Status");

            entity.Property(e => e.StatusId).ValueGeneratedOnAdd();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Template>(entity =>
        {
            entity.ToTable("Template");

            entity.Property(e => e.TemplateId).ValueGeneratedOnAdd();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.Property(e => e.UserId).HasMaxLength(36);
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.HashedPassword)
                .HasMaxLength(60)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.MiddleName).HasMaxLength(100);
            entity.Property(e => e.Phone)
                .HasMaxLength(11)
                .IsUnicode(false);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_User_Role");
        });

        modelBuilder.Entity<VendingMachine>(entity =>
        {
            entity.HasKey(e => e.MachineId);

            entity.ToTable("VendingMachine", tb =>
                {
                    tb.HasTrigger("TRG_Test");
                    tb.HasTrigger("TRG_VendingMachine_LastMaintenanceDate");
                });

            entity.HasIndex(e => e.SerialNumber, "UQ_VendingMachine_SerialNumber").IsUnique();

            entity.Property(e => e.MachineId).HasMaxLength(36);
            entity.Property(e => e.KitOnlineId).HasMaxLength(10);
            entity.Property(e => e.Location).HasMaxLength(150);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Notes).HasMaxLength(255);
            entity.Property(e => e.RfidCashCollection).HasMaxLength(12);
            entity.Property(e => e.RfidLoading).HasMaxLength(12);
            entity.Property(e => e.RfidService).HasMaxLength(13);
            entity.Property(e => e.Status).HasMaxLength(25);
            entity.Property(e => e.TotalIncome).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.UserId).HasMaxLength(36);

            entity.HasOne(d => d.Company).WithMany(p => p.VendingMachines)
                .HasForeignKey(d => d.CompanyId)
                .HasConstraintName("FK_VendingMachine_Company");

            entity.HasOne(d => d.CriticalThresholdTemplate).WithMany(p => p.VendingMachineCriticalThresholdTemplates)
                .HasForeignKey(d => d.CriticalThresholdTemplateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VendingMachine_Template");

            entity.HasOne(d => d.Model).WithMany(p => p.VendingMachines)
                .HasForeignKey(d => d.ModelId)
                .HasConstraintName("FK_VendingMachine_Model");

            entity.HasOne(d => d.NotificationTemplate).WithMany(p => p.VendingMachineNotificationTemplates)
                .HasForeignKey(d => d.NotificationTemplateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VendingMachine_Template1");

            entity.HasOne(d => d.Place).WithMany(p => p.VendingMachines)
                .HasForeignKey(d => d.PlaceId)
                .HasConstraintName("FK_VendingMachine_MachinePlace");

            entity.HasOne(d => d.Priority).WithMany(p => p.VendingMachines)
                .HasForeignKey(d => d.PriorityId)
                .HasConstraintName("FK_VendingMachine_ServicePriority");

            entity.HasOne(d => d.StatusNavigation).WithMany(p => p.VendingMachines)
                .HasForeignKey(d => d.StatusId)
                .HasConstraintName("FK_VendingMachine_Status");

            entity.HasOne(d => d.User).WithMany(p => p.VendingMachines)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_VendingMachine_User");

            entity.HasOne(d => d.WorkMode).WithMany(p => p.VendingMachines)
                .HasForeignKey(d => d.WorkModeId)
                .HasConstraintName("FK_VendingMachine_WorkMode");
        });

        modelBuilder.Entity<WorkDescription>(entity =>
        {
            entity.ToTable("WorkDescription");

            entity.Property(e => e.WorkDescriptionId).ValueGeneratedOnAdd();
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<WorkMode>(entity =>
        {
            entity.ToTable("WorkMode");

            entity.Property(e => e.WorkModeId).ValueGeneratedOnAdd();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Worker>(entity =>
        {
            entity.ToTable("Worker");

            entity.Property(e => e.UserId).HasMaxLength(36);

            entity.HasOne(d => d.User).WithMany(p => p.Workers)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Worker_User");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
