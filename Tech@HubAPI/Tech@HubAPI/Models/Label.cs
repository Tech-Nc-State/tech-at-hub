using System;
using System.Drawing;

namespace Tech_HubAPI.Models
{
    public class Label
    {
        public Label(int id, string title, string color)
        {
            Id = id;
            Title = title;
            Color = color;
        }

        public int Id { get; set; }

        public string Title { get; set; }

        // Hexcode possibly
        public string Color { get; set; }
    }
}

