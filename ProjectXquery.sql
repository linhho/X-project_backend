USE master
GO

--Create a database
IF EXISTS(SELECT name FROM sys.databases
	WHERE name = 'ProjectXdatabase')
	DROP DATABASE ProjectXdatabase
GO

CREATE DATABASE ProjectXdatabase
GO

use ProjectXdatabase

CREATE TABLE Authors
(
AuthorId int IDENTITY(1,1) PRIMARY KEY,
AuthorName nvarchar(50) NOT NULL,
Slug nvarchar(50) NOT NULL
)

CREATE TABLE Genres
(
GenreId int IDENTITY(1,1) PRIMARY KEY,
GenreName nvarchar(50) NOT NULL,
Slug nvarchar(50) NOT NULL
)

CREATE TABLE Stories
(
StoryId int IDENTITY(1,1) PRIMARY KEY,
StoryName nvarchar(50) NOT NULL,
StoryProgress smallint NOT NULL,
StoryDescription nvarchar(MAX) NOT NULL,
StoryStatus smallint NOT NULL,
AuthorId int NOT NULL,
CreatedDate datetime NOT NULL,
LastEditedDate datetime,
UserId int NOT NULL,
Rating int,
Slug nvarchar(50) NOT NULL
)

CREATE TABLE StoryGenre
(
StoryId int NOT NULL,
GenreId int NOT NULL,
PRIMARY KEY (StoryId,GenreId)
)

CREATE TABLE Chapters
(
ChapterId int IDENTITY(1,1) PRIMARY KEY,
StoryId int NOT NULL,
ChapterNumber int NOT NULL,
ChapterTitle nvarchar(50),
ChapterContent nvarchar(MAX) NOT NULL,
UploadedDate datetime NOT NULL,
LastEditedDate datetime,
UserId int NOT NULL,
Slug nvarchar(50) NOT NULL
)

CREATE TABLE Reviews
(
ReviewId int IDENTITY(1,1) PRIMARY KEY,
ReviewTitle nvarchar(50),
ReviewContent nvarchar(MAX) NOT NULL,
ReviewStatus smallint NOT NULL,
CreatedDate datetime NOT NULL,
LastEditedDate datetime,
UserId int NOT NULL,
Rating int,
Slug nvarchar(50) NOT NULL
)

CREATE TABLE UserWatch
(
UserId int NOT NULL,
StoryId int NOT NULL,
PRIMARY KEY (UserId,StoryId)
)


ALTER TABLE Stories
ADD CONSTRAINT fk_AuthorStories
FOREIGN KEY (AuthorId)
REFERENCES Authors(AuthorId)

ALTER TABLE StoryGenre
ADD CONSTRAINT fk_StoryGenres
FOREIGN KEY (StoryId)
REFERENCES Stories(StoryId)

ALTER TABLE StoryGenre
ADD CONSTRAINT fk_StoriesGenre
FOREIGN KEY (GenreId)
REFERENCES Genres(GenreId)

ALTER TABLE Chapters
ADD CONSTRAINT fk_StoryChapters
FOREIGN KEY (StoryId)
REFERENCES Stories(StoryId)

ALTER TABLE UserWatch
ADD CONSTRAINT fk_UsersWatch
FOREIGN KEY (StoryId)
REFERENCES Stories(StoryId)