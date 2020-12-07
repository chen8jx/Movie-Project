using System;
using System.Collections.Generic;
using System.Text;

namespace MovieShop.Core.Models.Response
{
    public class FavoriteResponseModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int MovieId { get; set; }
    }
}
