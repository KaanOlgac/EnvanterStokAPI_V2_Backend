using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace EnvanterStokAPI.Models;

public partial class EnvanterContext : DbContext
{
    public EnvanterContext()
    {
    }

    public EnvanterContext(DbContextOptions<EnvanterContext> options)
        : base(options)
    {
    }

    public virtual DbSet<StkCategory> StkCategories { get; set; }

    public virtual DbSet<StkInventoryRequest> StkInventoryRequests { get; set; }

    public virtual DbSet<StkProduct> StkProducts { get; set; }

    public virtual DbSet<StkRole> StkRoles { get; set; }

    public virtual DbSet<StkStock> StkStocks { get; set; }

    public virtual DbSet<StkStockMovement> StkStockMovements { get; set; }

    public virtual DbSet<StkUser> StkUsers { get; set; }

   
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StkCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("stk_categories_pkey");

            entity.ToTable("stk_categories");

            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(100)
                .HasColumnName("category_name");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .HasColumnName("description");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
        });

        modelBuilder.Entity<StkInventoryRequest>(entity =>
        {
            entity.HasKey(e => e.RequestId).HasName("stk_inventory_request_pkey");

            entity.ToTable("stk_inventory_request");

            entity.Property(e => e.RequestId).HasColumnName("request_id");
            entity.Property(e => e.ApprovedByUserId).HasColumnName("approved_by_user_id");
            entity.Property(e => e.ApprovedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("approved_date");
            entity.Property(e => e.ApprovedQuantity).HasColumnName("approved_quantity");
            entity.Property(e => e.DeliveredByUserId).HasColumnName("delivered_by_user_id");
            entity.Property(e => e.DeliveredDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("delivered_date");
            entity.Property(e => e.DeliveredQuantity).HasColumnName("delivered_quantity");
            entity.Property(e => e.Note)
                .HasMaxLength(20)
                .HasColumnName("note");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.RemainingQuantity).HasColumnName("remaining_quantity");
            entity.Property(e => e.RequestDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("request_date");
            entity.Property(e => e.RequestedByUserId).HasColumnName("requested_by_user_id");
            entity.Property(e => e.RequestedQuantity).HasColumnName("requested_quantity");
            entity.Property(e => e.SecurityApprovalStatus)
                .HasMaxLength(20)
                .HasColumnName("security_approval_status");
            entity.Property(e => e.StatusCode)
                .HasMaxLength(1)
                .HasColumnName("status_code");
        });

        modelBuilder.Entity<StkProduct>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("stk_products_pkey");

            entity.ToTable("stk_products");

            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.ClearanceLevel).HasColumnName("clearance_level");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.MinStockLevel).HasColumnName("min_stock_level");
            entity.Property(e => e.ProductCode)
                .HasMaxLength(50)
                .HasColumnName("product_code");
            entity.Property(e => e.ProductName)
                .HasMaxLength(150)
                .HasColumnName("product_name");
            entity.Property(e => e.UnitName)
                .HasMaxLength(20)
                .HasColumnName("unit_name");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<StkRole>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("stk_roles_pkey");

            entity.ToTable("stk_roles");

            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.ClearanceLevel)
                .HasDefaultValue(1)
                .HasColumnName("clearance_level");
            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .HasColumnName("description");
            entity.Property(e => e.IsAvtive).HasColumnName("is_avtive");
            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .HasColumnName("role_name");
        });

        modelBuilder.Entity<StkStock>(entity =>
        {
            entity.HasKey(e => e.StockId).HasName("stk_stocks_pkey");

            entity.ToTable("stk_stocks");

            entity.Property(e => e.StockId).HasColumnName("stock_id");
            entity.Property(e => e.CurrentQuantity).HasColumnName("current_quantity");
            entity.Property(e => e.DeliveredQuantity).HasColumnName("delivered_quantity");
            entity.Property(e => e.NetQuantity).HasColumnName("net_quantity");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.ReservedQuantity).HasColumnName("reserved_quantity");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<StkStockMovement>(entity =>
        {
            entity.HasKey(e => e.MovementId).HasName("stk_stock_movements_pkey");

            entity.ToTable("stk_stock_movements");

            entity.Property(e => e.MovementId).HasColumnName("movement_id");
            entity.Property(e => e.MovementDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("movement_date");
            entity.Property(e => e.MovementType)
                .HasMaxLength(1)
                .HasColumnName("movement_type");
            entity.Property(e => e.Note)
                .HasMaxLength(250)
                .HasColumnName("note");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.RequestId).HasColumnName("request_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
        });

        modelBuilder.Entity<StkUser>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("users_pkey");

            entity.ToTable("stk_users");

            entity.Property(e => e.UserId)
                .HasDefaultValueSql("nextval('users_user_id_seq'::regclass)")
                .HasColumnName("user_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.EmailAddress)
                .HasMaxLength(100)
                .HasColumnName("email_address");
            entity.Property(e => e.FullName)
                .HasMaxLength(100)
                .HasColumnName("full_name");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.PasswordHash).HasColumnName("password_hash");
            entity.Property(e => e.PasswordSalt).HasColumnName("password_salt");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}