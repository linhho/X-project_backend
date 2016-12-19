using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
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
            IQueryable<StoryVM> storyVMs =
                db.Stories.Select(
                    s =>
                        new StoryVM
                        {
                            StoryId = s.StoryId,
                            StoryName = s.StoryName,
                            StoryProgress = s.StoryProgress,
                            StoryDescription = s.StoryDescription,
                            StoryStatus = s.StoryStatus,
                            Author =
                                new AuthorVM
                                {
                                    AuthorId = s.AuthorId,
                                    AuthorName = s.Author.AuthorName,
                                    Slug = s.Author.Slug
                                },
                            CreatedDate = s.CreatedDate,
                            LastEditedDate = s.LastEditedDate,
                            UserId = s.UserId,
                            Score = s.Score,
                            RateCount = s.RateCount,
                            Image = s.Image,
                            Slug = s.Slug
                        });
            return storyVMs;
        }

        // GET: api/Stories/5
        [ResponseType(typeof(StoryVM))]
        public IHttpActionResult GetStory(int id)
        {
            Story story = db.Stories.Find(id);
            StoryVM storyVM = new StoryVM(story);

            if (story == null)
            {
                return NotFound();
            }

            return Ok(storyVM);
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

            db.Stories.Remove(story);
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
            story.LastEditedDate = storyVM.LastEditedDate;
            story.Score = storyVM.Score;
            story.RateCount = storyVM.RateCount;
            story.Image = storyVM.Image;
            story.Slug = storyVM.Slug;
        }
    }
}