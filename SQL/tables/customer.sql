use facturacion
go
create table customer(
id int identity(1,1),
names varchar(40) not null,
surnames varchar(40) not null,
identification varchar(10) not null,
address text not null,
email varchar(50) not null,
phone varchar(14) not null,
created_at datetime not null,
updated_at datetime null,
status bit not null
)
