using System;
using System.Collections.Generic;
using System.Text;

namespace MovieShop.Core.Entities
{
    public class Trailer
    {
        public int Id { get; set; }
        //foreign key from movie which is Id as PK
        //first check if the id exists in Movie, if yes, execute
        //this property => create foreign key
        public int MovieId { get; set; }
        public string TrailerUrl { get; set; }
        public string Name { get; set; }
        //Navigation property, used to navigate to related entities
        //trailerId 24 => get me Movie Tile and Movie Overview
        public Movie Movie { get; set; }
    }
}
