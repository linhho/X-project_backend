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
    public class StoriesController : ApiController
    {
        private ProjectXdatabaseEntities db = new ProjectXdatabaseEntities();

        // GET: api/Stories
        public IQueryable<StoryVM> GetStories()
        {
            List<StoryVM> storyVMs = new List<StoryVM>();
            StoryVM storyVM;

            foreach (var story in db.Stories)
            {
                storyVM = new StoryVM(story);
                storyVMs.Add(storyVM);
            }

            return storyVMs.AsQueryable();
        }

        // GET: api/Stories/range/1/5
        public IQueryable<StorySearchVM> GetStoriesByRange(int start, int end)
        {
            IQueryable<Story> stories = db.Stories.OrderByDescending(s => s.StoryId);
            List<StorySearchVM> storyVMs = new List<StorySearchVM>();
            StorySearchVM storySearchVM;

            if (start < 1)
            {
                start = 1;
            }

            if (end > stories.Count())
            {
                end = stories.Count();
            }

            int begin = start - 1;

            foreach (var story in stories.Skip(begin).Take(end - begin))
            {
                storySearchVM = new StorySearchVM(story);
                storyVMs.Add(storySearchVM);
            }

            return storyVMs.AsQueryable();
        }

        // GET: api/Stories/author/A
        [Route("api/Stories/author/{name}", Name = "AuthorApi")]
        public IQueryable<StorySearchVM> GetStoriesByAuthor(string name)
        {
            IQueryable<Story> stories = from s in db.Stories
                where s.Author.AuthorName.Contains(name) || s.Author.Slug.Contains(name)
                select s;
            List<StorySearchVM> storyVMs = new List<StorySearchVM>();
            StorySearchVM storySearchVM;

            foreach (var story in stories)
            {
                storySearchVM = new StorySearchVM(story);
                storyVMs.Add(storySearchVM);
            }

            return storyVMs.AsQueryable();
        }

        // GET: api/Stories/user/U
        public IQueryable<StorySearchVM> GetStoriesByUser(string userId)
        {
            IQueryable<Story> stories = from s in db.Stories
                where s.UserId.Equals(userId)
                select s;
            List<StorySearchVM> storyVMs = new List<StorySearchVM>();
            StorySearchVM storySearchVM;

            foreach (var story in stories)
            {
                storySearchVM = new StorySearchVM(story);
                storyVMs.Add(storySearchVM);
            }

            return storyVMs.AsQueryable();
        }

        // GET: api/Stories/rank/20
        public IQueryable<StorySearchVM> GetStoriesByRank(int top)
        {
            IQueryable<Story> stories = db.Stories.OrderByDescending(s => SqlFunctions.Exp((double) s.Score/s.RateCount));
            List<StorySearchVM> storyVMs = new List<StorySearchVM>();
            StorySearchVM storySearchVM;

            if (top > stories.Count())
            {
                top = stories.Count();
            }

            foreach (var story in stories.Take(top))
            {
                storySearchVM = new StorySearchVM(story);
                storyVMs.Add(storySearchVM);
            }

            return storyVMs.AsQueryable();
        }

        // GET: api/Stories/genre/G
        [Route("api/Stories/genre/{name}", Name = "GenreApi")]
        public IQueryable<StorySearchVM> GetStoriesByGenre(string name)
        {
            IQueryable<Story> stories = from s in db.Stories
                where s.Genres.Count(g => g.GenreName.Contains(name) || g.Slug.Contains(name)) > 0
                select s;
            List<StorySearchVM> storyVMs = new List<StorySearchVM>();
            StorySearchVM storySearchVM;

            foreach (var story in stories)
            {
                storySearchVM = new StorySearchVM(story);
                storyVMs.Add(storySearchVM);
            }

            return storyVMs.AsQueryable();
        }

        // GET: api/Stories/search/S
        public IQueryable<StorySearchVM> GetStoriesByName(string name)
        {
            IQueryable<Story> stories = from s in db.Stories
                where s.StoryName.Contains(name) || s.Slug.Contains(name)
                select s;
            List<StorySearchVM> storyVMs = new List<StorySearchVM>();
            StorySearchVM storySearchVM;

            foreach (var story in stories)
            {
                storySearchVM = new StorySearchVM(story);
                storyVMs.Add(storySearchVM);
            }

            return storyVMs.AsQueryable();
        }

        // GET: api/Stories/users/10
        [Route("api/Stories/users/{top}", Name = "UserRankApi")]
        public IQueryable<String> GetUserByRank(int top)
        {
            var q = db.Stories.OrderByDescending(s => SqlFunctions.Exp((double) s.Score/s.RateCount));

            if (top > q.Count())
            {
                top = q.Count();
            }

            IQueryable<String> users = from s in q select s.UserId;

            return users.Take(top);
        }

        // GET: api/Stories/5
        [ResponseType(typeof(StoryVM))]
        public IHttpActionResult GetStory(int id)
        {
            Story story = db.Stories.Find(id);

            if (story == null)
            {
                return NotFound();
            }
            StoryVM storyVM = new StoryVM(story);

            return Ok(storyVM);
        }

        // GET: api/Stories/name/N
        public IQueryable<StoryVM> GetStoryByName(string slug)
        {
            IQueryable<Story> stories = db.Stories.Where(s => s.Slug.Equals(slug));
            List<StoryVM> storyVMs = new List<StoryVM>();
            StoryVM storyVM;

            foreach (var story in stories)
            {
                storyVM = new StoryVM(story);
                storyVMs.Add(storyVM);
            }

            return storyVMs.AsQueryable();
        }

        // PUT: api/Stories/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutStory(int id, StoryVM storyVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != storyVM.StoryId)
            {
                return BadRequest();
            }

            Story story = db.Stories.Find(id);
            ConvertStoryVM(ref story, storyVM);

            db.Entry(story).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StoryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Stories
        [ResponseType(typeof(StoryVM))]
        public IHttpActionResult PostStory(StoryVM storyVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Story story = new Story();
            ConvertStoryVM(ref story, storyVM);

            db.Stories.Add(story);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = story.StoryId }, storyVM);
        }

        // DELETE: api/Stories/5
        [ResponseType(typeof(Story))]
        public IHttpActionResult DeleteStory(int id)
        {
            Story story = db.Stories.Find(id);
            if (story == null)
            {
                return NotFound();
            }

            //db.Stories.Remove(story);
            story.StoryStatus = -1;
            db.Entry(story).State = EntityState.Modified;
            db.SaveChanges();

            return Ok(story);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StoryExists(int id)
        {
            return db.Stories.Count(e => e.StoryId == id) > 0;
        }

        private void ConvertStoryVM(ref Story story, StoryVM storyVM)
        {
            story.StoryName = storyVM.StoryName;
            story.StoryProgress = storyVM.StoryProgress;
            story.StoryStatus = storyVM.StoryStatus;
            story.StoryDescription = storyVM.StoryDescription;
            story.AuthorId = storyVM.Author.AuthorId;
            story.Author = new Author
            {
                AuthorId = storyVM.Author.AuthorId,
                AuthorName = storyVM.Author.AuthorName,
                AuthorStatus = storyVM.Author.AuthorStatus,
                Slug = storyVM.Author.Slug
            };
            story.Genres =
                storyVM.Genres.Select(
                    g =>
                        new Genre
                        {
                            GenreId = g.GenreId,
                            GenreName = g.GenreName,
                            GenreStatus = g.GenreStatus,
                            Slug = g.Slug
                        }).ToList();
            story.CreatedDate = storyVM.CreatedDate;
            story.LastEditedDate = storyVM.LastEditedDate;
            story.UserId = storyVM.UserId;
            story.Score = storyVM.Score;
            story.RateCount = storyVM.RateCount;
            story.Image = storyVM.Image;
            story.Slug = storyVM.Slug;

            var slugCount = db.Stories.Count(s => s.Slug.StartsWith(storyVM.Slug));

            if (slugCount > 0)
            {
                story.Slug += slugCount;
            }
        }
    }
}