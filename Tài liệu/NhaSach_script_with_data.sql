
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

if OBJECT_ID ('Admin', 'U') is not null
	drop table Admin
else
begin
	create table Admin
	(
		id int identity not null primary key,	--CUS001/AD001
		name nvarchar(255),
		email varchar(255),
		username varchar(255),
		password varchar(1000),
		imageName varchar(MAX),
		url varchar(MAX)
	)
end
go
insert into Admin values (N'System admin', 'huynhthanhdang77@gmail.com', 'root', '8E1EFD3B06AD62427E298137DB4179C97C22DE1A', '1_root.png', 'https://localhost:44327/Data/1_root.png')
insert into Admin values (N'1A', 'huynh@gmail.com', 'admin', '8E1EFD3B06AD62427E298137DB4179C97C22DE1A', '2_user2.png', 'https://localhost:44327/Data/2_user2.png')
insert into Admin values (N'2A', 'thanh77@gmail.com', 'noadmin', '8E1EFD3B06AD62427E298137DB4179C97C22DE1A', '3_user3.png', 'https://localhost:44327/Data/3_user3.png')

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
		dept float,
		url varchar(MAX)
	)
end
go
insert into Customer values (N'Đàn', N'Trần Thanh', '0132658479', 'thd@gmail.com', N'154 Nguyễn Chí Thanh', 'user1', '601f1889667efaebb33b8c12572835da3f027f78', 15000, null)
insert into Customer values (N'Tâm', N'Trần Thanh', '0132658479', 'thd@gmail.com', N'154 Nguyễn Chí Thanh', 'user2', '601f1889667efaebb33b8c12572835da3f027f78', 0, null)
insert into Customer values (N'Ly', N'Trần Thanh', '0132658479', 'thd@gmail.com', N'154 Nguyễn Chí Thanh', 'user3', '601f1889667efaebb33b8c12572835da3f027f78', 5000, null)
insert into Customer values (N'Nam', N'Trần Thanh', '0132658479', 'thd@gmail.com', N'154 Nguyễn Chí Thanh', 'user4', '601f1889667efaebb33b8c12572835da3f027f78', 20000, null)
insert into Customer values (N'Trung', N'Trần Thanh', '0132658479', 'thd@gmail.com', N'154 Nguyễn Chí Thanh', 'user5', '601f1889667efaebb33b8c12572835da3f027f78', 25000, null)
insert into Customer values (N'Thanh', N'Trần Thanh', '0132658479', 'thd@gmail.com', N'154 Nguyễn Chí Thanh', 'user6', '601f1889667efaebb33b8c12572835da3f027f78', 10000, null)
insert into Customer values (N'Thành', N'Trần Thanh', '0132658479', 'thd@gmail.com', N'154 Nguyễn Chí Thanh', 'user7', '601f1889667efaebb33b8c12572835da3f027f78', 5000, null)
insert into Customer values (N'Tú', N'Trần Thanh', '0132658479', 'thd@gmail.com', N'154 Nguyễn Chí Thanh', 'admin1', 'ccbe91b1f19bd31a1365363870c0eec2296a61c1', 5000, null)
insert into Customer values (N'Xuân', N'Trần Thanh', '0132658479', 'thd@gmail.com', N'154 Nguyễn Chí Thanh', 'admin2', 'ccbe91b1f19bd31a1365363870c0eec2296a61c1', 0, null)
insert into Customer values (N'Tuấn', N'Trần Thanh', '0132658479', 'thd@gmail.com', N'154 Nguyễn Chí Thanh', 'admin3', 'ccbe91b1f19bd31a1365363870c0eec2296a61c1', 0, null)

if OBJECT_ID('Book', 'U') is not null
	drop table Book
else
begin
	create table Book
	(
		id int identity not null primary key,	--PRO001
		name nvarchar(100),
		price float,
		kind nvarchar(100),
		author nvarchar(255),	
		stock int,
		state bit	--tình trạng: còn hàng/hết hàng
	)
end
go
insert into Book values (N'Thám tử lừng danh Conan (tập 95)', 25000, N'Truyện tranh', N'Huỳnh TD', 100, 1)
insert into Book values (N'Thám tử lừng danh Conan (tập 95)', 25000, N'Truyện tranh', N'Huỳnh TD', 100, 1)
insert into Book values (N'Thám tử lừng danh Conan (tập 95)', 25000, N'Truyện tranh', N'Huỳnh TD', 100, 1)
insert into Book values (N'Thám tử lừng danh Conan (tập 95)', 25000, N'Truyện tranh', N'Huỳnh TD', 100, 1)
insert into Book values (N'Thần đồng Đất Việt (tập 227)', 25000, N'Truyện tranh', N'Huỳnh TD', 100, 1)
insert into Book values (N'Thần đồng Đất Việt (tập 227)', 25000, N'Truyện tranh', N'Huỳnh TD', 100, 1)
insert into Book values (N'Thần đồng Đất Việt (tập 227)', 25000, N'Truyện tranh', N'Huỳnh TD', 100, 1)
insert into Book values (N'Thần đồng Đất Việt (tập 227)', 25000, N'Truyện tranh', N'Huỳnh TD', 100, 1)
insert into Book values (N'Truyện cổ tích Việt Nam được yêu thích nhất', 100000, N'Truyện tranh', N'Huỳnh TD', 100, 1)
insert into Book values (N'Truyện cổ tích Việt Nam được yêu thích nhất', 100000, N'Truyện tranh', N'Huỳnh TD', 100, 1)
insert into Book values (N'Truyện cổ tích Việt Nam được yêu thích nhất', 100000, N'Truyện tranh', N'Huỳnh TD', 100, 1)
insert into Book values (N'Truyện cổ tích Việt Nam được yêu thích nhất', 100000, N'Truyện tranh', N'Huỳnh TD', 100, 1)
insert into Book values (N'Công chúa và chàng ếch', 100000, N'Truyện tranh', N'Huỳnh TD', 100, 1)
insert into Book values (N'Công chúa và chàng ếch', 100000, N'Truyện tranh', N'Huỳnh TD', 100, 1)
insert into Book values (N'Công chúa và chàng ếch', 100000, N'Truyện tranh', N'Huỳnh TD', 100, 1)
insert into Book values (N'Công chúa và chàng ếch', 100000, N'Truyện tranh', N'Huỳnh TD', 100, 1)

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
insert into Receipt values (1, '01/01/2020', 250000)
insert into Receipt values (2, '01/02/2020', 300000)
insert into Receipt values (3, '01/03/2020', 150000)
insert into Receipt values (4, '01/04/2020', 50000)

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

if OBJECT_ID('SaleDetail', 'U') is not null
	drop table SaleDetail
else
begin
	create table SaleDetail
	(
		id int identity not null primary key,	--OR001
		saleInvoiceId int,
		bookId int,
		quantity int,
		totalPrice float
	)
end
go
alter table SaleDetail add constraint FK_SaleDetail_SaleInvoice foreign key(saleInvoiceId) references SaleInvoice(id)
alter table SaleDetail add constraint FK_SaleDetail_Book foreign key(bookId) references Book(id)
insert into SaleDetail values (1, 1, 2, 50000)
insert into SaleDetail values (1, 2, 2, 50000)
insert into SaleDetail values (1, 3, 2, 50000)
insert into SaleDetail values (1, 4, 2, 50000)
insert into SaleDetail values (1, 5, 2, 50000)
insert into SaleDetail values (2, 3, 4, 100000)
insert into SaleDetail values (2, 5, 4, 100000)
insert into SaleDetail values (2, 7, 4, 100000)
insert into SaleDetail values (3, 9, 2, 50000)
insert into SaleDetail values (3, 1, 2, 50000)
insert into SaleDetail values (3, 3, 2, 50000)
insert into SaleDetail values (4, 3, 2, 50000)

if OBJECT_ID('Input', 'U') is not null
	drop table Input
else
begin
	create table Input
	(
		id int identity not null primary key,	--INI001
		bookId int,
		quantity int
	)
end
go
alter table Input add constraint FK_Input_Book foreign key(bookId) references Book(id)
insert into Input values (1, 50)
insert into Input values (1, 15)
insert into Input values (2, 50)
insert into Input values (2, 25)
insert into Input values (3, 50)
insert into Input values (4, 50)
insert into Input values (5, 55)
insert into Input values (1, 50)
insert into Input values (2, 50)
insert into Input values (3, 45)
insert into Input values (3, 50)
insert into Input values (3, 50)
insert into Input values (2, 45)
insert into Input values (3, 20)
insert into Input values (7, 35)
insert into Input values (9, 50)
insert into Input values (5, 30)
insert into Input values (3, 50)