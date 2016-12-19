using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProjectXwebAPI.Models;

namespace ProjectXwebAPI.ViewModels
{
    public class StorySearchVM
    {
        public int StoryId { get; set; }
        public string StoryName { get; set; }
        public int StoryProgress { get; set; }
        public int StoryStatus { get; set; }
        public AuthorVM Author { get; set; }
        public IEnumerable<GenreVM> Genres { get; set; }
        public string UserId { get; set; }
        public int Score { get; set; }
        public int RateCount { get; set; }
        public string Image { get; set; }
        public string Slug { get; set; }

        public StorySearchVM() { }

        public StorySearchVM(Story story)
        {
            this.StoryId = story.StoryId;
            this.StoryName = story.StoryName;
            this.StoryProgress = story.StoryProgress;
            this.StoryStatus = story.StoryStatus;
            this.Author = new AuthorVM(story.Author);
            this.Genres = story.Genres.Select(g =>
                new GenreVM
                {
                    GenreId = g.GenreId,
                    GenreName = g.GenreName,
                    GenreStatus = g.GenreStatus,
                    Slug = g.Slug
                });
            this.UserId = story.UserId;
            this.Score = story.Score;
            this.RateCount = story.RateCount;
            this.Image = story.Image;
            this.Slug = story.Slug;
        }
    }
}