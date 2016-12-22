using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProjectXwebAPI.Models;

namespace ProjectXwebAPI.ViewModels
{
    public class AuthorVM
    {
        public AuthorVM() { }

        public int AuthorId { get; set; }
        public string AuthorName { get; set; }
        public int AuthorStatus { get; set; }
        public string Slug { get; set; }

        public AuthorVM(Author author)
        {
            ConvertAuthor(author);
        }

        public void Update(Author author)
        {
            ConvertAuthor(author);
        }

        private void ConvertAuthor(Author author)
        {
            this.AuthorId = author.AuthorId;
            this.AuthorName = author.AuthorName;
            this.AuthorStatus = author.AuthorStatus;
            this.Slug = author.Slug;
        }
    }
}