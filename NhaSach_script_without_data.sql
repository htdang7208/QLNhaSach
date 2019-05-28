
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
		isAdmin bit
	)
end
go
insert into Customer values (N'Đàn', N'Trần Thanh', '0132658479', 'thd@gmail.com', N'154 Nguyễn Chí Thanh', 'user1', '601f1889667efaebb33b8c12572835da3f027f78', 0)
insert into Customer values (N'Tâm', N'Trần Thanh', '0132658479', 'thd@gmail.com', N'154 Nguyễn Chí Thanh', 'user2', '601f1889667efaebb33b8c12572835da3f027f78', 0)
insert into Customer values (N'Ly', N'Trần Thanh', '0132658479', 'thd@gmail.com', N'154 Nguyễn Chí Thanh', 'user3', '601f1889667efaebb33b8c12572835da3f027f78', 0)
insert into Customer values (N'Nam', N'Trần Thanh', '0132658479', 'thd@gmail.com', N'154 Nguyễn Chí Thanh', 'user4', '601f1889667efaebb33b8c12572835da3f027f78', 0)
insert into Customer values (N'Trung', N'Trần Thanh', '0132658479', 'thd@gmail.com', N'154 Nguyễn Chí Thanh', 'user5', '601f1889667efaebb33b8c12572835da3f027f78', 0)
insert into Customer values (N'Thanh', N'Trần Thanh', '0132658479', 'thd@gmail.com', N'154 Nguyễn Chí Thanh', 'user6', '601f1889667efaebb33b8c12572835da3f027f78', 0)
insert into Customer values (N'Thành', N'Trần Thanh', '0132658479', 'thd@gmail.com', N'154 Nguyễn Chí Thanh', 'user7', '601f1889667efaebb33b8c12572835da3f027f78', 0)
insert into Customer values (N'Tú', N'Trần Thanh', '0132658479', 'thd@gmail.com', N'154 Nguyễn Chí Thanh', 'admin1', 'ccbe91b1f19bd31a1365363870c0eec2296a61c1', 1)
insert into Customer values (N'Xuân', N'Trần Thanh', '0132658479', 'thd@gmail.com', N'154 Nguyễn Chí Thanh', 'admin2', 'ccbe91b1f19bd31a1365363870c0eec2296a61c1', 1)
insert into Customer values (N'Tuấn', N'Trần Thanh', '0132658479', 'thd@gmail.com', N'154 Nguyễn Chí Thanh', 'admin3', 'ccbe91b1f19bd31a1365363870c0eec2296a61c1', 1)

if OBJECT_ID('Product', 'U') is not null
	drop table Product
else
begin
	create table Product
	(
		id int identity not null primary key,	--PRO001
		name nvarchar(100),
		price float,
		kind nvarchar(100),
		author nvarchar(255),	
		quantity int,
		state bit	--tình trạng: còn hàng/hết hàng
	)
end
go
insert into Product values (N'Thám tử lừng danh Conan (tập 95)', 25000, N'Truyện tranh', null, 100, 1)
insert into Product values (N'Thám tử lừng danh Conan (tập 95)', 25000, N'Truyện tranh', null, 100, 1)
insert into Product values (N'Thám tử lừng danh Conan (tập 95)', 25000, N'Truyện tranh', null, 100, 1)
insert into Product values (N'Thám tử lừng danh Conan (tập 95)', 25000, N'Truyện tranh', null, 100, 1)
insert into Product values (N'Thần đồng Đất Việt (tập 227)', 25000, N'Truyện tranh', null, 100, 1)
insert into Product values (N'Thần đồng Đất Việt (tập 227)', 25000, N'Truyện tranh', null, 100, 1)
insert into Product values (N'Thần đồng Đất Việt (tập 227)', 25000, N'Truyện tranh', null, 100, 1)
insert into Product values (N'Thần đồng Đất Việt (tập 227)', 25000, N'Truyện tranh', null, 100, 1)
insert into Product values (N'Truyện cổ tích Việt Nam được yêu thích nhất', 100000, N'Truyện tranh', null, 100, 1)
insert into Product values (N'Truyện cổ tích Việt Nam được yêu thích nhất', 100000, N'Truyện tranh', null, 100, 1)
insert into Product values (N'Truyện cổ tích Việt Nam được yêu thích nhất', 100000, N'Truyện tranh', null, 100, 1)
insert into Product values (N'Truyện cổ tích Việt Nam được yêu thích nhất', 100000, N'Truyện tranh', null, 100, 1)
insert into Product values (N'Công chúa và chàng ếch', 100000, N'Truyện tranh', null, 100, 1)
insert into Product values (N'Công chúa và chàng ếch', 100000, N'Truyện tranh', null, 100, 1)
insert into Product values (N'Công chúa và chàng ếch', 100000, N'Truyện tranh', null, 100, 1)
insert into Product values (N'Công chúa và chàng ếch', 100000, N'Truyện tranh', null, 100, 1)

if OBJECT_ID('SaleInvoice', 'U') is not null
	drop table SaleInvoice
else
begin
	create table SaleInvoice
	(
		id int identity not null primary key,	--OR001
		customerId int,
		dateSold datetime
	)
end
go
alter table SaleInvoice add constraint FK_SaleInvoice_Customer foreign key(customerId) references Customer(id)
insert into SaleInvoice values (1, '12/12/2019')
insert into SaleInvoice values (2, '12/11/2019')
insert into SaleInvoice values (3, '12/10/2019')
insert into SaleInvoice values (4, '12/09/2019')
insert into SaleInvoice values (5, '12/08/2019')
insert into SaleInvoice values (6, '12/07/2019')
insert into SaleInvoice values (7, '12/06/2019')
insert into SaleInvoice values (8, '12/05/2019')
insert into SaleInvoice values (9, '12/04/2019')
insert into SaleInvoice values (10, '12/03/2019')

if OBJECT_ID('SaleDetail', 'U') is not null
	drop table SaleDetail
else
begin
	create table SaleDetail
	(
		id int identity not null primary key,	--OR001
		saleInvoiceId int,
		productId int,
		quantity int,
		totalPrice float
	)
end
go
alter table SaleDetail add constraint FK_SaleDetail_SaleInvoice foreign key(saleInvoiceId) references SaleInvoice(id)
alter table SaleDetail add constraint FK_SaleDetail_Product foreign key(productId) references Product(id)
insert into SaleDetail values (1, 1, 2, null)
insert into SaleDetail values (1, 2, 2, null)
insert into SaleDetail values (1, 3, 2, null)
insert into SaleDetail values (1, 4, 2, null)
insert into SaleDetail values (1, 5, 2, null)
insert into SaleDetail values (2, 3, 4, null)
insert into SaleDetail values (2, 5, 4, null)
insert into SaleDetail values (2, 7, 4, null)
insert into SaleDetail values (3, 9, 2, null)
insert into SaleDetail values (3, 1, 2, null)
insert into SaleDetail values (3, 3, 2, null)

if OBJECT_ID('InputInvoice', 'U') is not null
	drop table InputInvoice
else
begin
	create table InputInvoice
	(
		id int identity not null primary key,	--INI001
		productId int,
		quantity int,
		totalPrice float
	)
end
go
alter table InputInvoice add constraint FK_InputInvoice_Product foreign key(productId) references Product(id)
insert into InputInvoice values (1, 50, null)
insert into InputInvoice values (1, 50, null)
insert into InputInvoice values (2, 50, null)
insert into InputInvoice values (2, 50, null)
insert into InputInvoice values (3, 50, null)
insert into InputInvoice values (4, 50, null)
insert into InputInvoice values (5, 50, null)
insert into InputInvoice values (1, 50, null)
insert into InputInvoice values (2, 50, null)
insert into InputInvoice values (3, 50, null)

if OBJECT_ID('Receipt', 'U') is not null
	drop table Receipt
else
begin
	create table Receipt
	(
		id int identity not null primary key,	--RE001
		customerId int,
		datePaid datetime,
		payment float
	)
end
go
alter table Receipt add constraint FK_Receipt_Customer foreign key(customerId) references Customer(id)
insert into Receipt values (1, '01/01/2020', null)
insert into Receipt values (2, '01/02/2020', null)
insert into Receipt values (3, '01/03/2020', null)
insert into Receipt values (4, '01/04/2020', null)
insert into Receipt values (5, '01/05/2020', null)
insert into Receipt values (6, '01/06/2020', null)
insert into Receipt values (7, '01/07/2020', null)
insert into Receipt values (8, '01/08/2020', null)
insert into Receipt values (9, '01/09/2020', null)
insert into Receipt values (10, '01/10/2020', null)