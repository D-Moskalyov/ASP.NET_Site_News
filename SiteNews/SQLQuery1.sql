CREATE DATABASE NewsSite
--DROP DATABASE NewsSite
USE NewsSite
--USE ChatInfo

CREATE TABLE Users
(
	UserID int IDENTITY(1,1) PRIMARY KEY,
	Name varchar(255),
	Pass varchar(255)
)

CREATE TABLE News
(
	NewsID int IDENTITY(1,1) PRIMARY KEY,
	UserName varchar(255),
	Title varchar(255),
	Contect text,
	Raiting int,
	CreateDate DateTime
)

CREATE TABLE Comments
(
	CommentID int IDENTITY(1,1) PRIMARY KEY,
	NewsID int,
	UserID int,
	Comment text,
	CreateDate DateTime
)

CREATE TABLE Images
(
	ImageID int IDENTITY(1,1) PRIMARY KEY,
	NewsID int,
	PathIMG varchar(255)
)

ALTER TABLE Comments ADD 
CONSTRAINT FK_Comments_News FOREIGN KEY(NewsID)
	REFERENCES News(NewsID);

ALTER TABLE Comments ADD 
CONSTRAINT FK_Comments_Users FOREIGN KEY(UserID)
	REFERENCES Users(UserID);

ALTER TABLE Images ADD 
CONSTRAINT FK_Images_News FOREIGN KEY(NewsID)
	REFERENCES News(NewsID);

ALTER TABLE News ADD 
CONSTRAINT FK_News_Users FOREIGN KEY(UserName)
	REFERENCES Users(Name);

select * from Users
select * from News
select * from Comments
select * from Images

drop table Comments

insert into Users (Name, Pass) values('test2', '96e79218965eb72c92a549dd5a330112')

insert into News (UserName, Title, Contect, Raiting) values('Dima', 'First', 'saffdsgs', 0)

insert into Comments (NewsID, UserID, Comment, CreateDate) values(1, 1, 'My First Comment', '2015-03-08')
insert into Comments (NewsID, UserID, Comment, CreateDate) values(1, 1, 'My Second Comment', '2015-03-09')

select Name from Users where UserID = 1

update News set Title = 'modify' where Title = ''

/*INSERT INTO Users (NickName, Pass, Ban) 
VALUES ('777', 777, '1900-01-01')

INSERT INTO Users (NickName, Pass, Ban) 
VALUES ('888', 888, '1900-01-01')

INSERT INTO Users (NickName, Pass, Ban) 
VALUES ('999', 999, '1900-01-01')

INSERT INTO Users (NickName, Pass, Ban) 
VALUES ('666', 666, '1900-01-01')

INSERT INTO Users (NickName, Pass, Ban) 
VALUES ('555', 555, '1900-01-01')

INSERT INTO Groups (GroupName) 
VALUES ('sport')

INSERT INTO Groups (GroupName) 
VALUES ('movie')

INSERT INTO Groups (GroupName) 
VALUES ('music')

INSERT INTO Groups (GroupName) 
VALUES ('education')

INSERT INTO Groups (GroupName) 
VALUES ('science')

INSERT INTO Groups (GroupName) 
VALUES ('GENERAL')

INSERT INTO MemeberOfGroups(UserID, GroupID) 
VALUES (1, 6)
INSERT INTO MemeberOfGroups(UserID, GroupID) 
VALUES (2, 6)
INSERT INTO MemeberOfGroups(UserID, GroupID) 
VALUES (3, 6)
INSERT INTO MemeberOfGroups(UserID, GroupID) 
VALUES (4, 6)
INSERT INTO MemeberOfGroups(UserID, GroupID) 
VALUES (5, 6)

--insert into Messagese (GroupID, Texts, UserID, TimeMes)
--values (6, 'sdc', 1, '03.01.2015 20:40:59')

select COUNT(*) from Messagese

select * from Users;
select * from Messagese;
select * from Groups;
select * from MemeberOfGroups;
select * from BanList;
--select * from IndividualMessages;

--delete from Users
--delete from Messagese
--delete from Groups
--delete from MemeberOfGroups
--delete from BanList
--delete from IndividualMessages




--SELECT SCOPE_IDENTITY() from Users;
--select IDENT_CURRENT('Users') as ID_cur

--select * from Users where NickName=888;

--delete from Users where UserID=8 or UserID=1016;
--delete from MemeberOfGroups where UserID=1016 or UserID=1016;

--delete from BanList where UserID1=2 and UserID2=1;

--update Users set Ban = '1900-01-01';

--INSERT INTO MemeberOfGroups(UserID, GroupID) 
--VALUES (1, 6)
--INSERT INTO MemeberOfGroups(UserID, GroupID) 
--VALUES (2, 6)*/

