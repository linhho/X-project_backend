﻿using System;
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
    public class ChaptersController : ApiController
    {
        private ProjectXdatabaseEntities db = new ProjectXdatabaseEntities();

        // GET: api/Chapters
        public IQueryable<ChapterVM> GetChapters()
        {
            IQueryable<ChapterVM> chapterVMs =
                db.Chapters.Select(c => new ChapterVM
                {
                    ChapterId = c.ChapterId,
                    StoryId = c.StoryId,
                    ChapterNumber = c.ChapterNumber,
                    ChapterTitle = c.ChapterTitle,
                    ChapterContent = c.ChapterContent,
                    ChapterStatus = c.ChapterStatus,
                    UploadedDate = c.UploadedDate,
                    LastEditedDate = c.LastEditedDate,
                    UserId = c.UserId,
                    Slug = c.Slug
                });
            return chapterVMs;
        }

        // GET: api/Chapters/5
        [ResponseType(typeof(ChapterVM))]
        public IHttpActionResult GetChapter(int id)
        {
            Chapter chapter = db.Chapters.Find(id);
            ChapterVM chapterVM = new ChapterVM(chapter);
            if (chapter == null)
            {
                return NotFound();
            }

            return Ok(chapterVM);
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

            return StatusCode(HttpStatusCode.NoContent);
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

            return CreatedAtRoute("DefaultApi", new { id = chapter.ChapterId }, chapterVM);
        }

        // DELETE: api/Chapters/5
        [ResponseType(typeof(Chapter))]
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

            return Ok(chapter);
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
            chapter.StoryId = chapterVM.StoryId;
            chapter.ChapterNumber = chapterVM.ChapterNumber;
            chapter.ChapterTitle = chapterVM.ChapterTitle;
            chapter.ChapterContent = chapterVM.ChapterContent;
            chapter.ChapterStatus = chapterVM.ChapterStatus;
            chapter.UploadedDate = chapterVM.UploadedDate;
            chapter.LastEditedDate = chapterVM.LastEditedDate;
            chapter.UserId = chapterVM.UserId;
            chapter.Slug = chapterVM.Slug;
        }
    }
}