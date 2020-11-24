using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MovieShop.Core.Entities
{
    [Table("Role")]
    public class Role
    {
        public int Id { get; set; }
        [MaxLength(20)]
        public string Name { get; set; }
        public ICollection<User> User { get; set; }
    }
}
