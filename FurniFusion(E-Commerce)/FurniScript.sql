-- -- -- Database: FurniFusionDb

-- DROP DATABASE IF EXISTS "FurniFusionDb";

-- CREATE DATABASE "FurniFusionDb"
--     WITH
--     OWNER = postgres
--     ENCODING = 'UTF8'
--     LC_COLLATE = 'English_United States.1252'
--     LC_CTYPE = 'English_United States.1252'
--     LOCALE_PROVIDER = 'libc'
--     TABLESPACE = pg_default
--     CONNECTION LIMIT = -1
--     IS_TEMPLATE = False;

-- 1. Table: User (no foreign key dependencies)
CREATE TABLE "User" (
    user_id SERIAL PRIMARY KEY,                      -- Primary key for the user table
    first_name VARCHAR(100) NOT NULL,                -- Ensure first name is provided
    last_name VARCHAR(100) NOT NULL,                 -- Ensure last name is provided
    email VARCHAR(150) UNIQUE NOT NULL,              -- Ensure email is unique and cannot be null
    image_url TEXT,                                  -- URL to the user's profile image
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,  -- Track when the user was created
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP    -- Track when the user information was last updated
);

-- 2. Table: Payment_Method (no foreign key dependencies)
CREATE TABLE "Payment_Method" (
    method_id SERIAL PRIMARY KEY,
    method_name VARCHAR(100) NOT NULL UNIQUE,  -- Ensure the method name is unique and cannot be null
    is_active BOOLEAN DEFAULT FALSE,             -- Indicate if the payment method is active
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,  -- Track when the payment method was created
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP    -- Track when the payment method was last updated
);

-- 3. Table: Carrier (no foreign key dependencies)
CREATE TABLE "Carrier" (
    carrier_id SERIAL PRIMARY KEY,
    carrier_name VARCHAR(100) NOT NULL UNIQUE,           -- Ensure the carrier name is unique and cannot be null
    email VARCHAR(255) NOT NULL UNIQUE,                  -- Contact email for the carrier
    phone VARCHAR(20) NOT NULL UNIQUE,                   -- Contact phone number for the carrier
    website VARCHAR(255),                                -- Website URL for the carrier
    address VARCHAR(255) NOT NULL,                       -- Physical address of the carrier
    shipping_api VARCHAR(255),                            -- API endpoint for shipping services
    is_active BOOLEAN DEFAULT FALSE,                      -- Indicates if the carrier is currently active
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,      -- Track when the carrier was created
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP        -- Track when the carrier information was last updated
);

-- 4. Table: Inventory (no foreign key dependencies)
CREATE TABLE "Inventory" (
    inventory_id SERIAL PRIMARY KEY,
    warehouse_location VARCHAR(255) NOT NULL,              -- Ensure the warehouse location is specified
    is_active BOOLEAN DEFAULT FALSE,                        -- Indicates if the inventory location is active
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,       -- Track when the inventory was created
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP         -- Track when the inventory information was last updated
);

-- First, create the ENUM type for discount_unit
CREATE TABLE "Discount_Unit" (
    unit_id SERIAL PRIMARY KEY,
    unit_name VARCHAR(10) UNIQUE NOT NULL -- '%' or '$'
);

-- Then, create the Discount table using the ENUM
CREATE TABLE "Discount" (
    discount_id SERIAL PRIMARY KEY,
    discount_code VARCHAR(100) UNIQUE NOT NULL,
    discount_value DECIMAL(10, 2) NOT NULL,
    discount_unit_id INT REFERENCES "Discount_Unit"(unit_id),
    valid_from DATE NOT NULL,
    valid_to DATE NOT NULL,
    is_active BOOLEAN DEFAULT False,
    max_amount DECIMAL(10, 2),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ,
    created_by INT REFERENCES "User"(user_id),
    updated_by INT REFERENCES "User"(user_id),
    CHECK (valid_from <= valid_to)
);

-- Change Log Table for Discount
CREATE TABLE "Discount_Change_Log" (
    log_id SERIAL PRIMARY KEY,
    discount_id INT NOT NULL REFERENCES "Discount"(discount_id),
    old_discount_code VARCHAR(100),
    new_discount_code VARCHAR(100),
    old_discount_value DECIMAL(10, 2),
    new_discount_value DECIMAL(10, 2),
    old_discount_unit_id INT REFERENCES "Discount_Unit"(unit_id),
    new_discount_unit_id INT REFERENCES "Discount_Unit"(unit_id),
    updated_by INT REFERENCES "User"(user_id),  -- Who made the change
    changed_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,  -- When the change occurred
    action_type VARCHAR(10) DEFAULT 'UPDATE'  -- Can be 'UPDATE' or 'DELETE'
);


-- 6. Table: Category (depends on User)
CREATE TABLE "Category" (
    category_id SERIAL PRIMARY KEY,
    category_name VARCHAR(100) UNIQUE,
    created_by INT REFERENCES "User"(user_id),
    updated_by INT REFERENCES "User"(user_id),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP 
);

-- Change Log Table for Category
CREATE TABLE "Category_Change_Log" (
    log_id SERIAL PRIMARY KEY,
    -- category_id INT NOT NULL REFERENCES "Category"(category_id),
    old_category_name VARCHAR(100),
    new_category_name VARCHAR(100),
    updated_by INT REFERENCES "User"(user_id),  -- Who made the change
    changed_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,  -- When the change occurred
    action_type VARCHAR(10) DEFAULT 'UPDATE'  -- Can be 'UPDATE' or 'DELETE'
);

-- 7. Table: Product (depends on Discount and Category)
CREATE TABLE "Product" (
    product_id SERIAL PRIMARY KEY,
    product_name VARCHAR(255) NOT NULL,       -- Name of the product
    image_urls TEXT[] DEFAULT '{}',           -- Array of image URLs for the product
    dimensions JSONB,                         -- JSONB field to store dimensions (e.g., {"length": x, "width": y, "height": z})
    weight DECIMAL(10,2) DEFAULT 0.00,        -- Product weight in kg or other units
    colors TEXT[] DEFAULT '{}',               -- Array of available colors
    description TEXT,                         -- Detailed description of the product
    price DECIMAL(10,2) NOT NULL CHECK (price > 0),             -- Price of the product
    stock_quantity INT DEFAULT 0 CHECK(stock_quantity >= 0),             -- Number of items in stock
    is_available BOOLEAN DEFAULT FALSE,       -- Availability status of the product
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP, -- Creation timestamp
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP , -- Last updated timestamp
    created_by INT REFERENCES "User"(user_id),      -- User who created the product
    updated_by INT REFERENCES "User"(user_id),      -- User who last updated the product
    discount_id INT REFERENCES "Discount"(discount_id) ON DELETE SET NULL,  -- Reference to Discount (optional)
    category_id INT REFERENCES "Category"(category_id) ON DELETE SET NULL,  -- Reference to Category
    average_rating DECIMAL(1,2) DEFAULT 0.00 CHECK (average_rating >= 0 AND average_rating <= 5.0)  -- Average rating of the product based on reviews
);

-- Change Log Table for Product
CREATE TABLE "Product_Change_Log" (
    log_id SERIAL PRIMARY KEY,
    product_id INT NOT NULL REFERENCES "Product"(product_id) ON DELETE CASCADE,
    old_product_name VARCHAR(255),
    new_product_name VARCHAR(255),
    old_image_urls TEXT[],
    new_image_urls TEXT[],
    old_dimensions JSONB,
    new_dimensions JSONB,
    old_weight DECIMAL(10, 2),
    new_weight DECIMAL(10, 2),
    old_colors TEXT[],
    new_colors TEXT[],
    old_description TEXT,
    new_description TEXT,
    old_price DECIMAL(10, 2),
    new_price DECIMAL(10, 2),
    old_stock_quantity INT,
    new_stock_quantity INT,
    old_is_available BOOLEAN,
    new_is_available BOOLEAN,
    updated_by INT REFERENCES "User"(user_id),  -- Who made the change
    changed_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,  -- When the change occurred
    action_type VARCHAR(10) DEFAULT 'UPDATE'  -- 'UPDATE' or 'DELETE'
);

-- 8. Table: Product_Review (depends on User and Product)
CREATE TABLE "Product_Review" (
    review_id SERIAL PRIMARY KEY,
    product_id INT REFERENCES "Product"(product_id) ON DELETE CASCADE,  -- Ensure reviews are removed if the product is deleted
    user_id INT REFERENCES "User"(user_id) ON DELETE CASCADE,          -- Ensure reviews are removed if the user is deleted
    rating INT NOT NULL CHECK (rating >= 1 AND rating <= 5),           -- Rating from 1 to 5
    review_text TEXT,                                                  -- Optional review text
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,                   -- Timestamp when the review was created
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP                     -- Timestamp when the review was last updated
);

-- 9. Table: Inventory_Products (depends on Inventory and Product)
CREATE TABLE "Inventory_Products" (
    inventory_id INT REFERENCES "Inventory"(inventory_id) ON DELETE CASCADE, -- Ensure that deleting an inventory deletes related entries
    product_id INT REFERENCES "Product"(product_id) ON DELETE CASCADE,       -- Ensure that deleting a product deletes related entries
    quantity INT DEFAULT 0 CHECK (quantity >= 0),   -- Ensure non-negative quantity
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP, -- Timestamp when the record was created
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP , -- Timestamp when the record was last updated
    PRIMARY KEY(inventory_id, product_id)
);

-- 2. Table: Shipping_Status
CREATE TABLE "Shipping_Status" (
    status_id SERIAL PRIMARY KEY,
    status_name VARCHAR(50) UNIQUE NOT NULL -- 'pending', 'shipped', etc.
);

-- Then, create the Shipping table using the ENUM for shipping_status
CREATE TABLE "Shipping" (
    shipping_id SERIAL PRIMARY KEY,
    shipping_method VARCHAR(100) NOT NULL,
    shipping_cost DECIMAL(10,2) NOT NULL CHECK (shipping_cost >= 0),
    shipping_date DATE NOT NULL,
    estimated_delivery_date DATE NOT NULL,
    shipping_status_id INT REFERENCES "Shipping_Status"(status_id),
    carrier_id INT REFERENCES "Carrier"(carrier_id) ON DELETE SET NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP 
);

CREATE TABLE "Payment_Status" (
    status_id SERIAL PRIMARY KEY,
    status_name VARCHAR(50) UNIQUE NOT NULL -- 'pending', 'completed', etc.
);

-- 11. Table: Payment (depends on User)
CREATE TABLE "Payment" (
    payment_id SERIAL PRIMARY KEY,
    amount DECIMAL(10,2) NOT NULL CHECK (amount > 0),    -- Amount of the payment, ensuring it's positive
    payment_status_id INT REFERENCES "Payment_Status"(status_id),                 -- Status of the payment (e.g., pending, completed, failed)
    date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,             -- Date and time of the payment
    payment_method INT REFERENCES "Payment_Method"(method_id), -- Reference to Payment Method table
    transaction_id VARCHAR(255) UNIQUE,                   -- Unique transaction ID for the payment
    user_id INT REFERENCES "User"(user_id) ON DELETE CASCADE -- Reference to User, ensures user deletion affects payments
);


CREATE TABLE "Order_Status" (
    status_id SERIAL PRIMARY KEY,
    status_name VARCHAR(50) UNIQUE NOT NULL -- 'pending', 'processing', etc.
);

-- 12. Table: Order (depends on User, Discount, Payment, and Shipping)
CREATE TABLE "Order" (
    order_id SERIAL PRIMARY KEY,
    user_id INT REFERENCES "User"(user_id) ON DELETE CASCADE,  -- Delete orders if the user is deleted
    status INT REFERENCES "Order_Status"(status_id),                      -- Default status of the order
    address_to_deliver VARCHAR(255) NOT NULL,                  -- Delivery address must be provided
    total_price DECIMAL(10,2) NOT NULL CHECK (total_price >= 0), -- Total price must be non-negative
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,           -- Timestamp when the order was created
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ,           -- Timestamp when the order was last updated
    discount_id INT REFERENCES "Discount"(discount_id) ON DELETE SET NULL,  -- Allow discounts to be removed
    payment_id INT REFERENCES "Payment"(payment_id) ON DELETE SET NULL,      -- Allow payments to be removed
    shipping_id INT REFERENCES "Shipping"(shipping_id) ON DELETE SET NULL    -- Allow shipping info to be removed
);

-- 13. Table: Order_Item (depends on Order and Product)
CREATE TABLE "Order_Item" (
    item_id SERIAL PRIMARY KEY,
    order_id INT REFERENCES "Order"(order_id) ON DELETE CASCADE,  -- Ensure items are removed if the order is deleted
    product_id INT REFERENCES "Product"(product_id) ON DELETE CASCADE,  -- Ensure items are removed if the product is deleted
    price DECIMAL(10,2) NOT NULL CHECK (price >= 0),   -- Price of the item must be non-negative
    quantity INT NOT NULL CHECK (quantity > 0),         -- Quantity must be positive
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP       -- Track when the item was added to the order
);


-- 14. Table: User_Address (depends on User)
CREATE TABLE "User_Address" (
    address_id SERIAL PRIMARY KEY,
    user_id INT REFERENCES "User"(user_id) ON DELETE CASCADE,  -- Ensure addresses are removed if the user is deleted
    country VARCHAR(100) NOT NULL,                              -- Ensure country is always provided
    city VARCHAR(100) NOT NULL,                                 -- Ensure city is always provided
    state VARCHAR(100) NOT NULL,                                -- Ensure state is always provided
    street VARCHAR(255) NOT NULL,                              -- Ensure street address is always provided
    postal_code VARCHAR(20) NOT NULL,                          -- Ensure postal code is always provided
    is_primary_address BOOLEAN DEFAULT FALSE,                  -- Default to false to allow multiple addresses
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,            -- Track when the address was created
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ,            -- Track when the address was last updated
    PRIMARY KEY(address_id)  -- Composite primary key for user and address
);


-- 15. Table: User_Phone_Number (depends on User)
CREATE TABLE "User_Phone_Number" (
    user_id INT REFERENCES "User"(user_id) ON DELETE CASCADE,   -- Ensure phone numbers are removed if the user is deleted
    phone_number VARCHAR(15) NOT NULL UNIQUE,                  -- Ensure phone numbers are unique and cannot be NULL
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,            -- Track when the phone number was created
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ,            -- Track when the phone number was last updated
    PRIMARY KEY(user_id, phone_number)                         -- Composite primary key for user and phone number
);


-- 16. Table: User_Payment_Info (depends on User)
CREATE TABLE "User_Payment_Info" (
    payment_info_id SERIAL PRIMARY KEY,
    user_id INT REFERENCES "User"(user_id) ON DELETE CASCADE,  -- Ensure payment info is removed if the user is deleted
    card_number VARCHAR(16) NOT NULL,                           -- Ensure card number cannot be NULL
    card_type VARCHAR(50) NOT NULL,                             -- Ensure card type cannot be NULL
    expiry_date DATE NOT NULL,                                  -- Ensure expiry date cannot be NULL
    billing_address VARCHAR(255) NOT NULL,                      -- Ensure billing address cannot be NULL
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,            -- Track when the payment info was created
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP              -- Track when the payment info was last updated
);

-- 17. Table: Wishlist (depends on User and Product)
CREATE TABLE "Wishlist" (
    wishlist_id SERIAL PRIMARY KEY,
    user_id INT UNIQUE REFERENCES "User"(user_id) ON DELETE CASCADE,  -- Ensure wishlists are removed if the user is deleted
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP              -- Track when the wishlist was last updated
);

-- Table to hold products in the wishlist
CREATE TABLE "Wishlist_Items" (
    wishlist_id INT REFERENCES "Wishlist"(wishlist_id) ON DELETE CASCADE,  -- Reference to the wishlist
    product_id INT REFERENCES "Product"(product_id) ON DELETE CASCADE,      -- Reference to the product
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,                        -- Track when the product was added to the wishlist
    PRIMARY KEY(wishlist_id, product_id)                                   -- Composite primary key for wishlist and product
);

-- 18. Table: Shopping_Cart (one per user, depends on User)
CREATE TABLE "Shopping_Cart" (
    cart_id SERIAL PRIMARY KEY,
    user_id INT UNIQUE REFERENCES "User"(user_id) ON DELETE CASCADE,  -- Ensure only one cart per user, remove on user deletion
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP                  -- Track when the cart was last updated
);

CREATE TABLE "Shopping_Cart_Items" (
    cart_id INT REFERENCES "Shopping_Cart"(cart_id) ON DELETE CASCADE,  -- Reference to the shopping cart
    product_id INT REFERENCES "Product"(product_id) ON DELETE CASCADE,   -- Reference to the product
    quantity INT DEFAULT 1 CHECK (quantity > 0),                                             -- Quantity of the product in the cart
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,                    -- Track when the item was added to the cart
    PRIMARY KEY(cart_id, product_id)                                   -- Composite primary key for cart and product
);

-- Function to log category updates
CREATE OR REPLACE FUNCTION log_category_changes() 
RETURNS TRIGGER AS $$
BEGIN
    -- Convert old and new category names to lower case for comparison
    IF LOWER(OLD.category_name) IS DISTINCT FROM LOWER(NEW.category_name) THEN
        INSERT INTO "Category_Change_Log" (
            old_category_name, 
            new_category_name, 
            updated_by,
            action_type  -- Set action type to 'UPDATE'
        )
        VALUES (
            LOWER(OLD.category_name),  -- Log the old category name in lower case
            LOWER(NEW.category_name),  -- Log the new category name in lower case
            NEW.updated_by,
            'UPDATE'  -- Specify action as an update
        );
    END IF;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

-- Trigger that calls the function on updates
CREATE TRIGGER category_update_trigger
AFTER UPDATE ON "Category"
FOR EACH ROW
EXECUTE FUNCTION log_category_changes();

-- Function to log category deletions
CREATE OR REPLACE FUNCTION log_category_deletions() 
RETURNS TRIGGER AS $$
BEGIN
    INSERT INTO "Category_Change_Log" (
        old_category_name,  -- Log the old category name
        updated_by,         -- The user who deleted the category
        action_type         -- Mark this action as 'DELETE'
    )
    VALUES (
        LOWER(OLD.category_name),  -- Log the old category name in lower case
        OLD.updated_by,            -- User who performed the deletion
        'DELETE'                   -- Set action_type to 'DELETE'
    );
    RETURN OLD;
END;
$$ LANGUAGE plpgsql;

-- Trigger that calls the function on deletions
CREATE TRIGGER category_delete_trigger
AFTER DELETE ON "Category"
FOR EACH ROW
EXECUTE FUNCTION log_category_deletions();




-- Trigger Function to log changes for Product
CREATE OR REPLACE FUNCTION log_product_changes() 
RETURNS TRIGGER AS $$
BEGIN
    IF LOWER(OLD.product_name) IS DISTINCT FROM LOWER(NEW.product_name) OR
       OLD.image_urls IS DISTINCT FROM NEW.image_urls OR
       OLD.dimensions IS DISTINCT FROM NEW.dimensions OR
       OLD.weight IS DISTINCT FROM NEW.weight OR
       OLD.colors IS DISTINCT FROM NEW.colors OR
       LOWER(OLD.description) IS DISTINCT FROM LOWER(NEW.description) OR
       OLD.price IS DISTINCT FROM NEW.price OR
       OLD.stock_quantity IS DISTINCT FROM NEW.stock_quantity OR
       OLD.is_available IS DISTINCT FROM NEW.is_available THEN
       
        INSERT INTO "Product_Change_Log" (
            product_id, old_product_name, new_product_name,
            old_image_urls, new_image_urls,
            old_dimensions, new_dimensions,
            old_weight, new_weight,
            old_colors, new_colors,
            old_description, new_description,
            old_price, new_price,
            old_stock_quantity, new_stock_quantity,
            old_is_available, new_is_available,
            updated_by,
            action_type
        )
        VALUES (
            OLD.product_id, 
            LOWER(OLD.product_name), LOWER(NEW.product_name),
            OLD.image_urls, NEW.image_urls,
            OLD.dimensions, NEW.dimensions,
            OLD.weight, NEW.weight,
            OLD.colors, NEW.colors,
            LOWER(OLD.description), LOWER(NEW.description),
            OLD.price, NEW.price,
            OLD.stock_quantity, NEW.stock_quantity,
            OLD.is_available, NEW.is_available,
            NEW.updated_by,
            'UPDATE'
        );
    END IF;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

-- Trigger that calls the function on updates
CREATE TRIGGER product_update_trigger
AFTER UPDATE ON "Product"
FOR EACH ROW
EXECUTE FUNCTION log_product_changes();

-- Function to log product deletions
CREATE OR REPLACE FUNCTION log_product_deletions() 
RETURNS TRIGGER AS $$
BEGIN
    INSERT INTO "Product_Change_Log" (
        product_id, 
        old_product_name, 
        old_image_urls,
        old_dimensions,
        old_weight,
        old_colors,
        old_description,
        old_price,
        old_stock_quantity,
        old_is_available,
        updated_by,
        action_type
    )
    VALUES (
        OLD.product_id, 
        OLD.product_name, 
        OLD.image_urls,
        OLD.dimensions,
        OLD.weight,
        OLD.colors,
        OLD.description,
        OLD.price,
        OLD.stock_quantity,
        OLD.is_available,
        OLD.updated_by,
        'DELETE'
    );
    RETURN OLD;
END;
$$ LANGUAGE plpgsql;

-- Trigger that calls the function on deletions
CREATE TRIGGER product_delete_trigger
BEFORE DELETE ON "Product"
FOR EACH ROW
EXECUTE FUNCTION log_product_deletions();


-- Trigger Function to log changes for Discount
CREATE OR REPLACE FUNCTION log_discount_changes() 
RETURNS TRIGGER AS $$
BEGIN
    IF LOWER(OLD.discount_code) IS DISTINCT FROM LOWER(NEW.discount_code) OR
       OLD.discount_value IS DISTINCT FROM NEW.discount_value OR
       OLD.discount_unit_id IS DISTINCT FROM NEW.discount_unit_id THEN
       
        INSERT INTO "Discount_Change_Log" (
            discount_id, 
            old_discount_code, new_discount_code, 
            old_discount_value, new_discount_value, 
            old_discount_unit_id, new_discount_unit_id, 
            updated_by,
            action_type
        )
        VALUES (
            OLD.discount_id, 
            LOWER(OLD.discount_code), LOWER(NEW.discount_code), 
            OLD.discount_value, NEW.discount_value, 
            OLD.discount_unit_id, NEW.discount_unit_id, 
            NEW.updated_by,
            'UPDATE'
        );
    END IF;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

-- Trigger that calls the function on updates
CREATE TRIGGER discount_update_trigger
AFTER UPDATE ON "Discount"
FOR EACH ROW
EXECUTE FUNCTION log_discount_changes();

-- Function to log discount deletions
CREATE OR REPLACE FUNCTION log_discount_deletions() 
RETURNS TRIGGER AS $$
BEGIN
    INSERT INTO "Discount_Change_Log" (
        discount_id, 
        old_discount_code, 
        old_discount_value, 
        old_discount_unit_id, 
        updated_by,  -- User who deleted the discount
        action_type  -- Mark this action as 'DELETE'
    )
    VALUES (
        OLD.discount_id, 
        LOWER(OLD.discount_code), 
        OLD.discount_value, 
        OLD.discount_unit_id, 
        OLD.updated_by, 
        'DELETE'
    );
    RETURN OLD;
END;
$$ LANGUAGE plpgsql;

-- Trigger that calls the function on deletions
CREATE TRIGGER discount_delete_trigger
BEFORE DELETE ON "Discount"
FOR EACH ROW
EXECUTE FUNCTION log_discount_deletions();



// Function to update average ratings
CREATE OR REPLACE FUNCTION update_average_rating() RETURNS VOID AS $$
BEGIN
    -- Update average ratings for all products
    UPDATE "Product" p
    SET average_rating = COALESCE(r.avg_rating, 0)  -- Set to 0 if no reviews
    FROM (
        SELECT product_id, AVG(rating) AS avg_rating
        FROM "Product_Review"
        GROUP BY product_id
    ) r
    WHERE p.product_id = r.product_id;
END;
$$ LANGUAGE plpgsql;

