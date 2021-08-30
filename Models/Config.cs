using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LeftRightNet.Models
{
    public class Config
    {
        public int Id { get; set; }
        [Required]
        [Column(TypeName = "varchar(800)")]
        public string URLForScrapping { get; set; }
        [Required]
        [Column(TypeName = "varchar(800)")]
        public string URLForSentiment { get; set; }
    }
}
