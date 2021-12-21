using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using Tech_HubAPI.Models;

namespace Tech_HubAPI.Controllers
{
    public static class ControllerBaseExtensions
    {
        /// <summary>
        /// Returns the user who made the current request. If the current user is unauthenticated,
        /// returns null.
        /// </summary>
        /// <param name="controller">Current controller class.</param>
        /// <param name="dbContext">A valid <see cref="DatabaseContext"/> to retrieve the
        /// user from the database.</param>
        /// <returns>User who made the current request.</returns>
        /// <exception cref="Exception">If there was an error parsing the users identifier from
        /// the JWT token.</exception>
        public static User GetUser(this ControllerBase controller, DatabaseContext dbContext)
        {
            if (controller.User == null)
            {
                return null;
            }

            try
            {
                int userId = int.Parse(controller.User.FindFirst("Id").Value);
                var user = dbContext.Users.Find(userId);

                if (user == null)
                {
                    return null;
                }

                // detach the entity so the caller doesn't need to worry about accidentally modifying it
                dbContext.Entry(user).State = EntityState.Detached;
                return user;
            }
            catch
            {
                throw new Exception("Unable to retrieve user ID from the JWT token passed in this request.");
            }
        }
    }
}
