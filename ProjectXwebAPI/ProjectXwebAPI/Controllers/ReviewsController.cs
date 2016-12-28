using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using ProjectXwebAPI.Models;
using ProjectXwebAPI.ViewModels;
using ProjectXwebAPI.Utils;

namespace ProjectXwebAPI.Controllers
{
    public class ReviewsController : ApiController
    {
        private ProjectXdatabaseEntities db = new ProjectXdatabaseEntities();

        // GET: api/Reviews
        public IQueryable<Review> GetReviews()
        {
            return db.Reviews.Where(r => r.ReviewStatus == 1);
        }

        // GET: api/Reviews/range/1/5
        public IQueryable<ReviewVM> GetReviewsByRange(int start, int end)
        {
            IQueryable<Review> reviews = from r in db.Reviews
                where r.ReviewStatus == 1
                orderby r.ReviewId descending
                select r;
            List<ReviewVM> reviewVMs = new List<ReviewVM>();
            ReviewVM reviewVM;

            int total = reviews.Count();

            if (start < 1 || (start > end && start < total) || start > total)
            {
                start = 1;
                end = 0;
            }

            if (end > total)
            {
                end = total;
            }

            int begin = start - 1;

            foreach (var review in reviews.Skip(begin).Take(end - begin))
            {
                reviewVM = new ReviewVM(review);
                reviewVMs.Add(reviewVM);
            }

            return reviewVMs.AsQueryable();
        }

        // GET: api/Reviews/user/U/1/5
        public IQueryable<ReviewVM> GetReviewsByUser(string userId, int start, int end)
        {
            IQueryable<Review> reviews = from r in db.Reviews
                where r.UserId.Equals(userId) && r.ReviewStatus == 1
                orderby r.ReviewId descending
                select r;
            List<ReviewVM> reviewVMs = new List<ReviewVM>();
            ReviewVM reviewVM;

            int total = reviews.Count();

            if (start < 1 || (start > end && start < total) || start > total)
            {
                start = 1;
                end = 0;
            }

            if (end > total)
            {
                end = total;
            }

            int begin = start - 1;

            foreach (var review in reviews.Skip(begin).Take(end - begin))
            {
                reviewVM = new ReviewVM(review);
                reviewVMs.Add(reviewVM);
            }

            return reviewVMs.AsQueryable();
        }

        // GET: api/Reviews/rank/20
        public IQueryable<ReviewVM> GetReviewsByRank(int top)
        {
            IQueryable<Review> reviews = from r in db.Reviews
                where r.ReviewStatus == 1
                orderby SqlFunctions.Exp(r.RateCount == 0 ? 0 : (double) (r.Score/r.RateCount)) descending
                select r;
            List<ReviewVM> reviewVMs = new List<ReviewVM>();
            ReviewVM reviewVM;

            if (top > reviews.Count())
            {
                top = reviews.Count();
            }

            foreach (var review in reviews.Take(top))
            {
                reviewVM = new ReviewVM(review);
                reviewVMs.Add(reviewVM);
            }

            return reviewVMs.AsQueryable();
        }

        // GET: api/Reviews/search/N/1/5
        public IQueryable<ReviewVM> GetReviewsByName(string name, int start, int end)
        {
            IQueryable<Review> reviews = from r in db.Reviews
                where (r.ReviewTitle.Contains(name) || r.Slug.Contains(name)) && r.ReviewStatus == 1
                orderby r.ReviewTitle
                select r;
            List<ReviewVM> reviewVMs = new List<ReviewVM>();
            ReviewVM reviewVM;

            int total = reviews.Count();

            if (start < 1 || (start > end && start < total) || start > total)
            {
                start = 1;
                end = 0;
            }

            if (end > total)
            {
                end = total;
            }

            int begin = start - 1;

            foreach (var review in reviews.Skip(begin).Take(end - begin))
            {
                reviewVM = new ReviewVM(review);
                reviewVMs.Add(reviewVM);
            }

            return reviewVMs.AsQueryable();
        }

        // GET: api/Reviews/0/1/5
        public IQueryable<ReviewVM> GetReviewsByStatus(int status, int start, int end)
        {
            IQueryable<Review> reviews = from r in db.Reviews
                where r.ReviewStatus == status
                orderby r.ReviewTitle
                select r;
            List<ReviewVM> reviewVMs = new List<ReviewVM>();
            ReviewVM reviewVM;

            int total = reviews.Count();

            if (start < 1 || (start > end && start < total) || start > total)
            {
                start = 1;
                end = 0;
            }

            if (end > total)
            {
                end = total;
            }

            int begin = start - 1;

            foreach (var review in reviews.Skip(begin).Take(end - begin))
            {
                reviewVM = new ReviewVM(review);
                reviewVMs.Add(reviewVM);
            }

            return reviewVMs.AsQueryable();
        }

        // GET: api/Reviews/5
        [ResponseType(typeof(Review))]
        public IHttpActionResult GetReview(int id)
        {
            Review review = db.Reviews.Find(id);
            if (review == null)
            {
                return NotFound();
            }

            return Ok(review);
        }

        // GET: api/Reviews/name/N
        public IQueryable<Review> GetReviewByName(string slug)
        {
            return db.Reviews.Where(r => r.Slug.Equals(slug));
        }

        // PUT: api/Reviews/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutReview(int id, Review review)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != review.ReviewId)
            {
                return BadRequest();
            }

            review.Slug = SlugUtil.GenerateSlug(review.ReviewTitle);

            var slugCount = db.Reviews.Count(r => r.Slug.StartsWith(review.Slug));

            if (slugCount > 0)
            {
                review.Slug += slugCount;
            }

            db.Entry(review).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReviewExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.OK);
        }

        // POST: api/Reviews
        [ResponseType(typeof(Review))]
        public IHttpActionResult PostReview(Review review)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            review.Slug = SlugUtil.GenerateSlug(review.ReviewTitle);

            var slugCount = db.Reviews.Count(r => r.Slug.StartsWith(review.Slug));

            if (slugCount > 0)
            {
                review.Slug += slugCount;
            }

            db.Reviews.Add(review);
            db.SaveChanges();

            return CreatedAtRoute("NameApi", new { slug = review.Slug }, review);
        }

        // POST: api/Reviews/change/5/1
        [Route("api/Reviews/change/{id}/{status}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PostReviewChange(int id, int status)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Review review = db.Reviews.Find(id);
            review.ReviewStatus = status;

            db.Entry(review).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReviewExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.OK);
        }

        // DELETE: api/Reviews/5
        [ResponseType(typeof(Review))]
        public IHttpActionResult DeleteReview(int id)
        {
            Review review = db.Reviews.Find(id);
            if (review == null)
            {
                return NotFound();
            }

            //db.Reviews.Remove(review);
            review.ReviewStatus = -1;
            db.Entry(review).State = EntityState.Modified;
            db.SaveChanges();

            return Ok(review);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ReviewExists(int id)
        {
            return db.Reviews.Count(e => e.ReviewId == id) > 0;
        }
    }
}