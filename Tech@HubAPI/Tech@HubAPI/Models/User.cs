﻿using System;
using System.Collections.Generic;

namespace Tech_HubAPI.Models
{
    public class User
    {
        public User(string username, byte[] password, byte[] salt, string email, string firstName,
            string lastName, string description, string? profilePicturePath, DateTime birthDate)
        {
            Username = username;
            Password = password;
            Salt = salt;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            Description = description;
            ProfilePicturePath = profilePicturePath;
            BirthDate = birthDate;
            Permissions = new List<RepositoryPermission>();
            Repositories = new List<Repository>();
        }

        public int Id { get; set; }

        public string Username { get; set; }

        public byte[] Password { get; set; }

        public byte[] Salt { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Description { get; set; }

        public string? ProfilePicturePath { get; set; }

        public DateTime BirthDate { get; set; }

        public List<RepositoryPermission> Permissions { get; set; }

        public List<Repository> Repositories { get; set; }
    }
}
