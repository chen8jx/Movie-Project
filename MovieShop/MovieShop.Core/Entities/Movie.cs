﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MovieShop.Core.Entities
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Overview { get; set; }
        public string Tagline { get; set; }
        public decimal? Budget { get; set; }
        public decimal? Revenue { get; set; }
        public string ImdbUrl { get; set; }
        public string TmdbUrl { get; set; }
        public string PosterUrl { get; set; }
        public string BackdropUrl { get; set; }
        public string OriginalLanguage { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public int? RunTime { get; set; }
        public decimal? Price { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string CreatedBy { get; set; }
        public ICollection<Trailer> Trailer { get; set; }
        //public ICollection<Genre> Genre { get; set; }
        public ICollection<MovieCast> MovieCast { get; set; }
        public ICollection<MovieCrew> MovieCrew { get; set; }
        public ICollection<Review> Review { get; set; }
        public ICollection<Purchase> Purchase { get; set; }
        public ICollection<Favorite> Favorite { get; set; }
        public ICollection<MovieGenre> MovieGenre { get; set; }
    }
}
