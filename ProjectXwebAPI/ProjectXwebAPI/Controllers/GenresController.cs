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
            IQueryable<GenreVM> genreVMs =
                db.Genres.Select(
                    g =>
                        new GenreVM
                        {
                            GenreId = g.GenreId,
                            GenreName = g.GenreName,
                            GenreStatus = g.GenreStatus,
                            Slug = g.Slug
                        });
            return genreVMs;
        }

        // GET: api/Genres/5
        [ResponseType(typeof(GenreVM))]
        public IHttpActionResult GetGenre(int id)
        {
            Genre genre = db.Genres.Find(id);
            GenreVM genreVM = new GenreVM(genre);

            if (genre == null)
            {
                return NotFound();
            }

            return Ok(genre);
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

            return StatusCode(HttpStatusCode.NoContent);
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

            return CreatedAtRoute("DefaultApi", new { id = genre.GenreId }, genre);
        }

        // DELETE: api/Genres/5
        [ResponseType(typeof(Genre))]
        public IHttpActionResult DeleteGenre(int id)
        {
            Genre genre = db.Genres.Find(id);
            if (genre == null)
            {
                return NotFound();
            }

            db.Genres.Remove(genre);
            db.SaveChanges();

            return Ok(genre);
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
        }
    }
}