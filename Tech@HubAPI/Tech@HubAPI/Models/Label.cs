using System;
using System.Drawing;

namespace Tech_HubAPI.Models
{
    public class Label
    {
        public Label(string title, Color color)
        {
            Title = title;
            Color = color;
        }

        public string Title { get; set; }
        
        public Color Color { get; set; }
    }
}

