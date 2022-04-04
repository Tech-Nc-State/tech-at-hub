using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tech_HubAPI.Models;
using Xunit;

namespace Tech_HubAPITest
{
    [Collection("DatabaseCollection")]
    public class TestBookAndAuthor
    {
        private readonly DatabaseService _db;

        public TestBookAndAuthor(DatabaseService db)
        {
            _db = db;
        }

        [Fact]
        public void TestCreateAndRetrieveAuthor()
        {
            var author = new Author("Ken");
            _db.DbContext.Authors.Add(author);
            _db.DbContext.SaveChanges();

            author = _db.DbContext.Authors
                .Where(a => a.Name == "Ken")
                .First();

            author.Name.Should().Be("Ken");
        }

        [Fact]
        public void TestCreateAndRetrieveBook()
        {
            var author = new Author("Ken");
            _db.DbContext.Authors.Add(author);
            _db.DbContext.SaveChanges();

            var book1 = new Book("Harry Potter", DateTime.Now, author.Id);
            var book2 = new Book("Star Wars", DateTime.Now, author.Id);
            _db.DbContext.Books.Add(book1);
            _db.DbContext.Books.Add(book2);
            _db.DbContext.SaveChanges();

            author = _db.DbContext.Authors
                .Where(a => a.Name == "Ken")
                .First();

            author.Books.Count.Should().Be(2);
            author.Books[0].Title.Should().Be("Harry Potter");
            author.Books[1].Title.Should().Be("Star Wars");
        }

        [Fact]
        public void TestDuplicateBookName()
        {
            var author = new Author("Ken");
            _db.DbContext.Authors.Add(author);
            _db.DbContext.SaveChanges();

            var book1 = new Book("Harry Potter", DateTime.Now, author.Id);
            var book2 = new Book("Harry Potter", DateTime.Now, author.Id);
            _db.DbContext.Books.Add(book1);
            _db.DbContext.Books.Add(book2);

            Action save = () => _db.DbContext.SaveChanges();
            save.Should().Throw<DbUpdateException>();
        }
    }
}
