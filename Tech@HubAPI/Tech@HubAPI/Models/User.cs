using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tech_HubAPI.Models
{
    public class User
    {
        public User(string username, byte[] password, byte[] salt, string email, string firstName, string lastName, 
            uint age, string description, string profilePicturePath, DateTime birthDate)
        {
            Username = username;
            Password = password;
            Salt = salt;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            Age = age;
            Description = description;
            ProfilePicturePath = profilePicturePath;
            BirthDate = birthDate;
        }

        public int Id { get; set; }

        public string Username { get; set; }

        public byte[] Password { get; set; }

        public byte[] Salt { get; set; }

        public string Email {
            get
            {
                return this.Email;
            }
            set
            {
                if (!Email.Contains('@') || !Email.Contains('.') || Email.IndexOf('@') > Email.IndexOf('.'))
                {
                    throw new ArgumentException("Invalid email.");
                }
            }
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public uint Age { get; set; }

        public string Description { get; set; }

        public string ProfilePicturePath { get; set; }

        public DateTime BirthDate { get; set; }
    }
}
