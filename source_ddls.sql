drop table OrderProducts;
drop table Orders;
drop table Products;
drop table Categories;

CREATE TABLE Categories (
    category_id INT PRIMARY KEY IDENTITY(1,1),
    category_name NVARCHAR(255) NOT NULL
);

CREATE TABLE Products (
    product_id INT PRIMARY KEY IDENTITY(1,1),
    product_name NVARCHAR(255) NOT NULL,
    category_id INT,
    price DECIMAL(10, 2) NOT NULL,
    description NVARCHAR(MAX),
    image_url NVARCHAR(MAX),
    date_added DATE,
    INDEX IX_CategoryId NONCLUSTERED (category_id),
    INDEX IX_Price NONCLUSTERED (price)
);

CREATE TABLE Orders (
    order_id INT PRIMARY KEY IDENTITY(1,1),
    order_date DATE NOT NULL,
    customer_name NVARCHAR(255) NOT NULL,
	INDEX IX_order_date NONCLUSTERED (order_date)
);

CREATE TABLE OrderProducts (
    order_id INT,
    product_id INT,
    PRIMARY KEY CLUSTERED (order_id, product_id),
    FOREIGN KEY (order_id) REFERENCES Orders(order_id),
    FOREIGN KEY (product_id) REFERENCES Products(product_id)
);




-- Enable CDC on the database
USE sourcedb
GO
EXEC sys.sp_cdc_enable_db;

-- Enable CDC on tables
EXEC sys.sp_cdc_enable_table 
   @source_schema = N'dbo', 
   @source_name = N'Products', 
   @role_name = NULL;

EXEC sys.sp_cdc_enable_table 
   @source_schema = N'dbo', 
   @source_name = N'Categories', 
   @role_name = NULL;

EXEC sys.sp_cdc_enable_table 
   @source_schema = N'dbo', 
   @source_name = N'Orders', 
   @role_name = NULL;

EXEC sys.sp_cdc_enable_table 
   @source_schema = N'dbo', 
   @source_name = N'OrderProducts', 
   @role_name = NULL;

