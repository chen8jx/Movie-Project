using System;
using System.Collections.Generic;
using System.Text;

namespace MovieShop.Core.Models.Response
{
    public class PurchaseResponseModel
    {
        public int UserId { get; set; }
        public int MovieId { get; set; }
        public DateTime PurchaseDateTime { get; set; }
    }
}
