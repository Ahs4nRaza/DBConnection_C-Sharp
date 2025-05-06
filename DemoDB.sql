
-- CREATE A NEW DATABASE
	CREATE DATABASE InventorySystem;

-- SWITCH TO NEW DATABASE
	USE InventorySystem;
	GO

-- CREATE NEW TABLES
--  CUSTOMERS
	CREATE TABLE CUSTOMERS 
	(
		CustomerId INT PRIMARY KEY IDENTITY(1,1),
		CustomerName NVARCHAR(255) NOT NULL,
		EmailAddress NVARCHAR(255) NULL,
		PhoneNumber NVARCHAR(20) NOT NULL,
		ShippingAddress NVARCHAR(500) NOT NULL,

		CONSTRAINT UQ_Customers_EmailAddress UNIQUE (EmailAddress),
		CONSTRAINT UQ_Customers_PhoneNumber UNIQUE (PhoneNumber),
		CONSTRAINT CK_Customers_PhoneNumberLength CHECK (LEN(PhoneNumber) = 11),
		CONSTRAINT CK_Customers_PhoneNumberDigits CHECK (PhoneNumber NOT LIKE '%[^0-9]%')
	);

--  PRODUCTS
	CREATE TABLE PRODUCTS 
	(
		ProductId INT PRIMARY KEY IDENTITY(1,1),
		ProductName NVARCHAR(255) NOT NULL,
		Price DECIMAL(10, 2) NOT NULL CHECK (Price > 0.00),
		AvailableStock INT NOT NULL DEFAULT 0 CHECK (AvailableStock >= 0),
		CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
		UpdatedAt DATETIME NULL 
    );

--  PRODUCT_DETAILS 
	CREATE TABLE PRODUCT_DETAILS 
	(
		ProductDetailId INT PRIMARY KEY IDENTITY(1,1),
		ProductId INT NOT NULL,
		ProductDescription NVARCHAR(1000),
		Manufacturer NVARCHAR(255) NOT NULL,
		WarrantyMonths INT NOT NULL DEFAULT 0 CHECK (WarrantyMonths >= 0),

		CONSTRAINT FK_ProductDetails_ProductId FOREIGN KEY (ProductId) REFERENCES PRODUCTS(ProductId) ON DELETE CASCADE,
		CONSTRAINT UQ_ProductDetails_ProductId UNIQUE (ProductId)
	);


-- ORDER_STATUS
	CREATE TABLE ORDER_STATUS 
	(
		StatusId INT PRIMARY KEY,
		StatusName NVARCHAR(50) NOT NULL UNIQUE
	);

--  ORDERS
	CREATE TABLE ORDERS 
	(
		OrderId INT PRIMARY KEY IDENTITY(1,1),
		CustomerId INT NOT NULL,
		CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
		UpdatedAt DATETIME NULL,
		StatusId INT NOT NULL DEFAULT 1,
		TotalAmount DECIMAL(10, 2) NOT NULL DEFAULT 0.00 CHECK (TotalAmount >= 0.00),

		CONSTRAINT FK_Orders_CustomerId FOREIGN KEY (CustomerId) REFERENCES CUSTOMERS(CustomerId) ON DELETE NO ACTION,
		CONSTRAINT FK_Orders_StatusId FOREIGN KEY (StatusId) REFERENCES ORDER_STATUS(StatusId)
	);


--  ORDER_DETAILS
	CREATE TABLE ORDER_DETAILS 
	(
		OrderDetailsId INT PRIMARY KEY IDENTITY(1,1),
		OrderId INT NOT NULL,
		ProductId INT NOT NULL,
		OrderDescription NVARCHAR(1000),
		Quantity INT DEFAULT 1 CHECK (Quantity > 0),

		CONSTRAINT FK_OrderDetails_OrderId FOREIGN KEY (OrderId) REFERENCES ORDERS(OrderId) ON DELETE CASCADE,
		CONSTRAINT FK_OrderDetails_ProductId FOREIGN KEY (ProductId) REFERENCES PRODUCTS(ProductId) ON DELETE NO ACTION
	);


-- POPULATE ORDER_STATUS 
	INSERT INTO ORDER_STATUS (StatusId, StatusName) VALUES
	(1, 'Pending'),
	(2, 'Delivering'),
	(3, 'Completed');

--	POPULATE OTHER TABLES
	INSERT INTO CUSTOMERS (CustomerName, EmailAddress, PhoneNumber, ShippingAddress) VALUES
	('Customer 1', 'customer.1@example.com', '11111111111', '123 Main Street, Town A'),
	('Customer 2', 'customer.2@example.com', '22222222222', '456 Side Street, Town B');


	INSERT INTO PRODUCTS (ProductName, Price, AvailableStock) VALUES
	('Mouse', 5.99, 100),
	('Keyboard', 9.50, 50);

	INSERT INTO PRODUCT_DETAILS (ProductId, ProductDescription, Manufacturer, WarrantyMonths) VALUES
	(1, '2.4GHz Wireless Mouse with ergonomic design.', 'Logitech', 12),
	(2, 'RGB Mechanical Keyboard with blue switches.', 'Corsair', 24);

	INSERT INTO ORDERS (CustomerId, TotalAmount) VALUES
	(1, 9.50),
	(2, 5.99);

	INSERT INTO ORDER_DETAILS (OrderId, ProductId, OrderDescription, Quantity) VALUES
	(2, 2, 'Mechanical Keyboard for office use', 1),
	(3, 1, 'Mouse for travel use', 1);


--  VIEW INSERTED RECORDS
	SELECT * FROM CUSTOMERS;
	SELECT * FROM PRODUCTS;
	SELECT * FROM PRODUCT_DETAILS;
	SELECT * FROM ORDERS;
	SELECT * FROM ORDER_DETAILS;
	

--	DELETE ORDER # 2 & VIEW THE RESULTS
	DELETE FROM ORDERS WHERE OrderId = 2;
	
	SELECT * FROM ORDERS;
	SELECT * FROM ORDER_DETAILS;

--	UPDATE CUSTOMER RECORD # 2 & VIEW RESULTS
	UPDATE CUSTOMERS 
	SET PhoneNumber = '00000000000',
		ShippingAddress = 'New Location, xyz'
	WHERE CustomerId = 1;

	SELECT * FROM CUSTOMERS;
	SELECT * FROM ORDERS;
		