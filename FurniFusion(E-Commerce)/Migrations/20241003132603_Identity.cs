using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FurniFusion.Migrations
{
    /// <inheritdoc />
    public partial class Identity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Carrier",
                columns: table => new
                {
                    carrier_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    carrier_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    website = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    address = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    shipping_api = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("Carrier_pkey", x => x.carrier_id);
                });

            migrationBuilder.CreateTable(
                name: "Discount_Unit",
                columns: table => new
                {
                    unit_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    unit_name = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Discount_Unit_pkey", x => x.unit_id);
                });

            migrationBuilder.CreateTable(
                name: "Inventory",
                columns: table => new
                {
                    inventory_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    warehouse_location = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("Inventory_pkey", x => x.inventory_id);
                });

            migrationBuilder.CreateTable(
                name: "Order_Status",
                columns: table => new
                {
                    status_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    status_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Order_Status_pkey", x => x.status_id);
                });

            migrationBuilder.CreateTable(
                name: "Payment_Method",
                columns: table => new
                {
                    method_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    method_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("Payment_Method_pkey", x => x.method_id);
                });

            migrationBuilder.CreateTable(
                name: "Payment_Status",
                columns: table => new
                {
                    status_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    status_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Payment_Status_pkey", x => x.status_id);
                });

            migrationBuilder.CreateTable(
                name: "Shipping_Status",
                columns: table => new
                {
                    status_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    status_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Shipping_Status_pkey", x => x.status_id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    first_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    last_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    image_url = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("User_pkey", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Shipping",
                columns: table => new
                {
                    shipping_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    shipping_method = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    shipping_cost = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    shipping_date = table.Column<DateOnly>(type: "date", nullable: false),
                    estimated_delivery_date = table.Column<DateOnly>(type: "date", nullable: false),
                    shipping_status_id = table.Column<int>(type: "integer", nullable: true),
                    carrier_id = table.Column<int>(type: "integer", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("Shipping_pkey", x => x.shipping_id);
                    table.ForeignKey(
                        name: "Shipping_carrier_id_fkey",
                        column: x => x.carrier_id,
                        principalTable: "Carrier",
                        principalColumn: "carrier_id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "Shipping_shipping_status_id_fkey",
                        column: x => x.shipping_status_id,
                        principalTable: "Shipping_Status",
                        principalColumn: "status_id");
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    category_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    category_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("Category_pkey", x => x.category_id);
                    table.ForeignKey(
                        name: "Category_created_by_fkey",
                        column: x => x.created_by,
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "Category_updated_by_fkey",
                        column: x => x.updated_by,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Category_Change_Log",
                columns: table => new
                {
                    log_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    old_category_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    new_category_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true),
                    changed_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    action_type = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true, defaultValueSql: "'UPDATE'::character varying")
                },
                constraints: table =>
                {
                    table.PrimaryKey("Category_Change_Log_pkey", x => x.log_id);
                    table.ForeignKey(
                        name: "Category_Change_Log_updated_by_fkey",
                        column: x => x.updated_by,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Discount",
                columns: table => new
                {
                    discount_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    discount_code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    discount_value = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    discount_unit_id = table.Column<int>(type: "integer", nullable: true),
                    valid_from = table.Column<DateOnly>(type: "date", nullable: false),
                    valid_to = table.Column<DateOnly>(type: "date", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    max_amount = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Discount_pkey", x => x.discount_id);
                    table.ForeignKey(
                        name: "Discount_created_by_fkey",
                        column: x => x.created_by,
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "Discount_discount_unit_id_fkey",
                        column: x => x.discount_unit_id,
                        principalTable: "Discount_Unit",
                        principalColumn: "unit_id");
                    table.ForeignKey(
                        name: "Discount_updated_by_fkey",
                        column: x => x.updated_by,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Payment",
                columns: table => new
                {
                    payment_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    amount = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    payment_status_id = table.Column<int>(type: "integer", nullable: true),
                    date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    payment_method = table.Column<int>(type: "integer", nullable: true),
                    transaction_id = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    user_id = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Payment_pkey", x => x.payment_id);
                    table.ForeignKey(
                        name: "Payment_payment_method_fkey",
                        column: x => x.payment_method,
                        principalTable: "Payment_Method",
                        principalColumn: "method_id");
                    table.ForeignKey(
                        name: "Payment_payment_status_id_fkey",
                        column: x => x.payment_status_id,
                        principalTable: "Payment_Status",
                        principalColumn: "status_id");
                    table.ForeignKey(
                        name: "Payment_user_id_fkey",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Shopping_Cart",
                columns: table => new
                {
                    cart_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<string>(type: "text", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("Shopping_Cart_pkey", x => x.cart_id);
                    table.ForeignKey(
                        name: "Shopping_Cart_user_id_fkey",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "User_Address",
                columns: table => new
                {
                    user_id = table.Column<string>(type: "text", nullable: false),
                    is_primary_address = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    country = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    city = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    state = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    street = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    postal_code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("User_Address_pkey", x => new { x.user_id, x.is_primary_address });
                    table.ForeignKey(
                        name: "User_Address_user_id_fkey",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "User_Payment_Info",
                columns: table => new
                {
                    payment_info_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<string>(type: "text", nullable: true),
                    card_number = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    card_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    expiry_date = table.Column<DateOnly>(type: "date", nullable: false),
                    billing_address = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("User_Payment_Info_pkey", x => x.payment_info_id);
                    table.ForeignKey(
                        name: "User_Payment_Info_user_id_fkey",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "User_Phone_Number",
                columns: table => new
                {
                    user_id = table.Column<string>(type: "text", nullable: false),
                    phone_number = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("User_Phone_Number_pkey", x => new { x.user_id, x.phone_number });
                    table.ForeignKey(
                        name: "User_Phone_Number_user_id_fkey",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Wishlist",
                columns: table => new
                {
                    wishlist_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<string>(type: "text", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("Wishlist_pkey", x => x.wishlist_id);
                    table.ForeignKey(
                        name: "Wishlist_user_id_fkey",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Discount_Change_Log",
                columns: table => new
                {
                    log_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    discount_id = table.Column<int>(type: "integer", nullable: false),
                    old_discount_code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    new_discount_code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    old_discount_value = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    new_discount_value = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    old_discount_unit_id = table.Column<int>(type: "integer", nullable: true),
                    new_discount_unit_id = table.Column<int>(type: "integer", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true),
                    changed_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    action_type = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true, defaultValueSql: "'UPDATE'::character varying")
                },
                constraints: table =>
                {
                    table.PrimaryKey("Discount_Change_Log_pkey", x => x.log_id);
                    table.ForeignKey(
                        name: "Discount_Change_Log_discount_id_fkey",
                        column: x => x.discount_id,
                        principalTable: "Discount",
                        principalColumn: "discount_id");
                    table.ForeignKey(
                        name: "Discount_Change_Log_new_discount_unit_id_fkey",
                        column: x => x.new_discount_unit_id,
                        principalTable: "Discount_Unit",
                        principalColumn: "unit_id");
                    table.ForeignKey(
                        name: "Discount_Change_Log_old_discount_unit_id_fkey",
                        column: x => x.old_discount_unit_id,
                        principalTable: "Discount_Unit",
                        principalColumn: "unit_id");
                    table.ForeignKey(
                        name: "Discount_Change_Log_updated_by_fkey",
                        column: x => x.updated_by,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    product_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    product_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    image_urls = table.Column<List<string>>(type: "text[]", nullable: true, defaultValueSql: "'{}'::text[]"),
                    dimensions = table.Column<string>(type: "jsonb", nullable: true),
                    weight = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true, defaultValueSql: "0.00"),
                    colors = table.Column<List<string>>(type: "text[]", nullable: true, defaultValueSql: "'{}'::text[]"),
                    description = table.Column<string>(type: "text", nullable: true),
                    price = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    stock_quantity = table.Column<int>(type: "integer", nullable: true, defaultValue: 0),
                    is_available = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true),
                    discount_id = table.Column<int>(type: "integer", nullable: true),
                    category_id = table.Column<int>(type: "integer", nullable: true),
                    average_rating = table.Column<decimal>(type: "numeric(1,2)", precision: 1, scale: 2, nullable: true, defaultValueSql: "0.00")
                },
                constraints: table =>
                {
                    table.PrimaryKey("Product_pkey", x => x.product_id);
                    table.ForeignKey(
                        name: "Product_category_id_fkey",
                        column: x => x.category_id,
                        principalTable: "Category",
                        principalColumn: "category_id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "Product_created_by_fkey",
                        column: x => x.created_by,
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "Product_discount_id_fkey",
                        column: x => x.discount_id,
                        principalTable: "Discount",
                        principalColumn: "discount_id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "Product_updated_by_fkey",
                        column: x => x.updated_by,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    order_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<int>(type: "integer", nullable: true),
                    address_to_deliver = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    total_price = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    discount_id = table.Column<int>(type: "integer", nullable: true),
                    payment_id = table.Column<int>(type: "integer", nullable: true),
                    shipping_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Order_pkey", x => x.order_id);
                    table.ForeignKey(
                        name: "Order_discount_id_fkey",
                        column: x => x.discount_id,
                        principalTable: "Discount",
                        principalColumn: "discount_id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "Order_payment_id_fkey",
                        column: x => x.payment_id,
                        principalTable: "Payment",
                        principalColumn: "payment_id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "Order_shipping_id_fkey",
                        column: x => x.shipping_id,
                        principalTable: "Shipping",
                        principalColumn: "shipping_id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "Order_status_fkey",
                        column: x => x.status,
                        principalTable: "Order_Status",
                        principalColumn: "status_id");
                    table.ForeignKey(
                        name: "Order_user_id_fkey",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Inventory_Products",
                columns: table => new
                {
                    inventory_id = table.Column<int>(type: "integer", nullable: false),
                    product_id = table.Column<int>(type: "integer", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: true, defaultValue: 0),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("Inventory_Products_pkey", x => new { x.inventory_id, x.product_id });
                    table.ForeignKey(
                        name: "Inventory_Products_inventory_id_fkey",
                        column: x => x.inventory_id,
                        principalTable: "Inventory",
                        principalColumn: "inventory_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "Inventory_Products_product_id_fkey",
                        column: x => x.product_id,
                        principalTable: "Product",
                        principalColumn: "product_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Product_Change_Log",
                columns: table => new
                {
                    log_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    product_id = table.Column<int>(type: "integer", nullable: false),
                    old_product_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    new_product_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    old_image_urls = table.Column<List<string>>(type: "text[]", nullable: true),
                    new_image_urls = table.Column<List<string>>(type: "text[]", nullable: true),
                    old_dimensions = table.Column<string>(type: "jsonb", nullable: true),
                    new_dimensions = table.Column<string>(type: "jsonb", nullable: true),
                    old_weight = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    new_weight = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    old_colors = table.Column<List<string>>(type: "text[]", nullable: true),
                    new_colors = table.Column<List<string>>(type: "text[]", nullable: true),
                    old_description = table.Column<string>(type: "text", nullable: true),
                    new_description = table.Column<string>(type: "text", nullable: true),
                    old_price = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    new_price = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    old_stock_quantity = table.Column<int>(type: "integer", nullable: true),
                    new_stock_quantity = table.Column<int>(type: "integer", nullable: true),
                    old_is_available = table.Column<bool>(type: "boolean", nullable: true),
                    new_is_available = table.Column<bool>(type: "boolean", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true),
                    changed_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    action_type = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true, defaultValueSql: "'UPDATE'::character varying")
                },
                constraints: table =>
                {
                    table.PrimaryKey("Product_Change_Log_pkey", x => x.log_id);
                    table.ForeignKey(
                        name: "Product_Change_Log_product_id_fkey",
                        column: x => x.product_id,
                        principalTable: "Product",
                        principalColumn: "product_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "Product_Change_Log_updated_by_fkey",
                        column: x => x.updated_by,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Product_Review",
                columns: table => new
                {
                    review_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    product_id = table.Column<int>(type: "integer", nullable: true),
                    user_id = table.Column<string>(type: "text", nullable: true),
                    rating = table.Column<int>(type: "integer", nullable: false),
                    review_text = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("Product_Review_pkey", x => x.review_id);
                    table.ForeignKey(
                        name: "Product_Review_product_id_fkey",
                        column: x => x.product_id,
                        principalTable: "Product",
                        principalColumn: "product_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "Product_Review_user_id_fkey",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Shopping_Cart_Items",
                columns: table => new
                {
                    cart_id = table.Column<int>(type: "integer", nullable: false),
                    product_id = table.Column<int>(type: "integer", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: true, defaultValue: 1),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("Shopping_Cart_Items_pkey", x => new { x.cart_id, x.product_id });
                    table.ForeignKey(
                        name: "Shopping_Cart_Items_cart_id_fkey",
                        column: x => x.cart_id,
                        principalTable: "Shopping_Cart",
                        principalColumn: "cart_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "Shopping_Cart_Items_product_id_fkey",
                        column: x => x.product_id,
                        principalTable: "Product",
                        principalColumn: "product_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Wishlist_Items",
                columns: table => new
                {
                    wishlist_id = table.Column<int>(type: "integer", nullable: false),
                    product_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("Wishlist_Items_pkey", x => new { x.wishlist_id, x.product_id });
                    table.ForeignKey(
                        name: "Wishlist_Items_product_id_fkey",
                        column: x => x.product_id,
                        principalTable: "Product",
                        principalColumn: "product_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "Wishlist_Items_wishlist_id_fkey",
                        column: x => x.wishlist_id,
                        principalTable: "Wishlist",
                        principalColumn: "wishlist_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Order_Item",
                columns: table => new
                {
                    item_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    order_id = table.Column<int>(type: "integer", nullable: true),
                    product_id = table.Column<int>(type: "integer", nullable: true),
                    price = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("Order_Item_pkey", x => x.item_id);
                    table.ForeignKey(
                        name: "Order_Item_order_id_fkey",
                        column: x => x.order_id,
                        principalTable: "Order",
                        principalColumn: "order_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "Order_Item_product_id_fkey",
                        column: x => x.product_id,
                        principalTable: "Product",
                        principalColumn: "product_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "55f49d87-3fd8-45be-a22b-9b90611036d7", null, "user", "USER" },
                    { "ddc38528-b933-4803-bf28-0e67afe094aa", null, "superAdmin", "SUPERADMIN" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "Carrier_carrier_name_key",
                table: "Carrier",
                column: "carrier_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Carrier_email_key",
                table: "Carrier",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Carrier_phone_key",
                table: "Carrier",
                column: "phone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Category_category_name_key",
                table: "Category",
                column: "category_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Category_created_by",
                table: "Category",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_Category_updated_by",
                table: "Category",
                column: "updated_by");

            migrationBuilder.CreateIndex(
                name: "IX_Category_Change_Log_updated_by",
                table: "Category_Change_Log",
                column: "updated_by");

            migrationBuilder.CreateIndex(
                name: "Discount_discount_code_key",
                table: "Discount",
                column: "discount_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Discount_created_by",
                table: "Discount",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_Discount_discount_unit_id",
                table: "Discount",
                column: "discount_unit_id");

            migrationBuilder.CreateIndex(
                name: "IX_Discount_updated_by",
                table: "Discount",
                column: "updated_by");

            migrationBuilder.CreateIndex(
                name: "IX_Discount_Change_Log_discount_id",
                table: "Discount_Change_Log",
                column: "discount_id");

            migrationBuilder.CreateIndex(
                name: "IX_Discount_Change_Log_new_discount_unit_id",
                table: "Discount_Change_Log",
                column: "new_discount_unit_id");

            migrationBuilder.CreateIndex(
                name: "IX_Discount_Change_Log_old_discount_unit_id",
                table: "Discount_Change_Log",
                column: "old_discount_unit_id");

            migrationBuilder.CreateIndex(
                name: "IX_Discount_Change_Log_updated_by",
                table: "Discount_Change_Log",
                column: "updated_by");

            migrationBuilder.CreateIndex(
                name: "Discount_Unit_unit_name_key",
                table: "Discount_Unit",
                column: "unit_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Inventory_Products_product_id",
                table: "Inventory_Products",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_Order_discount_id",
                table: "Order",
                column: "discount_id");

            migrationBuilder.CreateIndex(
                name: "IX_Order_payment_id",
                table: "Order",
                column: "payment_id");

            migrationBuilder.CreateIndex(
                name: "IX_Order_shipping_id",
                table: "Order",
                column: "shipping_id");

            migrationBuilder.CreateIndex(
                name: "IX_Order_status",
                table: "Order",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_Order_user_id",
                table: "Order",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_Order_Item_order_id",
                table: "Order_Item",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "IX_Order_Item_product_id",
                table: "Order_Item",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "Order_Status_status_name_key",
                table: "Order_Status",
                column: "status_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payment_payment_method",
                table: "Payment",
                column: "payment_method");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_payment_status_id",
                table: "Payment",
                column: "payment_status_id");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_user_id",
                table: "Payment",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "Payment_transaction_id_key",
                table: "Payment",
                column: "transaction_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Payment_Method_method_name_key",
                table: "Payment_Method",
                column: "method_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Payment_Status_status_name_key",
                table: "Payment_Status",
                column: "status_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Product_category_id",
                table: "Product",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_Product_created_by",
                table: "Product",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_Product_discount_id",
                table: "Product",
                column: "discount_id");

            migrationBuilder.CreateIndex(
                name: "IX_Product_updated_by",
                table: "Product",
                column: "updated_by");

            migrationBuilder.CreateIndex(
                name: "IX_Product_Change_Log_product_id",
                table: "Product_Change_Log",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_Product_Change_Log_updated_by",
                table: "Product_Change_Log",
                column: "updated_by");

            migrationBuilder.CreateIndex(
                name: "IX_Product_Review_product_id",
                table: "Product_Review",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_Product_Review_user_id",
                table: "Product_Review",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_Shipping_carrier_id",
                table: "Shipping",
                column: "carrier_id");

            migrationBuilder.CreateIndex(
                name: "IX_Shipping_shipping_status_id",
                table: "Shipping",
                column: "shipping_status_id");

            migrationBuilder.CreateIndex(
                name: "Shipping_Status_status_name_key",
                table: "Shipping_Status",
                column: "status_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Shopping_Cart_user_id_key",
                table: "Shopping_Cart",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Shopping_Cart_Items_product_id",
                table: "Shopping_Cart_Items",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "User",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "User_email_key",
                table: "User",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "User",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_Payment_Info_user_id",
                table: "User_Payment_Info",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "User_Phone_Number_phone_number_key",
                table: "User_Phone_Number",
                column: "phone_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Wishlist_user_id_key",
                table: "Wishlist",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Wishlist_Items_product_id",
                table: "Wishlist_Items",
                column: "product_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Category_Change_Log");

            migrationBuilder.DropTable(
                name: "Discount_Change_Log");

            migrationBuilder.DropTable(
                name: "Inventory_Products");

            migrationBuilder.DropTable(
                name: "Order_Item");

            migrationBuilder.DropTable(
                name: "Product_Change_Log");

            migrationBuilder.DropTable(
                name: "Product_Review");

            migrationBuilder.DropTable(
                name: "Shopping_Cart_Items");

            migrationBuilder.DropTable(
                name: "User_Address");

            migrationBuilder.DropTable(
                name: "User_Payment_Info");

            migrationBuilder.DropTable(
                name: "User_Phone_Number");

            migrationBuilder.DropTable(
                name: "Wishlist_Items");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Inventory");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "Shopping_Cart");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "Wishlist");

            migrationBuilder.DropTable(
                name: "Payment");

            migrationBuilder.DropTable(
                name: "Shipping");

            migrationBuilder.DropTable(
                name: "Order_Status");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "Discount");

            migrationBuilder.DropTable(
                name: "Payment_Method");

            migrationBuilder.DropTable(
                name: "Payment_Status");

            migrationBuilder.DropTable(
                name: "Carrier");

            migrationBuilder.DropTable(
                name: "Shipping_Status");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Discount_Unit");
        }
    }
}
