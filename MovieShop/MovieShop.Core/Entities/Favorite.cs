using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MovieShop.Core.Entities
{
    [Table("Favorite")]
    public class Favorite
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
