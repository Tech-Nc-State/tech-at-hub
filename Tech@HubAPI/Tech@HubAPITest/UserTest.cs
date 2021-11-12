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
    public class UserTest : DatabaseTest
    {
        [Fact]
        public void TestCreateAndRetrieveUser()
        {
            var user = new User("joe", null, null, "test@email.com", "Joe", "Mama", 18, "test", "usr/pics/joe.png");
            user.Password = new byte[]{ 1, 2, 3, 4, 5, 6 };
            user.Salt = new byte[]{ 6, 5, 4, 3, 2, 1 };
            DbContext.Users.Add(user);
            DbContext.SaveChanges();

            user = DbContext.Users
                .Where(e => e.Username == "joe")
                .First();

            user.Username.Should().Be("joe");
            user.Password.Should().Equal(new byte[] { 1, 2, 3, 4, 5, 6 });
            user.Salt.Should().Equal(new byte[] { 6, 5, 4, 3, 2, 1 });
            user.Email.Should().Be("test@email.com");
            user.FirstName.Should().Be("Joe");
            user.LastName.Should().Be("Mama");
            user.Age.Should().Be(18);
            user.Description.Should().Be("test");
            user.ProfilePicturePath.Should().Be("usr/pics/joe.png");

        }

    }
}
