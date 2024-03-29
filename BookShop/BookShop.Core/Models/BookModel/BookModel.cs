﻿using System;
using BookShop.Core.Entities;

namespace BookShop.Core.Models.BookModel
{
    public class BookModel
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public double Price { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}