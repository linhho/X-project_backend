using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProjectXwebAPI.Models;

/* HOANG VAN HUNG*/
namespace ProjectXwebAPI.ViewModels
{
    public class ChapterVM
    {
        public int ChapterId { get; set; }
        public int StoryId { get; set; }
        public int ChapterNumber { get; set; }
        public string ChapterTitle { get; set; }
        public string ChapterContent { get; set; }
        public int ChapterStatus { get; set; }
        public System.DateTime UploadedDate { get; set; }
        public Nullable<System.DateTime> LastEditedDate { get; set; }
        public string UserId { get; set; }
        public string Slug { get; set; }

        public ChapterVM() { }

        public ChapterVM(Chapter chapter)
        {
            ConvertChapter(chapter);
        }

        public void Update(Chapter chapter)
        {
            ConvertChapter(chapter);
        }

        private void ConvertChapter(Chapter chapter)
        {
            this.ChapterId = chapter.ChapterId;
            this.StoryId = chapter.StoryId;
            this.ChapterNumber = chapter.ChapterNumber;
            this.ChapterTitle = chapter.ChapterTitle;
            this.ChapterContent = chapter.ChapterContent;
            this.ChapterStatus = chapter.ChapterStatus;
            this.UploadedDate = chapter.UploadedDate;
            this.LastEditedDate = chapter.LastEditedDate;
            this.UserId = chapter.UserId;
            this.Slug = chapter.Slug;
        }
    }
}