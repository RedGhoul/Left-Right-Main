using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LeftRightNet.Models
{
    public class SnapShot
    {
        public int Id { get; set; }
        
        [Required]
        [Column(TypeName = "varchar(255)")]
        public string ImageHashId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public List<HeadLine> HeadLines { get; set; }
        public int NewsSiteId { get; set; }
        public NewsSite NewsSite { get; set; }
    }
}
