using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProjectXwebAPI.Models;

namespace ProjectXwebAPI.ViewModels
{
    public class ChapterStoryVM
    {
        public int ChapterId { get; set; }
        public int StoryId { get; set; }
        public int ChapterNumber { get; set; }
        public string ChapterTitle { get; set; }
        public string Slug { get; set; }

        public ChapterStoryVM() { }

        public ChapterStoryVM(Chapter chapter)
        {
            this.ChapterId = chapter.ChapterId;
            this.StoryId = chapter.StoryId;
            this.ChapterNumber = chapter.ChapterNumber;
            this.ChapterTitle = chapter.ChapterTitle;
            this.Slug = chapter.Slug;
        }
    }
}