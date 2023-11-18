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

-- Stored procedure to transform data in products table

CREATE OR ALTER PROCEDURE sp_transform_data
  
AS
BEGIN
    DECLARE @SqlStatement NVARCHAR(MAX);
    SET @SqlStatement = N'UPDATE products SET product_name = lower(product_name);';
    EXEC sp_executesql @SqlStatement;
END;