﻿using FluentAssertions;
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
    public class UserTest : DatabaseTest
    {
        [Fact]
        public void TestCreateAndRetrieveUser()
        {
            var user = new User("joe", null, null, "test@email.com", "Joe", "Mama", "test", "usr/pics/joe.png", DateTime.Now);
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
            DbContext.Users.Add(user1);
            DbContext.Users.Add(user2);

            Action save = () => DbContext.SaveChanges();
            save.Should().Throw<DbUpdateException>();
        }

        [Fact]
        public void TestCreateUsersSameUsername()
        {
            var user1 = new User("billybob", null, null, "test2@email.com", "Billy", "Bob", "test", "usr/pics/billybob.png", DateTime.Now);
            var user2 = new User("billybob", null, null, "other@email.com", "Billard", "Robertson", "test", "usr/pics/billybob.png", DateTime.Now);
            DbContext.Users.Add(user1);
            DbContext.Users.Add(user2);

            Action save = () => DbContext.SaveChanges();
            save.Should().Throw<DbUpdateException>();
        }
    }
}
