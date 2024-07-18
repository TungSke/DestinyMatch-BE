use master;
go
if exists (select name from sys.databases where name = 'DestinyMatch')
begin
	drop database DestinyMatch;
end;
go
create database DestinyMatch;
go
use DestinyMatch;
go

--dotnet ef dbcontext scaffold "Server=(local);database=DestinyMatch;uid=sa;pwd=12345;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer --output-dir Models --force
--=====================================================================

create table [University]
(
	Id uniqueidentifier default newid() primary key,
	Code nvarchar(20) unique,
	[Name] nvarchar(max)
);
go

create table [Major]
(
	Id uniqueidentifier default newid() primary key,
	Code nvarchar(max),
	[Name] nvarchar(max)
);
go

create table [Hobby]
(
	Id uniqueidentifier default newid() primary key,
	[Name] nvarchar(50) unique,
	[Description] nvarchar(max)
);
go

create table [Account]
(
	Id uniqueidentifier default newid() primary key,
	Email nvarchar(100) not null,
	[Password] nvarchar(max),
	FcmtToken nvarchar(max),
	ReceiveNotifiEMail bit not null default 0,
	[CreateAt] datetime default CURRENT_TIMESTAMP,
	[Role] nvarchar(20) not null default 'member',				--1:admin   2:moderator   3:member
	[Status] nvarchar(20) not null default 'newbie'						--newbie	experienced		working		deleted		banned
);
go

create table [Member]
(
	Id uniqueidentifier default newid() primary key,
	Fullname nvarchar(100),
	Introduce nvarchar(max) default N'Tên này rất lười, chả để lại lời nói gì cả!',
	Dob date,
	Gender bit,					--0:girl   1:boy
	[Address] nvarchar(max) default N'Ở trên mặt đất, ở dưới bầu trời! :3',
	Surplus int default 0,				--money bag
	[Status] nvarchar(30) default N'Chưa Xác Thực',

	AccountId uniqueidentifier unique not null foreign key references [Account](Id),
	UniversityId uniqueidentifier not null foreign key references [University](Id),
	MajorId uniqueidentifier not null foreign key (MajorId) references [Major](Id)
);
--increase query speed , decrease insert, update speed
create index idx_UniversityId on [Member](UniversityId);
create index idx_MajorId on [Member](MajorId);
create index idx_Gender on [Member](Gender);
go

create table [HobbyMember]
(
	HobbyId uniqueidentifier not null,
	MemberId uniqueidentifier not null,

	primary key (HobbyId, MemberId),
	foreign key (HobbyId) references [Hobby](Id),
	foreign key (MemberId) references [Member](Id)
);
create index idx_HobbyId on [HobbyMember](HobbyId);
go

create table [Picture]
(
	Id uniqueidentifier default newid() primary key,
	UrlPath nvarchar(max),
	IsAvatar bit,
	[Status] nvarchar(30),

	MemberId uniqueidentifier not null foreign key references [Member](Id)
);
create index idx_MemberId on [Picture](MemberId);
go

create table [Package]
(
	Id uniqueidentifier default newid() primary key,
	Code nvarchar(20) unique,
	[Name] nvarchar(50),
	[Description] nvarchar(max),
	Price int,
	Duration int,
	[Status] nvarchar(30)
);
go

create table [MemberPackage]
(
	Id uniqueidentifier default newid() primary key,--Buy again same Package
	StartDate datetime default CURRENT_TIMESTAMP,
	EndDate datetime,
	[Status] nvarchar(30),
	
	MemberId uniqueidentifier not null,
	PackageId uniqueidentifier not null,

	foreign key (MemberId) references [Member](Id),
	foreign key (PackageId) references [Package](Id)
);
go

--create table [MatchRequest]
--(
--	Id uniqueidentifier default newid() primary key,
--	[CreateAt] datetime default CURRENT_TIMESTAMP,
--	[Status] nvarchar(30) default N'Chưa Phản Hồi',

--	FromId uniqueidentifier not null foreign key references [Member](Id),
--	ToId uniqueidentifier not null foreign key references [Member](Id)
--);
--create index idx_FromId on [MatchRequest](FromId);
--create index idx_ToId on [MatchRequest](ToId);
--go

create table [Matching]
(
	Id uniqueidentifier default newid() primary key,
	[FirstName] nvarchar(50),
	[SecondName] nvarchar(50),
	[RecentlyActivity] datetime default CURRENT_TIMESTAMP,
	[CreatedAt] datetime default CURRENT_TIMESTAMP,
	[Status] nvarchar(30) default N'Chưa Phản Hồi',

	FirstMemberId uniqueidentifier not null foreign key references [Member](Id),
	SecondMemberId uniqueidentifier not null foreign key references [Member](Id)
);
create index idx_FirstMemberId on [Matching](FirstMemberId);
create index idx_SecondMemberId on [Matching](SecondMemberId);
go

create table [Message]
(
	Id uniqueidentifier default newid() primary key,
	Content nvarchar(max) not null,
	[SentAt] datetime default CURRENT_TIMESTAMP,
	[Status] nvarchar(30) default N'Đã gửi',

	MatchingId uniqueidentifier not null foreign key references [Matching](Id),
	SenderId uniqueidentifier not null foreign key references [Member](Id)
);
create index idx_SenderId on [Message](SenderId);
go

--create table [Feedback]
--(
--	Id uniqueidentifier default newid() primary key,
--	Title nvarchar(max) not null,
--	Content nvarchar(max) not null,
--	[TimeStamp] datetime default CURRENT_TIMESTAMP,
--	[Status] nvarchar(30) default N'Đã Gửi',

--	SenderId uniqueidentifier not null foreign key references [Member](Id)
--);
--go

--create table [Verification]
--(
--	Id uniqueidentifier default newid() primary key,
--	SubmittedPicture nvarchar(max),
--	[TimeStamp] datetime default CURRENT_TIMESTAMP,
--	[Status] nvarchar(30) default N'Chưa Duyệt',

--	MemberId uniqueidentifier not null foreign key (MemberId) references [Member](Id)
--);