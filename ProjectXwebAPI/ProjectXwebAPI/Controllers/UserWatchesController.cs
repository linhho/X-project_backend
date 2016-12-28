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
    [RoutePrefix("api/UserWatches")]
    public class UserWatchesController : ApiController
    {
        private ProjectXdatabaseEntities db = new ProjectXdatabaseEntities();

        // GET: api/UserWatches
        [Route("")]
        public IQueryable<UserWatchVM> GetUserWatches()
        {
            List<UserWatchVM> userWatchVMs = new List<UserWatchVM>();
            UserWatchVM userWatchVM;

            foreach (var userWatch in db.UserWatches)
            {
                userWatchVM = new UserWatchVM(userWatch);
                userWatchVMs.Add(userWatchVM);
            }

            return userWatchVMs.AsQueryable();
        }

        // GET: api/UserWatches/5/1
        [Route("{userId}/{storyId}", Name = "UserWatchApi")]
        [ResponseType(typeof(UserWatchVM))]
        public IHttpActionResult GetUserWatch(string userId, int storyId)
        {
            UserWatch userWatch = db.UserWatches.SingleOrDefault(uw => uw.UserId.Equals(userId) && uw.StoryId == storyId);
            if (userWatch == null)
            {
                return NotFound();
            }

            return Ok(new UserWatchVM(userWatch));
        }

        // PUT: api/UserWatches/5
        /*[ResponseType(typeof(void))]
        public IHttpActionResult PutUserWatch(string id, UserWatch userWatch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != userWatch.UserId)
            {
                return BadRequest();
            }

            db.Entry(userWatch).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserWatchExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }*/

        // POST: api/UserWatches/5/1
        [Route("{userId}/{storyId}")]
        [ResponseType(typeof(UserWatchVM))]
        public IHttpActionResult PostUserWatch(string userId, int storyId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            UserWatch userWatch = new UserWatch();
            userWatch.UserId = userId;
            userWatch.StoryId = storyId;

            db.UserWatches.Add(userWatch);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (UserWatchExists(userWatch.UserId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            UserWatchVM userWatchVM = new UserWatchVM(userWatch);

            return CreatedAtRoute("UserWatchApi", new { userId = userWatch.UserId, storyId = userWatch.StoryId }, userWatchVM);
        }

        // DELETE: api/UserWatches/5/1
        [Route("{userId}/{storyId}")]
        [ResponseType(typeof(UserWatchVM))]
        public IHttpActionResult DeleteUserWatch(string userId, int storyId)
        {
            UserWatch userWatch = db.UserWatches.SingleOrDefault(uw => uw.UserId.Equals(userId) && uw.StoryId == storyId);
            if (userWatch == null)
            {
                return NotFound();
            }

            db.UserWatches.Remove(userWatch);
            db.SaveChanges();

            return Ok(new UserWatchVM(userWatch));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserWatchExists(string id)
        {
            return db.UserWatches.Count(e => e.UserId == id) > 0;
        }

    }
}