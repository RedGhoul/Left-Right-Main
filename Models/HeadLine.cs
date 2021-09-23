using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LeftRightNet.Models
{
    public class HeadLine
    {
        public int Id { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(1000)")]
        public string ValueText { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public SnapShot SnapShot { get; set; }
        public int SnapShotId { get; set; }
        public Sentiment Sentiment { get; set; }
    }
}
