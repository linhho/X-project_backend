USE [ProjectXdatabase]
GO
INSERT INTO [dbo].[Authors]
           ([AuthorName]
           ,[Slug])
     VALUES
           (N'Nguyễn Nhật Ánh',N'nguyen-nhat-anh')
GO
INSERT INTO [dbo].[Authors]
           ([AuthorName]
           ,[Slug])
     VALUES
           (N'Tô Hoài',N'to-hoai')
GO
INSERT INTO [dbo].[Authors]
           ([AuthorName]
           ,[Slug])
     VALUES
           (N'Andersen',N'Andersen')
GO
INSERT INTO [dbo].[Authors]
           ([AuthorName]
           ,[Slug])
     VALUES
           (N'Trần Đăng Khoa',N'tran-dang-khoa')
GO
INSERT INTO [dbo].[Authors]
           ([AuthorName]
           ,[Slug])
     VALUES
           (N'Nguyên Ngọc',N'nguyen-ngoc')
GO

INSERT INTO [dbo].[Genres]
           ([GenreName]
           ,[Slug])
     VALUES
           (N'Tiên hiệp','tien-hiep')
GO
INSERT INTO [dbo].[Genres]
           ([GenreName]
           ,[Slug])
     VALUES
           (N'Kiếm hiệp','kiem-hiep')
GO
INSERT INTO [dbo].[Genres]
           ([GenreName]
           ,[Slug])
     VALUES
           (N'Thiếu nhi ','thieu-nhi')
GO
INSERT INTO [dbo].[Genres]
           ([GenreName]
           ,[Slug])
     VALUES
           (N'Nước ngoài','nuoc-ngoai')
GO


INSERT INTO [dbo].[Stories]
           ([StoryName]
           ,[StoryProgress]
           ,[StoryDescription]
           ,[StoryStatus]
           ,[AuthorId]
           ,[CreatedDate]
           ,[LastEditedDate]
           ,[UserId]
           ,[Rating]
           ,[Slug])
     VALUES
           (N'Tôi thấy hoa vàng trên cỏ xanh',-1,N'truyện ngắn đặc sắc nhất về tuổi học trò của Nguyễn Nhật Ánh',-1,1,'20120618 10:34:09 AM','20120618 11:34:09 AM',1,5,N'toi-thay-hoa-vang-tren-co-xanh')
GO

INSERT INTO [dbo].[Stories]
           ([StoryName]
           ,[StoryProgress]
           ,[StoryDescription]
           ,[StoryStatus]
           ,[AuthorId]
           ,[CreatedDate]
           ,[LastEditedDate]
           ,[UserId]
           ,[Rating]
           ,[Slug])
     VALUES
           (N'Dế mèn phiêu lưu ký',0,N'truyện ngắn đặc sắc nhất về tuổi học trò của Tô Hoài',1,2,'20120618 10:34:09 AM','20120618 11:34:09 AM',2,5,N'de-men-phieu-luu-ky')
GO

INSERT INTO [dbo].[Stories]
           ([StoryName]
           ,[StoryProgress]
           ,[StoryDescription]
           ,[StoryStatus]
           ,[AuthorId]
           ,[CreatedDate]
           ,[LastEditedDate]
           ,[UserId]
           ,[Rating]
           ,[Slug])
     VALUES
           (N'Cô bé bán diêm',0,N'truyện ngắn đặc sắc nhất về tuổi học trò của Andersen',1,3,'20120618 10:34:09 AM','20120618 11:34:09 AM',3,5,N'co-be-ban-diem')
GO
INSERT INTO [dbo].[Stories]
           ([StoryName]
           ,[StoryProgress]
           ,[StoryDescription]
           ,[StoryStatus]
           ,[AuthorId]
           ,[CreatedDate]
           ,[LastEditedDate]
           ,[UserId]
           ,[Rating]
           ,[Slug])
     VALUES
           (N'Góc sân và khoảng trời',0,N'truyện ngắn đặc sắc nhất về tuổi học trò của Trần Đăng Khoa ',1,4,'20120618 10:34:09 AM','20120618 11:34:09 AM',4,5,N'goc-san-va-khoang-troi')
GO

INSERT INTO [dbo].[Stories]
           ([StoryName]
           ,[StoryProgress]
           ,[StoryDescription]
           ,[StoryStatus]
           ,[AuthorId]
           ,[CreatedDate]
           ,[LastEditedDate]
           ,[UserId]
           ,[Rating]
           ,[Slug])
     VALUES
           (N'Rừng xà nu',0,N'truyện ngắn đặc sắc nhất về tuổi học trò của Nguyên Ngọc',1,5,'20120618 10:34:09 AM','20120618 11:34:09 AM',3,5,N'rung-xa-nu')
GO




INSERT INTO [dbo].[StoryGenre]
           ([StoryId]
           ,[GenreId])
     VALUES
           (1,1)
GO

INSERT INTO [dbo].[StoryGenre]
           ([StoryId]
           ,[GenreId])
     VALUES
           (2,2)
GO


INSERT INTO [dbo].[StoryGenre]
           ([StoryId]
           ,[GenreId])
     VALUES
           (3,3)
GO


INSERT INTO [dbo].[StoryGenre]
           ([StoryId]
           ,[GenreId])
     VALUES
           (4,4)
GO

INSERT INTO [dbo].[StoryGenre]
           ([StoryId]
           ,[GenreId])
     VALUES
           (5,4)
GO




USE [ProjectXdatabase]
GO

INSERT INTO [dbo].[Chapters]
           ([StoryId]
           ,[ChapterNumber]
           ,[ChapterTitle]
           ,[ChapterContent]
           ,[UploadedDate]
           ,[LastEditedDate]
           ,[UserId]
           ,[Slug])
     VALUES
           (1,1,N'chương đầu',N'Nội dung chương I','2016-12-13','2016-12-14',1,'chuong-dau')
GO

INSERT INTO [dbo].[Chapters]
           ([StoryId]
           ,[ChapterNumber]
           ,[ChapterTitle]
           ,[ChapterContent]
           ,[UploadedDate]
           ,[LastEditedDate]
           ,[UserId]
           ,[Slug])
     VALUES
           (2,1,N'chương đầu',N'Nội dung chương I','2016-12-13','2016-12-14',2,'chuong-dau')
GO

INSERT INTO [dbo].[Chapters]
           ([StoryId]
           ,[ChapterNumber]
           ,[ChapterTitle]
           ,[ChapterContent]
           ,[UploadedDate]
           ,[LastEditedDate]
           ,[UserId]
           ,[Slug])
     VALUES
           (3,1,N'chương đầu',N'Nội dung chương I','2016-12-13','2016-12-14',3,'chuong-dau')
GO

INSERT INTO [dbo].[Chapters]
           ([StoryId]
           ,[ChapterNumber]
           ,[ChapterTitle]
           ,[ChapterContent]
           ,[UploadedDate]
           ,[LastEditedDate]
           ,[UserId]
           ,[Slug])
     VALUES
           (4,1,N'chương đầu',N'Nội dung chương I','2016-12-13','2016-12-14',4,'chuong-dau')
GO

INSERT INTO [dbo].[Chapters]
           ([StoryId]
           ,[ChapterNumber]
           ,[ChapterTitle]
           ,[ChapterContent]
           ,[UploadedDate]
           ,[LastEditedDate]
           ,[UserId]
           ,[Slug])
     VALUES
           (5,1,N'chương đầu',N'Nội dung chương I','2016-12-13','2016-12-14',5,'chuong-dau')
GO


INSERT INTO [dbo].[UserWatch]
           ([UserId]
           ,[StoryId])
     VALUES
           (1,1)
GO
INSERT INTO [dbo].[UserWatch]
           ([UserId]
           ,[StoryId])
     VALUES
           (2,2)
GO

INSERT INTO [dbo].[UserWatch]
           ([UserId]
           ,[StoryId])
     VALUES
           (3,3)
GO

INSERT INTO [dbo].[UserWatch]
           ([UserId]
           ,[StoryId])
     VALUES
           (4,4)
GO

INSERT INTO [dbo].[UserWatch]
           ([UserId]
           ,[StoryId])
     VALUES
           (5,5)
GO

INSERT INTO [dbo].[Reviews]
           ([ReviewTitle]
           ,[ReviewContent]
           ,[ReviewStatus]
           ,[CreatedDate]
           ,[LastEditedDate]
           ,[UserId]
           ,[Rating]
           ,[Slug])
     VALUES
           (N'Bài review 1',N'Nội dung bài review 1',1,'2016-11-12','2016-11-13',1,5,N'bai-review-1')
GO

INSERT INTO [dbo].[Reviews]
           ([ReviewTitle]
           ,[ReviewContent]
           ,[ReviewStatus]
           ,[CreatedDate]
           ,[LastEditedDate]
           ,[UserId]
           ,[Rating]
           ,[Slug])
     VALUES
           (N'Bài review 2',N'Nội dung bài review 2',1,'2016-11-12','2016-11-13',2,5,N'bai-review-2')
GO

INSERT INTO [dbo].[Reviews]
           ([ReviewTitle]
           ,[ReviewContent]
           ,[ReviewStatus]
           ,[CreatedDate]
           ,[LastEditedDate]
           ,[UserId]
           ,[Rating]
           ,[Slug])
     VALUES
           (N'Bài review 3',N'Nội dung bài review 3',1,'2016-11-12','2016-11-13',3,5,N'bai-review-3')
GO

INSERT INTO [dbo].[Reviews]
           ([ReviewTitle]
           ,[ReviewContent]
           ,[ReviewStatus]
           ,[CreatedDate]
           ,[LastEditedDate]
           ,[UserId]
           ,[Rating]
           ,[Slug])
     VALUES
           (N'Bài review 4',N'Nội dung bài review 4',1,'2016-11-12','2016-11-13',4,5,N'bai-review-2')
GO

INSERT INTO [dbo].[Reviews]
           ([ReviewTitle]
           ,[ReviewContent]
           ,[ReviewStatus]
           ,[CreatedDate]
           ,[LastEditedDate]
           ,[UserId]
           ,[Rating]
           ,[Slug])
     VALUES
           (N'Bài review 5',N'Nội dung bài review 5',1,'2016-11-12','2016-11-13',5,5,N'bai-review-5')
GO




