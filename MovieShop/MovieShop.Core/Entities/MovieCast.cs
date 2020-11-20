using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieShop.Core.Entities
{
    public class MovieCast
    {
        public int CastId { get; set; }
        public int MovieId { get; set; }
        [Column("Character")]
        public string Charater { get; set; }
        public Movie Movie { get; set; }
        public Cast Cast { get; set; }
    }
}