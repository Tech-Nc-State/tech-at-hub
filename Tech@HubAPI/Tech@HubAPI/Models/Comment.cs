using System;
namespace Tech_HubAPI.Models
{
    public class Comment
    {
        public Comment(int id)
        {
            Id = id;
            Body = "";
        }

        public int Id { get; set; }

        public string Body { get; set; }
    }
}

