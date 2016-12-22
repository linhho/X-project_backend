using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProjectXwebAPI.Models;

namespace ProjectXwebAPI.ViewModels
{
    public class StoryVM
    {
        public int StoryId { get; set; }
        public string StoryName { get; set; }
        public int StoryProgress { get; set; }
        public string StoryDescription { get; set; }
        public int StoryStatus { get; set; }
        public AuthorVM Author { get; set; }
        public IEnumerable<GenreVM> Genres { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<System.DateTime> LastEditedDate { get; set; }
        public string UserId { get; set; }
        public int Score { get; set; }
        public int RateCount { get; set; }
        public string Image { get; set; }
        public string Slug { get; set; }

        public IEnumerable<ChapterStoryVM> Chapters { get; set; }

        public StoryVM() { }

        public StoryVM(Story story)
        {
            ConvertStory(story);
        }

        public void Update(Story story)
        {
            ConvertStory(story);
        }

        private void ConvertStory(Story story)
        {
            this.StoryId = story.StoryId;
            this.StoryName = story.StoryName;
            this.StoryProgress = story.StoryProgress;
            this.StoryStatus = story.StoryStatus;
            this.StoryDescription = story.StoryDescription;
            this.Author = new AuthorVM(story.Author);
            this.Genres = story.Genres.Select(g =>
                new GenreVM
                {
                    GenreId = g.GenreId,
                    GenreName = g.GenreName,
                    GenreStatus = g.GenreStatus,
                    Slug = g.Slug
                });
            this.CreatedDate = story.CreatedDate;
            this.LastEditedDate = story.LastEditedDate;
            this.UserId = story.UserId;
            this.Score = story.Score;
            this.RateCount = story.RateCount;
            this.Image = story.Image;
            this.Slug = story.Slug;

            this.Chapters = from c in story.Chapters
                orderby c.ChapterNumber
                select
                new ChapterStoryVM
                {
                    ChapterId = c.ChapterId,
                    ChapterNumber = c.ChapterNumber,
                    ChapterTitle = c.ChapterTitle,
                    Slug = c.Slug,
                    StoryId = c.StoryId
                };
        }
    }
}