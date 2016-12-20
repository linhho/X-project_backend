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
            IQueryable<StoryVM> storyVMs =
                db.Stories.Select(
                    s =>
                        new StoryVM
                        {
                            StoryId = s.StoryId,
                            StoryName = s.StoryName,
                            StoryProgress = s.StoryProgress,
                            StoryStatus = s.StoryStatus,
                            StoryDescription = s.StoryDescription,
                            Author =
                                new AuthorVM
                                {
                                    AuthorId = s.AuthorId,
                                    AuthorName = s.Author.AuthorName,
                                    AuthorStatus = s.Author.AuthorStatus,
                                    Slug = s.Author.Slug
                                },
                            Genres = s.Genres.Select(g =>
                                new GenreVM
                                {
                                    GenreId = g.GenreId,
                                    GenreName = g.GenreName,
                                    GenreStatus = g.GenreStatus,
                                    Slug = g.Slug
                                }),
                            CreatedDate = s.CreatedDate,
                            LastEditedDate = s.LastEditedDate,
                            UserId = s.UserId,
                            Score = s.Score,
                            RateCount = s.RateCount,
                            Image = s.Image,
                            Slug = s.Slug,
                            Chapters = s.Chapters.Select(c =>
                                new ChapterStoryVM
                                {
                                    ChapterId = c.ChapterId,
                                    ChapterNumber = c.ChapterNumber,
                                    ChapterTitle = c.ChapterTitle,
                                    Slug = c.Slug,
                                    StoryId = c.StoryId
                                })
                        });
            return storyVMs;
        }

        // GET: api/Stories/range/1/5
        public IQueryable<StorySearchVM> GetStoriesByRange(int start, int end)
        {
            IQueryable<StorySearchVM> storyVMs =
                db.Stories.Select(
                    s =>
                        new StorySearchVM
                        {
                            StoryId = s.StoryId,
                            StoryName = s.StoryName,
                            StoryProgress = s.StoryProgress,
                            StoryStatus = s.StoryStatus,
                            Author =
                                new AuthorVM
                                {
                                    AuthorId = s.AuthorId,
                                    AuthorName = s.Author.AuthorName,
                                    AuthorStatus = s.Author.AuthorStatus,
                                    Slug = s.Author.Slug
                                },
                            Genres = s.Genres.Select(g =>
                                new GenreVM
                                {
                                    GenreId = g.GenreId,
                                    GenreName = g.GenreName,
                                    GenreStatus = g.GenreStatus,
                                    Slug = g.Slug
                                }),
                            UserId = s.UserId,
                            Score = s.Score,
                            RateCount = s.RateCount,
                            Image = s.Image,
                            Slug = s.Slug
                        });

            var q = storyVMs.OrderByDescending(s => s.StoryId);

            if (start < 1)
            {
                start = 1;
            }

            if (end > q.Count())
            {
                end = q.Count();
            }

            int begin = start - 1;

            return q.Skip(begin).Take(end - begin);
        }

        // GET: api/Stories/author/A
        [Route("api/Stories/author/{name}", Name = "AuthorApi")]
        public IQueryable<StorySearchVM> GetStoriesByAuthor(string name)
        {
            IQueryable<Story> stories = from s in db.Stories
                where s.Author.AuthorName.Contains(name) || s.Author.Slug.Contains(name)
                select s;

            IQueryable<StorySearchVM> storyVMs = stories.Select(
                s =>
                    new StorySearchVM
                    {
                        StoryId = s.StoryId,
                        StoryName = s.StoryName,
                        StoryProgress = s.StoryProgress,
                        StoryStatus = s.StoryStatus,
                        Author =
                            new AuthorVM
                            {
                                AuthorId = s.AuthorId,
                                AuthorName = s.Author.AuthorName,
                                AuthorStatus = s.Author.AuthorStatus,
                                Slug = s.Author.Slug
                            },
                        Genres = s.Genres.Select(g =>
                            new GenreVM
                            {
                                GenreId = g.GenreId,
                                GenreName = g.GenreName,
                                GenreStatus = g.GenreStatus,
                                Slug = g.Slug
                            }),
                        UserId = s.UserId,
                        Score = s.Score,
                        RateCount = s.RateCount,
                        Image = s.Image,
                        Slug = s.Slug
                    });
            return storyVMs;
        }

        // GET: api/Stories/user/U
        public IQueryable<StorySearchVM> GetStoriesByUser(string userId)
        {
            IQueryable<Story> stories = from s in db.Stories
                where s.UserId.Equals(userId)
                select s;

            IQueryable<StorySearchVM> storyVMs = stories.Select(
                s =>
                    new StorySearchVM
                    {
                        StoryId = s.StoryId,
                        StoryName = s.StoryName,
                        StoryProgress = s.StoryProgress,
                        StoryStatus = s.StoryStatus,
                        Author =
                            new AuthorVM
                            {
                                AuthorId = s.AuthorId,
                                AuthorName = s.Author.AuthorName,
                                AuthorStatus = s.Author.AuthorStatus,
                                Slug = s.Author.Slug
                            },
                        Genres = s.Genres.Select(g =>
                            new GenreVM
                            {
                                GenreId = g.GenreId,
                                GenreName = g.GenreName,
                                GenreStatus = g.GenreStatus,
                                Slug = g.Slug
                            }),
                        UserId = s.UserId,
                        Score = s.Score,
                        RateCount = s.RateCount,
                        Image = s.Image,
                        Slug = s.Slug
                    });
            return storyVMs;
        }

        // GET: api/Stories/rank/20
        public IQueryable<StorySearchVM> GetStoriesByRank(int top)
        {
            IQueryable<StorySearchVM> storyVMs =
                db.Stories.Select(
                    s =>
                        new StorySearchVM
                        {
                            StoryId = s.StoryId,
                            StoryName = s.StoryName,
                            StoryProgress = s.StoryProgress,
                            StoryStatus = s.StoryStatus,
                            Author =
                                new AuthorVM
                                {
                                    AuthorId = s.AuthorId,
                                    AuthorName = s.Author.AuthorName,
                                    AuthorStatus = s.Author.AuthorStatus,
                                    Slug = s.Author.Slug
                                },
                            Genres = s.Genres.Select(g =>
                                new GenreVM
                                {
                                    GenreId = g.GenreId,
                                    GenreName = g.GenreName,
                                    GenreStatus = g.GenreStatus,
                                    Slug = g.Slug
                                }),
                            UserId = s.UserId,
                            Score = s.Score,
                            RateCount = s.RateCount,
                            Image = s.Image,
                            Slug = s.Slug
                        });

            var q = storyVMs.OrderByDescending(s => SqlFunctions.Exp((double) s.Score/s.RateCount));

            if (top > q.Count())
            {
                top = q.Count();
            }

            return q.Take(top);
        }

        // GET: api/Stories/genre/G
        [Route("api/Stories/genre/{name}", Name = "GenreApi")]
        public IQueryable<StorySearchVM> GetStoriesByGenre(string name)
        {
            IQueryable<Story> stories = from s in db.Stories
                where s.Genres.Count(g => g.GenreName.Contains(name) || g.Slug.Contains(name)) > 0
                select s;

            IQueryable<StorySearchVM> storyVMs = stories.Select(
                s =>
                    new StorySearchVM
                    {
                        StoryId = s.StoryId,
                        StoryName = s.StoryName,
                        StoryProgress = s.StoryProgress,
                        StoryStatus = s.StoryStatus,
                        Author =
                            new AuthorVM
                            {
                                AuthorId = s.AuthorId,
                                AuthorName = s.Author.AuthorName,
                                AuthorStatus = s.Author.AuthorStatus,
                                Slug = s.Author.Slug
                            },
                        Genres = s.Genres.Select(g =>
                            new GenreVM
                            {
                                GenreId = g.GenreId,
                                GenreName = g.GenreName,
                                GenreStatus = g.GenreStatus,
                                Slug = g.Slug
                            }),
                        UserId = s.UserId,
                        Score = s.Score,
                        RateCount = s.RateCount,
                        Image = s.Image,
                        Slug = s.Slug
                    });
            return storyVMs;
        }

        // GET: api/Stories/search/S
        public IQueryable<StorySearchVM> GetStoriesByName(string name)
        {
            IQueryable<Story> stories = from s in db.Stories
                where s.StoryName.Contains(name) || s.Slug.Contains(name)
                select s;

            IQueryable<StorySearchVM> storyVMs = stories.Select(
                s =>
                    new StorySearchVM
                    {
                        StoryId = s.StoryId,
                        StoryName = s.StoryName,
                        StoryProgress = s.StoryProgress,
                        StoryStatus = s.StoryStatus,
                        Author =
                            new AuthorVM
                            {
                                AuthorId = s.AuthorId,
                                AuthorName = s.Author.AuthorName,
                                AuthorStatus = s.Author.AuthorStatus,
                                Slug = s.Author.Slug
                            },
                        Genres = s.Genres.Select(g =>
                            new GenreVM
                            {
                                GenreId = g.GenreId,
                                GenreName = g.GenreName,
                                GenreStatus = g.GenreStatus,
                                Slug = g.Slug
                            }),
                        UserId = s.UserId,
                        Score = s.Score,
                        RateCount = s.RateCount,
                        Image = s.Image,
                        Slug = s.Slug
                    });
            return storyVMs;
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
            IQueryable<StoryVM> storyVMs =
                db.Stories.Select(
                    s =>
                        new StoryVM
                        {
                            StoryId = s.StoryId,
                            StoryName = s.StoryName,
                            StoryProgress = s.StoryProgress,
                            StoryStatus = s.StoryStatus,
                            StoryDescription = s.StoryDescription,
                            Author =
                                new AuthorVM
                                {
                                    AuthorId = s.AuthorId,
                                    AuthorName = s.Author.AuthorName,
                                    AuthorStatus = s.Author.AuthorStatus,
                                    Slug = s.Author.Slug
                                },
                            Genres = s.Genres.Select(g =>
                                new GenreVM
                                {
                                    GenreId = g.GenreId,
                                    GenreName = g.GenreName,
                                    GenreStatus = g.GenreStatus,
                                    Slug = g.Slug
                                }),
                            CreatedDate = s.CreatedDate,
                            LastEditedDate = s.LastEditedDate,
                            UserId = s.UserId,
                            Score = s.Score,
                            RateCount = s.RateCount,
                            Image = s.Image,
                            Slug = s.Slug,
                            Chapters = s.Chapters.Select(c =>
                                new ChapterStoryVM
                                {
                                    ChapterId = c.ChapterId,
                                    ChapterNumber = c.ChapterNumber,
                                    ChapterTitle = c.ChapterTitle,
                                    Slug = c.Slug,
                                    StoryId = c.StoryId
                                })
                        }).Where(s => s.Slug.Equals(slug));
            return storyVMs;
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