using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProjectXwebAPI.Models;

namespace ProjectXwebAPI.ViewModels
{
    public class ReviewVM
    {
        public int ReviewId { get; set; }
        public string ReviewTitle { get; set; }
        public string UserId { get; set; }
        public int Score { get; set; }
        public int RateCount { get; set; }
        public string Image { get; set; }
        public string Slug { get; set; }

        public ReviewVM() { }

        public ReviewVM(Review review)
        {
            this.ReviewId = review.ReviewId;
            this.ReviewTitle = review.ReviewTitle;
            this.UserId = review.UserId;
            this.Score = review.Score;
            this.RateCount = review.RateCount;
            this.Image = review.Image;
            this.Slug = review.Slug;
        }
    }
}