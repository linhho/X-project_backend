using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using ProjectXwebAPI.Models;
using ProjectXwebAPI.ViewModels;
using WebGrease.Css.Extensions;
using ProjectXwebAPI.Utils;

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

            foreach (var story in db.Stories.Where(s => s.StoryStatus == 1))
            {
                storyVM = new StoryVM(story);
                storyVMs.Add(storyVM);
            }

            return storyVMs.AsQueryable();
        }

        // GET: api/Stories/range/1/5
        public IQueryable<StorySearchVM> GetStoriesByRange(int start, int end)
        {
            IQueryable<Story> stories = from s in db.Stories
                where s.StoryStatus == 1
                orderby s.StoryId descending
                select s;
            List<StorySearchVM> storyVMs = new List<StorySearchVM>();
            StorySearchVM storySearchVM;

            int total = stories.Count();

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

            foreach (var story in stories.Skip(begin).Take(end - begin))
            {
                storySearchVM = new StorySearchVM(story);
                storyVMs.Add(storySearchVM);
            }

            return storyVMs.AsQueryable();
        }

        // GET: api/Stories/author/A/1/5
        [Route("api/Stories/author/{name}/{start}/{end}", Name = "AuthorApi")]
        public IQueryable<StorySearchVM> GetStoriesByAuthor(string name, int start, int end)
        {
            IQueryable<Story> stories = from s in db.Stories
                where (s.Author.AuthorName.Equals(name) || s.Author.Slug.Equals(name)) && s.StoryStatus == 1
                orderby s.StoryName
                select s;
            List<StorySearchVM> storyVMs = new List<StorySearchVM>();
            StorySearchVM storySearchVM;

            int total = stories.Count();

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

            foreach (var story in stories.Skip(begin).Take(end - begin))
            {
                storySearchVM = new StorySearchVM(story);
                storyVMs.Add(storySearchVM);
            }

            return storyVMs.AsQueryable();
        }

        // GET: api/Stories/user/U/1/5
        public IQueryable<StorySearchVM> GetStoriesByUser(string userId, int start, int end)
        {
            IQueryable<Story> stories = from s in db.Stories
                where s.UserId.Equals(userId) && s.StoryStatus == 1
                orderby s.StoryName
                select s;
            List<StorySearchVM> storyVMs = new List<StorySearchVM>();
            StorySearchVM storySearchVM;

            int total = stories.Count();

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

            foreach (var story in stories.Skip(begin).Take(end - begin))
            {
                storySearchVM = new StorySearchVM(story);
                storyVMs.Add(storySearchVM);
            }

            return storyVMs.AsQueryable();
        }

        // GET: api/Stories/rank/20
        public IQueryable<StorySearchVM> GetStoriesByRank(int top)
        {
            IQueryable<Story> stories = from s in db.Stories
                where s.StoryStatus == 1
                orderby SqlFunctions.Exp(s.RateCount == 0 ? 0 : (double) s.Score/s.RateCount) descending
                select s;
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

        // GET: api/Stories/genre/G/1/5
        [Route("api/Stories/genre/{name}/{start}/{end}", Name = "GenreApi")]
        public IQueryable<StorySearchVM> GetStoriesByGenre(string name, int start, int end)
        {
            IQueryable<Story> stories = from s in db.Stories
                where s.Genres.Count(g => g.GenreName.Equals(name) || g.Slug.Equals(name)) > 0 && s.StoryStatus == 1
                orderby s.StoryName
                select s;
            List<StorySearchVM> storyVMs = new List<StorySearchVM>();
            StorySearchVM storySearchVM;

            int total = stories.Count();

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

            foreach (var story in stories.Skip(begin).Take(end - begin))
            {
                storySearchVM = new StorySearchVM(story);
                storyVMs.Add(storySearchVM);
            }

            return storyVMs.AsQueryable();
        }

        // GET: api/Stories/search/S/1/5
        public IQueryable<StorySearchVM> GetStoriesByName(string name, int start, int end)
        {
            IQueryable<Story> stories = from s in db.Stories
                where (s.StoryName.Contains(name) || s.Slug.Contains(name)) && s.StoryStatus == 1
                orderby s.StoryName
                select s;
            List<StorySearchVM> storyVMs = new List<StorySearchVM>();
            StorySearchVM storySearchVM;

            int total = stories.Count();

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

            foreach (var story in stories.Skip(begin).Take(end - begin))
            {
                storySearchVM = new StorySearchVM(story);
                storyVMs.Add(storySearchVM);
            }

            return storyVMs.AsQueryable();
        }

        // GET: api/Stories/0/1/5
        public IQueryable<StorySearchVM> GetStoriesByStatus(int status, int start, int end)
        {
            IQueryable<Story> stories = from s in db.Stories
                where s.StoryStatus == status
                orderby s.StoryName
                select s;
            List<StorySearchVM> storyVMs = new List<StorySearchVM>();
            StorySearchVM storySearchVM;

            int total = stories.Count();

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

            foreach (var story in stories.Skip(begin).Take(end - begin))
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
            Dictionary<string, double> users = new Dictionary<string, double>();
            string userId;
            double rating;

            foreach (var story in db.Stories)
            {
                userId = story.UserId;
                rating = story.RateCount == 0 ? 0 : story.Score/story.RateCount;

                if (users.ContainsKey(userId))
                {
                    users[userId] += rating;
                }
                else
                {
                    users.Add(userId, rating);
                }
            }

            if (top > users.Count())
            {
                top = users.Count();
            }

            var q = from u in users orderby u.Value descending select u.Key;
            return q.Take(top).AsQueryable();
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
        public async Task<IHttpActionResult> PutStory(int id, StoryVM storyVM)
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
            foreach (var genreVM in storyVM.Genres)
            {
                Genre genre = await db.Genres.FindAsync(genreVM.GenreId);
                story.Genres.Add(genre);
            }

            db.Entry(story).State = EntityState.Modified;

            try
            {
                //db.SaveChanges();
                await db.SaveChangesAsync().ConfigureAwait(false);
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

            storyVM.Update(story);

            return StatusCode(HttpStatusCode.OK);
        }

        // POST: api/Stories
        [ResponseType(typeof(StoryVM))]
        public async Task<IHttpActionResult> PostStory(StoryVM storyVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Story story = new Story();
            ConvertStoryVM(ref story, storyVM);
            foreach (var genreVM in storyVM.Genres)
            {
                Genre genre = await db.Genres.FindAsync(genreVM.GenreId);
                story.Genres.Add(genre);
            }

            db.Stories.Add(story);
            await db.SaveChangesAsync().ConfigureAwait(false);
            //db.SaveChanges();

            storyVM.Update(story);

            return CreatedAtRoute("NameApi", new { slug = storyVM.Slug }, storyVM);
        }

        // POST: api/Stories/change/5/1
        [Route("api/Stories/change/{id}/{status}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PostStoryChange(int id, int status)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Story story = db.Stories.Find(id);
            story.StoryStatus = status;

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

            return StatusCode(HttpStatusCode.OK);
        }

        // DELETE: api/Stories/5
        [ResponseType(typeof(StoryVM))]
        public IHttpActionResult DeleteStory(int id)
        {
            Story story = db.Stories.Find(id);
            if (story == null)
            {
                return NotFound();
            }

            //db.Stories.Remove(story);
            story.StoryStatus = -1;
            story.Chapters.ForEach(c => c.ChapterStatus = -1);
            db.Entry(story).State = EntityState.Modified;
            db.SaveChanges();

            StoryVM storyVM = new StoryVM(story);

            return Ok(storyVM);
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
            storyVM.Slug = SlugUtil.GenerateSlug(storyVM.StoryName);

            story.StoryName = storyVM.StoryName;
            story.StoryProgress = storyVM.StoryProgress;
            story.StoryStatus = storyVM.StoryStatus;
            story.StoryDescription = storyVM.StoryDescription;
            story.AuthorId = storyVM.Author.AuthorId;
            story.Author = db.Authors.Find(storyVM.Author.AuthorId);
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