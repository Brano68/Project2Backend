using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace UserPersistance.Models
{
    class User
    {
        [Key]
        public int Id { get; set; }
    }
}
