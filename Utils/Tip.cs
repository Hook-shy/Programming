﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Programming.Utils
{
    public class Tip
    {
        public Tip()
        {
        }

        public Tip(string title, string smallTitle, string content, string textclass, int wait)
        {
            Title = title;
            SmallTitle = smallTitle;
            Content = content;
            TextClass = textclass;
            Wait = wait;
        }

        public string Title { get; set; }
        public string SmallTitle { get; set; }
        public string Content { get; set; }
        public int Wait { get; set; }
        public string TextClass { get; set; }
    }
}
