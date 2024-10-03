using FurniFusion.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FurniFusion.Data;

public partial class FurniFusionDbContext : IdentityDbContext<User>
{
    public FurniFusionDbContext()
    {
    }

    public FurniFusionDbContext(DbContextOptions<FurniFusionDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Carrier> Carriers { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<CategoryChangeLog> CategoryChangeLogs { get; set; }

    public virtual DbSet<Discount> Discounts { get; set; }

    public virtual DbSet<DiscountChangeLog> DiscountChangeLogs { get; set; }

    public virtual DbSet<DiscountUnit> DiscountUnits { get; set; }

    public virtual DbSet<Inventory> Inventories { get; set; }

    public virtual DbSet<InventoryProduct> InventoryProducts { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<OrderStatus> OrderStatuses { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }

    public virtual DbSet<PaymentStatus> PaymentStatuses { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductChangeLog> ProductChangeLogs { get; set; }

    public virtual DbSet<ProductReview> ProductReviews { get; set; }

    public virtual DbSet<Shipping> Shippings { get; set; }

    public virtual DbSet<ShippingStatus> ShippingStatuses { get; set; }

    public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; }

    public virtual DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserAddress> UserAddresses { get; set; }

    public virtual DbSet<UserPaymentInfo> UserPaymentInfos { get; set; }

    public virtual DbSet<UserPhoneNumber> UserPhoneNumbers { get; set; }

    public virtual DbSet<Wishlist> Wishlists { get; set; }

    public virtual DbSet<WishlistItem> WishlistItems { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseNpgsql("Host = localhost; Database = FurniFusionDb; Username = postgres; password = Joker@2003");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


        List<IdentityRole> roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Name = "superAdmin",
                    NormalizedName = "SUPERADMIN"
                },
                new IdentityRole
                {
                    Name = "user",
                    NormalizedName = "USER"
                }
            };

        modelBuilder.Entity<IdentityRole>().HasData(roles);


        modelBuilder.Entity<Carrier>(entity =>
        {
            entity.HasKey(e => e.CarrierId).HasName("Carrier_pkey");

            entity.ToTable("Carrier");

            entity.HasIndex(e => e.CarrierName, "Carrier_carrier_name_key").IsUnique();

            entity.HasIndex(e => e.Email, "Carrier_email_key").IsUnique();

            entity.HasIndex(e => e.Phone, "Carrier_phone_key").IsUnique();

            entity.Property(e => e.CarrierId).HasColumnName("carrier_id");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .HasColumnName("address");
            entity.Property(e => e.CarrierName)
                .HasMaxLength(100)
                .HasColumnName("carrier_name");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(false)
                .HasColumnName("is_active");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
            entity.Property(e => e.ShippingApi)
                .HasMaxLength(255)
                .HasColumnName("shipping_api");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.Website)
                .HasMaxLength(255)
                .HasColumnName("website");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("Category_pkey");

            entity.ToTable("Category");

            entity.HasIndex(e => e.CategoryName, "Category_category_name_key").IsUnique();

            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(100)
                .HasColumnName("category_name");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.CategoryCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("Category_created_by_fkey");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.CategoryUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("Category_updated_by_fkey");
        });

        modelBuilder.Entity<CategoryChangeLog>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("Category_Change_Log_pkey");

            entity.ToTable("Category_Change_Log");

            entity.Property(e => e.LogId).HasColumnName("log_id");
            entity.Property(e => e.ActionType)
                .HasMaxLength(10)
                .HasDefaultValueSql("'UPDATE'::character varying")
                .HasColumnName("action_type");
            entity.Property(e => e.ChangedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("changed_at");
            entity.Property(e => e.NewCategoryName)
                .HasMaxLength(100)
                .HasColumnName("new_category_name");
            entity.Property(e => e.OldCategoryName)
                .HasMaxLength(100)
                .HasColumnName("old_category_name");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.CategoryChangeLogs)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("Category_Change_Log_updated_by_fkey");
        });

        modelBuilder.Entity<Discount>(entity =>
        {
            entity.HasKey(e => e.DiscountId).HasName("Discount_pkey");

            entity.ToTable("Discount");

            entity.HasIndex(e => e.DiscountCode, "Discount_discount_code_key").IsUnique();

            entity.Property(e => e.DiscountId).HasColumnName("discount_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DiscountCode)
                .HasMaxLength(100)
                .HasColumnName("discount_code");
            entity.Property(e => e.DiscountUnitId).HasColumnName("discount_unit_id");
            entity.Property(e => e.DiscountValue)
                .HasPrecision(10, 2)
                .HasColumnName("discount_value");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(false)
                .HasColumnName("is_active");
            entity.Property(e => e.MaxAmount)
                .HasPrecision(10, 2)
                .HasColumnName("max_amount");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.Property(e => e.ValidFrom).HasColumnName("valid_from");
            entity.Property(e => e.ValidTo).HasColumnName("valid_to");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.DiscountCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("Discount_created_by_fkey");

            entity.HasOne(d => d.DiscountUnit).WithMany(p => p.Discounts)
                .HasForeignKey(d => d.DiscountUnitId)
                .HasConstraintName("Discount_discount_unit_id_fkey");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.DiscountUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("Discount_updated_by_fkey");
        });

        modelBuilder.Entity<DiscountChangeLog>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("Discount_Change_Log_pkey");

            entity.ToTable("Discount_Change_Log");

            entity.Property(e => e.LogId).HasColumnName("log_id");
            entity.Property(e => e.ActionType)
                .HasMaxLength(10)
                .HasDefaultValueSql("'UPDATE'::character varying")
                .HasColumnName("action_type");
            entity.Property(e => e.ChangedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("changed_at");
            entity.Property(e => e.DiscountId).HasColumnName("discount_id");
            entity.Property(e => e.NewDiscountCode)
                .HasMaxLength(100)
                .HasColumnName("new_discount_code");
            entity.Property(e => e.NewDiscountUnitId).HasColumnName("new_discount_unit_id");
            entity.Property(e => e.NewDiscountValue)
                .HasPrecision(10, 2)
                .HasColumnName("new_discount_value");
            entity.Property(e => e.OldDiscountCode)
                .HasMaxLength(100)
                .HasColumnName("old_discount_code");
            entity.Property(e => e.OldDiscountUnitId).HasColumnName("old_discount_unit_id");
            entity.Property(e => e.OldDiscountValue)
                .HasPrecision(10, 2)
                .HasColumnName("old_discount_value");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.Discount).WithMany(p => p.DiscountChangeLogs)
                .HasForeignKey(d => d.DiscountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Discount_Change_Log_discount_id_fkey");

            entity.HasOne(d => d.NewDiscountUnit).WithMany(p => p.DiscountChangeLogNewDiscountUnits)
                .HasForeignKey(d => d.NewDiscountUnitId)
                .HasConstraintName("Discount_Change_Log_new_discount_unit_id_fkey");

            entity.HasOne(d => d.OldDiscountUnit).WithMany(p => p.DiscountChangeLogOldDiscountUnits)
                .HasForeignKey(d => d.OldDiscountUnitId)
                .HasConstraintName("Discount_Change_Log_old_discount_unit_id_fkey");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.DiscountChangeLogs)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("Discount_Change_Log_updated_by_fkey");
        });

        modelBuilder.Entity<DiscountUnit>(entity =>
        {
            entity.HasKey(e => e.UnitId).HasName("Discount_Unit_pkey");

            entity.ToTable("Discount_Unit");

            entity.HasIndex(e => e.UnitName, "Discount_Unit_unit_name_key").IsUnique();

            entity.Property(e => e.UnitId).HasColumnName("unit_id");
            entity.Property(e => e.UnitName)
                .HasMaxLength(10)
                .HasColumnName("unit_name");
        });

        modelBuilder.Entity<Inventory>(entity =>
        {
            entity.HasKey(e => e.InventoryId).HasName("Inventory_pkey");

            entity.ToTable("Inventory");

            entity.Property(e => e.InventoryId).HasColumnName("inventory_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(false)
                .HasColumnName("is_active");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.WarehouseLocation)
                .HasMaxLength(255)
                .HasColumnName("warehouse_location");
        });

        modelBuilder.Entity<InventoryProduct>(entity =>
        {
            entity.HasKey(e => new { e.InventoryId, e.ProductId }).HasName("Inventory_Products_pkey");

            entity.ToTable("Inventory_Products");

            entity.Property(e => e.InventoryId).HasColumnName("inventory_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Quantity)
                .HasDefaultValue(0)
                .HasColumnName("quantity");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Inventory).WithMany(p => p.InventoryProducts)
                .HasForeignKey(d => d.InventoryId)
                .HasConstraintName("Inventory_Products_inventory_id_fkey");

            entity.HasOne(d => d.Product).WithMany(p => p.InventoryProducts)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("Inventory_Products_product_id_fkey");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("Order_pkey");

            entity.ToTable("Order");

            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.AddressToDeliver)
                .HasMaxLength(255)
                .HasColumnName("address_to_deliver");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.DiscountId).HasColumnName("discount_id");
            entity.Property(e => e.PaymentId).HasColumnName("payment_id");
            entity.Property(e => e.ShippingId).HasColumnName("shipping_id");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.TotalPrice)
                .HasPrecision(10, 2)
                .HasColumnName("total_price");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Discount).WithMany(p => p.Orders)
                .HasForeignKey(d => d.DiscountId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("Order_discount_id_fkey");

            entity.HasOne(d => d.Payment).WithMany(p => p.Orders)
                .HasForeignKey(d => d.PaymentId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("Order_payment_id_fkey");

            entity.HasOne(d => d.Shipping).WithMany(p => p.Orders)
                .HasForeignKey(d => d.ShippingId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("Order_shipping_id_fkey");

            entity.HasOne(d => d.StatusNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.Status)
                .HasConstraintName("Order_status_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("Order_user_id_fkey");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.ItemId).HasName("Order_Item_pkey");

            entity.ToTable("Order_Item");

            entity.Property(e => e.ItemId).HasColumnName("item_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.Price)
                .HasPrecision(10, 2)
                .HasColumnName("price");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("Order_Item_order_id_fkey");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("Order_Item_product_id_fkey");
        });

        modelBuilder.Entity<OrderStatus>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("Order_Status_pkey");

            entity.ToTable("Order_Status");

            entity.HasIndex(e => e.StatusName, "Order_Status_status_name_key").IsUnique();

            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.StatusName)
                .HasMaxLength(50)
                .HasColumnName("status_name");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("Payment_pkey");

            entity.ToTable("Payment");

            entity.HasIndex(e => e.TransactionId, "Payment_transaction_id_key").IsUnique();

            entity.Property(e => e.PaymentId).HasColumnName("payment_id");
            entity.Property(e => e.Amount)
                .HasPrecision(10, 2)
                .HasColumnName("amount");
            entity.Property(e => e.Date)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date");
            entity.Property(e => e.PaymentMethod).HasColumnName("payment_method");
            entity.Property(e => e.PaymentStatusId).HasColumnName("payment_status_id");
            entity.Property(e => e.TransactionId)
                .HasMaxLength(255)
                .HasColumnName("transaction_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.PaymentMethodNavigation).WithMany(p => p.Payments)
                .HasForeignKey(d => d.PaymentMethod)
                .HasConstraintName("Payment_payment_method_fkey");

            entity.HasOne(d => d.PaymentStatus).WithMany(p => p.Payments)
                .HasForeignKey(d => d.PaymentStatusId)
                .HasConstraintName("Payment_payment_status_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Payments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("Payment_user_id_fkey");
        });

        modelBuilder.Entity<PaymentMethod>(entity =>
        {
            entity.HasKey(e => e.MethodId).HasName("Payment_Method_pkey");

            entity.ToTable("Payment_Method");

            entity.HasIndex(e => e.MethodName, "Payment_Method_method_name_key").IsUnique();

            entity.Property(e => e.MethodId).HasColumnName("method_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(false)
                .HasColumnName("is_active");
            entity.Property(e => e.MethodName)
                .HasMaxLength(100)
                .HasColumnName("method_name");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<PaymentStatus>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("Payment_Status_pkey");

            entity.ToTable("Payment_Status");

            entity.HasIndex(e => e.StatusName, "Payment_Status_status_name_key").IsUnique();

            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.StatusName)
                .HasMaxLength(50)
                .HasColumnName("status_name");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("Product_pkey");

            entity.ToTable("Product");

            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.AverageRating)
                .HasPrecision(1, 2)
                .HasDefaultValueSql("0.00")
                .HasColumnName("average_rating");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.Colors)
                .HasDefaultValueSql("'{}'::text[]")
                .HasColumnName("colors");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Dimensions)
                .HasColumnType("jsonb")
                .HasColumnName("dimensions");
            entity.Property(e => e.DiscountId).HasColumnName("discount_id");
            entity.Property(e => e.ImageUrls)
                .HasDefaultValueSql("'{}'::text[]")
                .HasColumnName("image_urls");
            entity.Property(e => e.IsAvailable)
                .HasDefaultValue(false)
                .HasColumnName("is_available");
            entity.Property(e => e.Price)
                .HasPrecision(10, 2)
                .HasColumnName("price");
            entity.Property(e => e.ProductName)
                .HasMaxLength(255)
                .HasColumnName("product_name");
            entity.Property(e => e.StockQuantity)
                .HasDefaultValue(0)
                .HasColumnName("stock_quantity");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.Property(e => e.Weight)
                .HasPrecision(10, 2)
                .HasDefaultValueSql("0.00")
                .HasColumnName("weight");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("Product_category_id_fkey");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ProductCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("Product_created_by_fkey");

            entity.HasOne(d => d.Discount).WithMany(p => p.Products)
                .HasForeignKey(d => d.DiscountId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("Product_discount_id_fkey");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.ProductUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("Product_updated_by_fkey");
        });

        modelBuilder.Entity<ProductChangeLog>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("Product_Change_Log_pkey");

            entity.ToTable("Product_Change_Log");

            entity.Property(e => e.LogId).HasColumnName("log_id");
            entity.Property(e => e.ActionType)
                .HasMaxLength(10)
                .HasDefaultValueSql("'UPDATE'::character varying")
                .HasColumnName("action_type");
            entity.Property(e => e.ChangedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("changed_at");
            entity.Property(e => e.NewColors).HasColumnName("new_colors");
            entity.Property(e => e.NewDescription).HasColumnName("new_description");
            entity.Property(e => e.NewDimensions)
                .HasColumnType("jsonb")
                .HasColumnName("new_dimensions");
            entity.Property(e => e.NewImageUrls).HasColumnName("new_image_urls");
            entity.Property(e => e.NewIsAvailable).HasColumnName("new_is_available");
            entity.Property(e => e.NewPrice)
                .HasPrecision(10, 2)
                .HasColumnName("new_price");
            entity.Property(e => e.NewProductName)
                .HasMaxLength(255)
                .HasColumnName("new_product_name");
            entity.Property(e => e.NewStockQuantity).HasColumnName("new_stock_quantity");
            entity.Property(e => e.NewWeight)
                .HasPrecision(10, 2)
                .HasColumnName("new_weight");
            entity.Property(e => e.OldColors).HasColumnName("old_colors");
            entity.Property(e => e.OldDescription).HasColumnName("old_description");
            entity.Property(e => e.OldDimensions)
                .HasColumnType("jsonb")
                .HasColumnName("old_dimensions");
            entity.Property(e => e.OldImageUrls).HasColumnName("old_image_urls");
            entity.Property(e => e.OldIsAvailable).HasColumnName("old_is_available");
            entity.Property(e => e.OldPrice)
                .HasPrecision(10, 2)
                .HasColumnName("old_price");
            entity.Property(e => e.OldProductName)
                .HasMaxLength(255)
                .HasColumnName("old_product_name");
            entity.Property(e => e.OldStockQuantity).HasColumnName("old_stock_quantity");
            entity.Property(e => e.OldWeight)
                .HasPrecision(10, 2)
                .HasColumnName("old_weight");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductChangeLogs)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("Product_Change_Log_product_id_fkey");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.ProductChangeLogs)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("Product_Change_Log_updated_by_fkey");
        });

        modelBuilder.Entity<ProductReview>(entity =>
        {
            entity.HasKey(e => e.ReviewId).HasName("Product_Review_pkey");

            entity.ToTable("Product_Review");

            entity.Property(e => e.ReviewId).HasColumnName("review_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.ReviewText).HasColumnName("review_text");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductReviews)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("Product_Review_product_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.ProductReviews)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("Product_Review_user_id_fkey");
        });

        modelBuilder.Entity<Shipping>(entity =>
        {
            entity.HasKey(e => e.ShippingId).HasName("Shipping_pkey");

            entity.ToTable("Shipping");

            entity.Property(e => e.ShippingId).HasColumnName("shipping_id");
            entity.Property(e => e.CarrierId).HasColumnName("carrier_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.EstimatedDeliveryDate).HasColumnName("estimated_delivery_date");
            entity.Property(e => e.ShippingCost)
                .HasPrecision(10, 2)
                .HasColumnName("shipping_cost");
            entity.Property(e => e.ShippingDate).HasColumnName("shipping_date");
            entity.Property(e => e.ShippingMethod)
                .HasMaxLength(100)
                .HasColumnName("shipping_method");
            entity.Property(e => e.ShippingStatusId).HasColumnName("shipping_status_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Carrier).WithMany(p => p.Shippings)
                .HasForeignKey(d => d.CarrierId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("Shipping_carrier_id_fkey");

            entity.HasOne(d => d.ShippingStatus).WithMany(p => p.Shippings)
                .HasForeignKey(d => d.ShippingStatusId)
                .HasConstraintName("Shipping_shipping_status_id_fkey");
        });

        modelBuilder.Entity<ShippingStatus>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("Shipping_Status_pkey");

            entity.ToTable("Shipping_Status");

            entity.HasIndex(e => e.StatusName, "Shipping_Status_status_name_key").IsUnique();

            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.StatusName)
                .HasMaxLength(50)
                .HasColumnName("status_name");
        });

        modelBuilder.Entity<ShoppingCart>(entity =>
        {
            entity.HasKey(e => e.CartId).HasName("Shopping_Cart_pkey");

            entity.ToTable("Shopping_Cart");

            entity.HasIndex(e => e.UserId, "Shopping_Cart_user_id_key").IsUnique();

            entity.Property(e => e.CartId).HasColumnName("cart_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithOne(p => p.ShoppingCart)
                .HasForeignKey<ShoppingCart>(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("Shopping_Cart_user_id_fkey");
        });

        modelBuilder.Entity<ShoppingCartItem>(entity =>
        {
            entity.HasKey(e => new { e.CartId, e.ProductId }).HasName("Shopping_Cart_Items_pkey");

            entity.ToTable("Shopping_Cart_Items");

            entity.Property(e => e.CartId).HasColumnName("cart_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Quantity)
                .HasDefaultValue(1)
                .HasColumnName("quantity");

            entity.HasOne(d => d.Cart).WithMany(p => p.ShoppingCartItems)
                .HasForeignKey(d => d.CartId)
                .HasConstraintName("Shopping_Cart_Items_cart_id_fkey");

            entity.HasOne(d => d.Product).WithMany(p => p.ShoppingCartItems)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("Shopping_Cart_Items_product_id_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("User_pkey");

            entity.ToTable("User");


            entity.HasIndex(e => e.Email, "User_email_key").IsUnique();

            //entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .HasColumnName("first_name");
            entity.Property(e => e.ImageUrl).HasColumnName("image_url");
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .HasColumnName("last_name");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<UserAddress>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.IsPrimaryAddress }).HasName("User_Address_pkey");

            entity.ToTable("User_Address");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.IsPrimaryAddress)
                .HasDefaultValue(false)
                .HasColumnName("is_primary_address");
            entity.Property(e => e.City)
                .HasMaxLength(100)
                .HasColumnName("city");
            entity.Property(e => e.Country)
                .HasMaxLength(100)
                .HasColumnName("country");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.PostalCode)
                .HasMaxLength(20)
                .HasColumnName("postal_code");
            entity.Property(e => e.State)
                .HasMaxLength(100)
                .HasColumnName("state");
            entity.Property(e => e.Street)
                .HasMaxLength(255)
                .HasColumnName("street");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.User).WithMany(p => p.UserAddresses)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("User_Address_user_id_fkey");
        });

        modelBuilder.Entity<UserPaymentInfo>(entity =>
        {
            entity.HasKey(e => e.PaymentInfoId).HasName("User_Payment_Info_pkey");

            entity.ToTable("User_Payment_Info");

            entity.Property(e => e.PaymentInfoId).HasColumnName("payment_info_id");
            entity.Property(e => e.BillingAddress)
                .HasMaxLength(255)
                .HasColumnName("billing_address");
            entity.Property(e => e.CardNumber)
                .HasMaxLength(16)
                .HasColumnName("card_number");
            entity.Property(e => e.CardType)
                .HasMaxLength(50)
                .HasColumnName("card_type");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.ExpiryDate).HasColumnName("expiry_date");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.UserPaymentInfos)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("User_Payment_Info_user_id_fkey");
        });

        modelBuilder.Entity<UserPhoneNumber>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.PhoneNumber }).HasName("User_Phone_Number_pkey");

            entity.ToTable("User_Phone_Number");

            entity.HasIndex(e => e.PhoneNumber, "User_Phone_Number_phone_number_key").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(15)
                .HasColumnName("phone_number");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.User).WithMany(p => p.UserPhoneNumbers)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("User_Phone_Number_user_id_fkey");
        });

        modelBuilder.Entity<Wishlist>(entity =>
        {
            entity.HasKey(e => e.WishlistId).HasName("Wishlist_pkey");

            entity.ToTable("Wishlist");

            entity.HasIndex(e => e.UserId, "Wishlist_user_id_key").IsUnique();

            entity.Property(e => e.WishlistId).HasColumnName("wishlist_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithOne(p => p.Wishlist)
                .HasForeignKey<Wishlist>(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("Wishlist_user_id_fkey");
        });

        modelBuilder.Entity<WishlistItem>(entity =>
        {
            entity.HasKey(e => new { e.WishlistId, e.ProductId }).HasName("Wishlist_Items_pkey");

            entity.ToTable("Wishlist_Items");

            entity.Property(e => e.WishlistId).HasColumnName("wishlist_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");

            entity.HasOne(d => d.Product).WithMany(p => p.WishlistItems)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("Wishlist_Items_product_id_fkey");

            entity.HasOne(d => d.Wishlist).WithMany(p => p.WishlistItems)
                .HasForeignKey(d => d.WishlistId)
                .HasConstraintName("Wishlist_Items_wishlist_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
