using System;
using System.Linq;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Tech_HubAPI.Models;
using Tech_HubAPITest.Services;
using Xunit;


namespace Tech_HubAPITest
{
    [Collection("DatabaseFileSystemCollection")]
    public class UserTest
    {
        private readonly DatabaseService _db;

        public UserTest(DatabaseService db)
        {
            _db = db;
        }

        [Fact]
        public void TestCreateAndRetrieveUser()
        {
            var user = new User("joe", null, null, "test@email.com", "Joe", "Mama", "test", "usr/pics/joe.png", DateTime.Now);
            user.Password = new byte[] { 1, 2, 3, 4, 5, 6 };
            user.Salt = new byte[] { 6, 5, 4, 3, 2, 1 };
            _db.DbContext.Users.Add(user);
            _db.DbContext.SaveChanges();

            user = _db.DbContext.Users
                .Where(e => e.Username == "joe")
                .First();

            user.Username.Should().Be("joe");
            user.Password.Should().Equal(new byte[] { 1, 2, 3, 4, 5, 6 });
            user.Salt.Should().Equal(new byte[] { 6, 5, 4, 3, 2, 1 });
            user.Email.Should().Be("test@email.com");
            user.FirstName.Should().Be("Joe");
            user.LastName.Should().Be("Mama");
            user.Description.Should().Be("test");
            user.ProfilePicturePath.Should().Be("usr/pics/joe.png");

        }

        [Fact]
        public void TestCreateDuplicateUsers()
        {
            var user1 = new User("billybob", null, null, "test2@email.com", "Billy", "Bob", "test", "usr/pics/billybob.png", DateTime.Now);
            var user2 = new User("billybob", null, null, "test2@email.com", "Billy", "Bob", "test", "usr/pics/billybob.png", DateTime.Now);
            user1.Password = new byte[] { 1, 2, 3, 4, 5, 6 };
            user1.Salt = new byte[] { 6, 5, 4, 3, 2, 1 };
            _db.DbContext.Users.Add(user1);
            _db.DbContext.Users.Add(user2);

            Action save = () => _db.DbContext.SaveChanges();
            save.Should().Throw<DbUpdateException>();
        }

        [Fact]
        public void TestCreateUsersSameUsername()
        {
            var user1 = new User("billybob", null, null, "test2@email.com", "Billy", "Bob", "test", "usr/pics/billybob.png", DateTime.Now);
            var user2 = new User("billybob", null, null, "other@email.com", "Billard", "Robertson", "test", "usr/pics/billybob.png", DateTime.Now);
            _db.DbContext.Users.Add(user1);
            _db.DbContext.Users.Add(user2);

            Action save = () => _db.DbContext.SaveChanges();
            save.Should().Throw<DbUpdateException>();
        }
    }
}
