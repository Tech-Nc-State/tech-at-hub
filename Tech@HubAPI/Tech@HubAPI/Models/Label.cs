using System;
using System.Drawing;

namespace Tech_HubAPI.Models
{
    public class Label
    {
        public Label(int id, int labelTitle, int color)
        {
            Id = id;
            LabelTitle = labelTitle;
            Color = color;
        }

        public int Id { get; set; }

        public int LabelTitle { get; set; }

        // Hexcode possibly
        public int Color { get; set; }
    }
}

