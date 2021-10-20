USE master
IF NOT EXISTS(select * from sys.databases where name='ShopBridge')
	CREATE DATABASE ShopBridge;
GO
USE ShopBridge
GO
IF OBJECT_ID('dbo.Products', 'U') IS NOT NULL 
  DROP TABLE dbo.Products;

CREATE TABLE dbo.Products(
	ProductID INT IDENTITY(1,1) NOT NULL,
	ProductName NVARCHAR(250) NOT NULL,
	Price money NOT NULL
	CONSTRAINT pk_Product PRIMARY KEY (ProductID)
) 
GO
SET IDENTITY_INSERT dbo.Products ON 
GO
INSERT dbo.Products (ProductID, ProductName, Price) VALUES (386, N'Hex Nut 1', 3.5000)
GO
INSERT dbo.Products (ProductID, ProductName, Price) VALUES (362, N'Thin-Jam Hex Nut 2', 5.5)
GO
INSERT dbo.Products (ProductID, ProductName, Price) VALUES (734, N'ML Road Frame - Red, 58', 594.8300)
GO
INSERT dbo.Products (ProductID, ProductName, Price) VALUES (945, N'Front Derailleur', 91.4900)
GO
INSERT dbo.Products (ProductID, ProductName, Price) VALUES (725, N'LL Road Frame - Red, 44', 337.2200)
GO
INSERT dbo.Products (ProductID, ProductName, Price) VALUES (893, N'HL Touring Frame - Blue, 60', 1003.9100)
GO
INSERT dbo.Products (ProductID, ProductName, Price) VALUES (871, N'Mountain Bottle Cage', 9.9900)
GO
INSERT dbo.Products (ProductID, ProductName, Price) VALUES (929, N'ML Mountain Tire', 29.9900)
GO
INSERT dbo.Products (ProductID, ProductName, Price) VALUES (798, N'Road-550-W Yellow, 40', 1120.4900)
GO
INSERT dbo.Products (ProductID, ProductName, Price) VALUES (800, N'Road-550-W Yellow, 44', 1120.4900)
GO
SET IDENTITY_INSERT dbo.Products OFF
GO