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
    public class AuthorsController : ApiController
    {
        private ProjectXdatabaseEntities db = new ProjectXdatabaseEntities();

        // GET: api/Authors
        public IQueryable<AuthorVM> GetAuthors()
        {
            List<AuthorVM> authorsVM = new List<AuthorVM>();
            AuthorVM authorVM;

            foreach (var author in db.Authors)
            {
                authorVM = new AuthorVM(author);
                authorsVM.Add(authorVM);
            }

            return authorsVM.AsQueryable();
        }

        // GET: api/Authors/5
        [ResponseType(typeof(AuthorVM))]
        public IHttpActionResult GetAuthor(int id)
        {
            Author author = db.Authors.Find(id);
            if (author == null)
            {
                return NotFound();
            }
            AuthorVM authorVM = new AuthorVM(author);

            return Ok(authorVM);
        }

        // GET: api/Authors/name/N
        public IQueryable<AuthorVM> GetAuthorByName(string slug)
        {
            IQueryable<Author> authors = db.Authors.Where(a => a.Slug.Equals(slug));
            List<AuthorVM> authorsVM = new List<AuthorVM>();
            AuthorVM authorVM;

            foreach (var author in authors)
            {
                authorVM = new AuthorVM(author);
                authorsVM.Add(authorVM);
            }

            return authorsVM.AsQueryable();
        }

        // PUT: api/Authors/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAuthor(int id, AuthorVM authorVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != authorVM.AuthorId)
            {
                return BadRequest();
            }

            Author author = db.Authors.Find(id);
            ConvertAuthorVM(ref author, authorVM);

            db.Entry(author).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuthorExists(id))
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

        // POST: api/Authors
        [ResponseType(typeof(AuthorVM))]
        public IHttpActionResult PostAuthor(AuthorVM authorVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Author author = new Author();
            ConvertAuthorVM(ref author, authorVM);

            db.Authors.Add(author);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = author.AuthorId }, authorVM);
        }

        // DELETE: api/Authors/5
        [ResponseType(typeof(Author))]
        public IHttpActionResult DeleteAuthor(int id)
        {
            Author author = db.Authors.Find(id);
            if (author == null)
            {
                return NotFound();
            }

            //db.Authors.Remove(author);
            author.AuthorStatus = -1;
            db.Entry(author).State = EntityState.Modified;
            db.SaveChanges();

            return Ok(author);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AuthorExists(int id)
        {
            return db.Authors.Count(e => e.AuthorId == id) > 0;
        }

        private void ConvertAuthorVM(ref Author author, AuthorVM authorVM)
        {
            author.AuthorName = authorVM.AuthorName;
            author.AuthorStatus = authorVM.AuthorStatus;
            author.Slug = authorVM.Slug;

            var slugCount = db.Authors.Count(a => a.Slug.StartsWith(authorVM.Slug));

            if (slugCount > 0)
            {
                author.Slug += slugCount;
            }
        }
    }
}