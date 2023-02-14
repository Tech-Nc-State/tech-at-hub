using System;
using System.Drawing;

namespace Tech_HubAPI.Models
{
    public class Label
    {
        public Label(int id, string labelTitle, string color)
        {
            Id = id;
            LabelTitle = labelTitle;
            Color = color;
        }

        public int Id { get; set; }

        public string LabelTitle { get; set; }

        // Hexcode possibly
        public string Color { get; set; }
    }
}

