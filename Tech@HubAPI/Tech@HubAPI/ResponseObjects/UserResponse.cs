using System;
using Tech_HubAPI.Models;

namespace Tech_HubAPI.ResponseObjects
{
    /// <summary>
    /// A class to shape the JSON response object sent back to the frontend.
    /// This is structured to make sure we safely send back non-sensitive data.
    /// 
    /// Contains a constructor for User to make this class essentially act as a wrapper.
    /// </summary>
    public class UserResponse
    {
        public UserResponse (string username, string email, string firstName,
           string lastName, string description, string? profilePicturePath)
        {
            Username = username;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            Description = description;
            ProfilePicturePath = profilePicturePath;
        }

        public UserResponse(User user)
        {
            Username = user.Username;
            Email = user.Email; 
            FirstName = user.FirstName;
            LastName = user.LastName;
            Description = user.Description;
            ProfilePicturePath = user.ProfilePicturePath;
        }

        public int Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Description { get; set; }

        public string? ProfilePicturePath { get; set; }

    }
}
