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
using ProjectXwebAPI.Utils;

namespace ProjectXwebAPI.Controllers
{
    public class ChaptersController : ApiController
    {
        private ProjectXdatabaseEntities db = new ProjectXdatabaseEntities();

        // GET: api/Chapters
        public IQueryable<ChapterVM> GetChapters()
        {
            List<ChapterVM> chapterVMs = new List<ChapterVM>();
            ChapterVM chapterVM;

            foreach (var chapter in db.Chapters)
            {
                chapterVM = new ChapterVM(chapter);
                chapterVMs.Add(chapterVM);
            }

            return chapterVMs.AsQueryable();
        }

        // GET: api/Chapters/story/S
        [Route("api/Chapters/story/{slug}")]
        public IQueryable<ChapterVM> GetChaptersByStory(string slug)
        {
            IQueryable<Chapter> chapters = from c in db.Chapters where c.Story.Slug.Equals(slug) select c;
            List<ChapterVM> chapterVMs = new List<ChapterVM>();
            ChapterVM chapterVM;

            foreach (var chapter in chapters)
            {
                chapterVM = new ChapterVM(chapter);
                chapterVMs.Add(chapterVM);
            }

            return chapterVMs.AsQueryable();
        }

        // GET: api/Chapters/search/S/N
        [Route("api/Chapters/search/{story}/{keyword}")]
        public IQueryable<ChapterStoryVM> GetChaptersBySearch(string story, string keyword)
        {
            string name = keyword;
            int number;
            int.TryParse(keyword, out number);
            string[] keywords = keyword.Split(' ');
            if (keywords.Length > 0)
            {
                int.TryParse(keywords[keywords.Length - 1], out number);
            }
            IQueryable<Chapter> chapters =
                db.Chapters.Where(
                    c => c.Story.Slug.Equals(story) && (c.ChapterTitle.Contains(name) || c.ChapterNumber == number));
            List<ChapterStoryVM> chapterVMs = new List<ChapterStoryVM>();
            ChapterStoryVM chapterVM;

            foreach (var chapter in chapters)
            {
                chapterVM = new ChapterStoryVM(chapter);
                chapterVMs.Add(chapterVM);
            }

            return chapterVMs.AsQueryable();
        }

        // GET: api/Chapters/range/S/1/10
        [Route("api/Chapters/range/{slug}/{start}/{end}")]
        public IQueryable<ChapterStoryVM> GetChaptersByRange(string slug, int start, int end)
        {
            IQueryable<Chapter> chapters = from c in db.Chapters
                where c.Story.Slug.Equals(slug)
                orderby c.ChapterNumber
                select c;
            List<ChapterStoryVM> chapterVMs = new List<ChapterStoryVM>();
            ChapterStoryVM chapterVM;

            int total = chapters.Count();

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

            foreach (var chapter in chapters.Skip(begin).Take(end - begin))
            {
                chapterVM = new ChapterStoryVM(chapter);
                chapterVMs.Add(chapterVM);
            }

            return chapterVMs.AsQueryable();
        }

        // GET: api/Chapters/S/0/1/10
        [Route("api/Chapters/{slug}/{status}/{start}/{end}")]
        public IQueryable<ChapterStoryVM> GetChaptersByStatus(string slug, int status, int start, int end)
        {
            IQueryable<Chapter> chapters = from c in db.Chapters
                where c.Story.Slug.Equals(slug) && c.ChapterStatus == status
                orderby c.ChapterNumber
                select c;
            List<ChapterStoryVM> chapterVMs = new List<ChapterStoryVM>();
            ChapterStoryVM chapterVM;

            int total = chapters.Count();

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

            foreach (var chapter in chapters.Skip(begin).Take(end - begin))
            {
                chapterVM = new ChapterStoryVM(chapter);
                chapterVMs.Add(chapterVM);
            }

            return chapterVMs.AsQueryable();
        }

        // GET: api/Chapters/5
        [ResponseType(typeof(ChapterVM))]
        public IHttpActionResult GetChapter(int id)
        {
            Chapter chapter = db.Chapters.Find(id);
            if (chapter == null)
            {
                return NotFound();
            }
            ChapterVM chapterVM = new ChapterVM(chapter);

            return Ok(chapterVM);
        }

        // GET: api/Chapters/S/1
        [Route("api/Chapters/{story}/{number}")]
        public IQueryable<ChapterVM> GetChapterByNumber(string story, int number)
        {
            IQueryable<Chapter> chapters =
                db.Chapters.Where(c => c.Story.Slug.Equals(story) && c.ChapterNumber == number);
            List<ChapterVM> chapterVMs = new List<ChapterVM>();
            ChapterVM chapterVM;

            foreach (var chapter in chapters)
            {
                chapterVM = new ChapterVM(chapter);
                chapterVMs.Add(chapterVM);
            }

            return chapterVMs.AsQueryable();
        }

        // PUT: api/Chapters/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutChapter(int id, ChapterVM chapterVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != chapterVM.ChapterId)
            {
                return BadRequest();
            }

            Chapter chapter = db.Chapters.Find(id);
            ConvertChapterVM(ref chapter, chapterVM);

            db.Entry(chapter).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChapterExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            chapterVM.Update(chapter);

            return StatusCode(HttpStatusCode.OK);
        }

        // POST: api/Chapters
        [ResponseType(typeof(ChapterVM))]
        public IHttpActionResult PostChapter(ChapterVM chapterVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Chapter chapter = new Chapter();
            ConvertChapterVM(ref chapter, chapterVM);

            db.Chapters.Add(chapter);
            db.SaveChanges();

            chapterVM.Update(chapter);

            return CreatedAtRoute("DefaultApi", new { id = chapterVM.ChapterId }, chapterVM);
        }

        // DELETE: api/Chapters/5
        [ResponseType(typeof(ChapterVM))]
        public IHttpActionResult DeleteChapter(int id)
        {
            Chapter chapter = db.Chapters.Find(id);
            if (chapter == null)
            {
                return NotFound();
            }

            //db.Chapters.Remove(chapter);
            chapter.ChapterStatus = -1;
            db.Entry(chapter).State = EntityState.Modified;
            db.SaveChanges();

            ChapterVM chapterVM = new ChapterVM(chapter);

            return Ok(chapterVM);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ChapterExists(int id)
        {
            return db.Chapters.Count(e => e.ChapterId == id) > 0;
        }

        private void ConvertChapterVM(ref Chapter chapter, ChapterVM chapterVM)
        {
            chapterVM.Slug = SlugUtil.GenerateSlug(chapterVM.ChapterTitle);

            chapter.StoryId = chapterVM.StoryId;
            chapter.ChapterNumber = chapterVM.ChapterNumber;
            chapter.ChapterTitle = chapterVM.ChapterTitle;
            chapter.ChapterContent = chapterVM.ChapterContent;
            chapter.ChapterStatus = chapterVM.ChapterStatus;
            chapter.UploadedDate = chapterVM.UploadedDate;
            chapter.LastEditedDate = chapterVM.LastEditedDate;
            chapter.UserId = chapterVM.UserId;
            chapter.Slug = chapterVM.Slug;

            var slugCount = db.Chapters.Count(c => c.Slug.StartsWith(chapterVM.Slug));

            if (slugCount > 0)
            {
                chapter.Slug += slugCount;
            }
        }
    }
}