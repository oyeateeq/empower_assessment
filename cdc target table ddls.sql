

CREATE TABLE Categories_cdc (
    category_id INT ,
    category_name NVARCHAR(255) NOT NULL
);

CREATE TABLE Products_cdc (
    product_id INT,
    product_name NVARCHAR(255) NOT NULL,
    category_id INT ,
    price DECIMAL(10, 2) NOT NULL,
    description NVARCHAR(MAX),
    image_url NVARCHAR(MAX),
    date_added DATE
);


CREATE TABLE Orders_cdc (
    order_id INT ,
    order_date DATE NOT NULL,
    customer_name NVARCHAR(255) NOT NULL
);

CREATE TABLE OrderProducts_cdc (
    order_id INT,
    product_id INT
);

