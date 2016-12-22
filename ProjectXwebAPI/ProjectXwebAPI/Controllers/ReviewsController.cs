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

namespace ProjectXwebAPI.Controllers
{
    public class ReviewsController : ApiController
    {
        private ProjectXdatabaseEntities db = new ProjectXdatabaseEntities();

        // GET: api/Reviews
        public IQueryable<Review> GetReviews()
        {
            return db.Reviews;
        }

        // GET: api/Reviews/range/1/5
        public IQueryable<ReviewVM> GetReviewsByRange(int start, int end)
        {
            IQueryable<Review> reviews = db.Reviews.OrderByDescending(r => r.ReviewId);
            List<ReviewVM> reviewVMs = new List<ReviewVM>();
            ReviewVM reviewVM;

            if (start < 1)
            {
                start = 1;
            }

            if (end > reviews.Count())
            {
                end = reviews.Count();
            }

            int begin = start - 1;

            foreach (var review in reviews.Skip(begin).Take(end - begin))
            {
                reviewVM = new ReviewVM(review);
                reviewVMs.Add(reviewVM);
            }

            return reviewVMs.AsQueryable();
        }

        // GET: api/Reviews/user/U
        public IQueryable<ReviewVM> GetReviewsByUser(string userId)
        {
            IQueryable<Review> reviews = from r in db.Reviews where r.UserId.Equals(userId) select r;
            List<ReviewVM> reviewVMs = new List<ReviewVM>();
            ReviewVM reviewVM;

            foreach (var review in reviews)
            {
                reviewVM = new ReviewVM(review);
                reviewVMs.Add(reviewVM);
            }

            return reviewVMs.AsQueryable();
        }

        // GET: api/Reviews/rank/20
        public IQueryable<ReviewVM> GetReviewsByRank(int top)
        {
            IQueryable<Review> reviews =
                db.Reviews.OrderByDescending(r => SqlFunctions.Exp((double) (r.Score/r.RateCount)));
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

        // GET: api/Reviews/search/N
        public IQueryable<ReviewVM> GetReviewsByName(string name)
        {
            IQueryable<Review> reviews = from r in db.Reviews
                where r.ReviewTitle.Contains(name) || r.Slug.Contains(name)
                select r;
            List<ReviewVM> reviewVMs = new List<ReviewVM>();
            ReviewVM reviewVM;

            foreach (var review in reviews)
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

            var slugCount = db.Reviews.Count(r => r.Slug.StartsWith(review.Slug));

            if (slugCount > 0)
            {
                review.Slug += slugCount;
            }

            db.Reviews.Add(review);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = review.ReviewId }, review);
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