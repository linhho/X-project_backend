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
    public class GenresController : ApiController
    {
        private ProjectXdatabaseEntities db = new ProjectXdatabaseEntities();

        // GET: api/Genres
        public IQueryable<GenreVM> GetGenres()
        {
            List<GenreVM> genreVMs = new List<GenreVM>();
            GenreVM genreVM;

            foreach (var genre in db.Genres)
            {
                genreVM = new GenreVM(genre);
                genreVMs.Add(genreVM);
            }

            return genreVMs.AsQueryable();
        }

        // GET: api/Genres/0/1/5
        public IQueryable<GenreVM> GetGenresByStatus(int status, int start, int end)
        {
            IQueryable<Genre> genres = from g in db.Genres where g.GenreStatus == status orderby g.GenreName select g;
            List<GenreVM> genreVMs = new List<GenreVM>();
            GenreVM genreVM;

            int total = genres.Count();

            if (start < 1 || start >= total || end < 1)
            {
                start = 1;
                end = 0;
            }

            if (end > total)
            {
                end = total;
            }

            int begin = start - 1;

            foreach (var genre in genres.Skip(begin).Take(end - begin))
            {
                genreVM = new GenreVM(genre);
                genreVMs.Add(genreVM);
            }

            return genreVMs.AsQueryable();
        }

        // GET: api/Genres/5
        [ResponseType(typeof(GenreVM))]
        public IHttpActionResult GetGenre(int id)
        {
            Genre genre = db.Genres.Find(id);

            if (genre == null)
            {
                return NotFound();
            }
            GenreVM genreVM = new GenreVM(genre);

            return Ok(genreVM);
        }

        // GET: api/Genres/name/N
        public IQueryable<GenreVM> GetGenreByName(string slug)
        {
            IQueryable<Genre> genres = db.Genres.Where(g => g.Slug.Equals(slug));
            List<GenreVM> genreVMs = new List<GenreVM>();
            GenreVM genreVM;

            foreach (var genre in genres)
            {
                genreVM = new GenreVM(genre);
                genreVMs.Add(genreVM);
            }

            return genreVMs.AsQueryable();
        }

        // PUT: api/Genres/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutGenre(int id, GenreVM genreVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != genreVM.GenreId)
            {
                return BadRequest();
            }

            Genre genre = db.Genres.Find(id);
            ConvertGenreVM(ref genre, genreVM);

            db.Entry(genre).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GenreExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            genreVM.Update(genre);

            return StatusCode(HttpStatusCode.OK);
        }

        // POST: api/Genres
        [ResponseType(typeof(GenreVM))]
        public IHttpActionResult PostGenre(GenreVM genreVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Genre genre = new Genre();
            ConvertGenreVM(ref genre, genreVM);

            db.Genres.Add(genre);
            db.SaveChanges();

            genreVM.Update(genre);

            return CreatedAtRoute("DefaultApi", new { id = genreVM.GenreId }, genreVM);
        }

        // DELETE: api/Genres/5
        [ResponseType(typeof(GenreVM))]
        public IHttpActionResult DeleteGenre(int id)
        {
            Genre genre = db.Genres.Find(id);
            if (genre == null)
            {
                return NotFound();
            }

            //db.Genres.Remove(genre);
            genre.GenreStatus = -1;
            db.Entry(genre).State = EntityState.Modified;
            db.SaveChanges();

            GenreVM genreVM = new GenreVM(genre);

            return Ok(genreVM);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool GenreExists(int id)
        {
            return db.Genres.Count(e => e.GenreId == id) > 0;
        }

        private void ConvertGenreVM(ref Genre genre, GenreVM genreVM)
        {
            genre.GenreName = genreVM.GenreName;
            genre.GenreStatus = genreVM.GenreStatus;
            genre.Slug = genreVM.Slug;

            var slugCount = db.Genres.Count(g => g.Slug.StartsWith(genreVM.Slug));

            if (slugCount > 0)
            {
                genre.Slug += slugCount;
            }
        }
    }
}