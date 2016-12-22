using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProjectXwebAPI.Models;

namespace ProjectXwebAPI.ViewModels
{
    public class GenreVM
    {
        public GenreVM() { }

        public GenreVM(Genre genre)
        {
            ConvertGenre(genre);
        }

        public void Update(Genre genre)
        {
            ConvertGenre(genre);
        }

        private void ConvertGenre(Genre genre)
        {
            this.GenreId = genre.GenreId;
            this.GenreName = genre.GenreName;
            this.GenreStatus = genre.GenreStatus;
            this.Slug = genre.Slug;
        }

        public int GenreId { get; set; }
        public string GenreName { get; set; }
        public int GenreStatus { get; set; }
        public string Slug { get; set; }
    }
}