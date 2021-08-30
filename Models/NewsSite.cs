using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LeftRightNet.Models
{
    public class NewsSite
    {
        public int Id { get; set; }
        [Required]
        [Column(TypeName = "varchar(700)")]
        public string Name { get; set; }
        [Required]
        [Column(TypeName = "varchar(700)")]
        public string Url { get; set; }
        public List<SnapShot> SnapShots { get; set; }
    }
}
