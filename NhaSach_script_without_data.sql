
if DB_ID('NhaSach_online') is not null
begin
	-- Delete database backup and restore history from MSDB System Database
	exec msdb.dbo.sp_delete_database_backuphistory @database_name = 'NhaSach_online'
	-- query to get exclusive access of SQL Server Database before Dropping the Database
	use [master]
	alter database [NhaSach_online] set SINGLE_USER with rollback immediate
	-- query to drop database in SQL Server
	drop database NhaSach_online
end
else
begin
	create database NhaSach_online
end
go

use NhaSach_online
go

if OBJECT_ID ('Customer', 'U') is not null
	drop table Customer
else
begin
	create table Customer
	(
		id int identity not null primary key,	--CUS001/AD001
		firstName nvarchar(255),
		lastName nvarchar(255),
		phone varchar(50),
		email varchar(255),
		address nvarchar(255),
		username varchar(255),
		password varchar(1000),
		isAddmin bit
	)
end
go

if OBJECT_ID('Product', 'U') is not null
	drop table Product
else
begin
	create table Product
	(
		id int identity not null primary key,	--PRO001
		name nvarchar(50),
		mainPrice float,
		vicePrice float,
		discount float,
		kind nvarchar(50),
		author nvarchar(255),	
		quantity int,
		state bit	--tình trạng: còn hàng/hết hàng

	)
end
go


if OBJECT_ID('OrderDetail', 'U') is not null
	drop table OrderDetail
else
begin
	create table OrderDetail
	(
		id int identity not null primary key,	--ORD001
		productId int,
		quantity int
	)
end
go

if OBJECT_ID('Order', 'U') is not null
	drop table [Order]
else
begin
	create table [Order]
	(
		id int identity not null primary key,	--OR001
		customerId int,
		dateBought datetime,
		orderDetailId int
	)
end
go

alter table [Order]
	add constraint FK_Order_Customer foreign key(customerId) references Customer(id)
alter table [Order]
	add constraint FK_Order_OrderDetail foreign key(orderDetailId) references OrderDetail(id)


alter table OrderDetail
	add constraint FK_OrderDetail_Product foreign key(productId) references Product(id)

if OBJECT_ID('InputInvoice', 'U') is not null
	drop table InputInvoice
else
begin
	create table InputInvoice
	(
		id int identity not null primary key,	--INI001
		productId int,
		quantity int
	)
end
go

alter table InputInvoice
	add constraint FK_InputInvoice_Product foreign key(productId) references Product(id)


if OBJECT_ID('Receipt', 'U') is not null
	drop table Receipt
else
begin
	create table Receipt
	(
		id int identity not null primary key,	--RE001
		customerId int,
		orderId int,
		totalPrice float
	)
end
go

alter table Receipt
	add constraint FK_Receipt_Customer foreign key(customerId) references Customer(id)
alter table Receipt
	add constraint FK_Receipt_Order foreign key(orderId) references [Order](id)
