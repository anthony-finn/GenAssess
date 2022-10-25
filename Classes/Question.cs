using PdfSharp.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GenAssess
{
    public class Question
    {
        enum Categories
        {
            Creativity,
            Abstraction,
            DataAndInformation,
            Algorithms,
            Programming,
            Internet,
            GlobalImpact,
            Nonassigned,
        }

        public string Text { get; set; }
        public string[] Answers { get; set; }
        public string[] Choices { get; set; }
        public int Category { get; set; }
        public string ImagePath { get; set; }

        public Question(string Text, string[] Answers, string[] Choices, int Category = (int)Categories.Nonassigned, string ImagePath = null)
        {
            this.Text = Text;
            this.Answers = Answers;
            this.Choices = Choices;
            this.Category = Category;
            this.ImagePath = ImagePath;
        }

        public Question() {}
    }
}
