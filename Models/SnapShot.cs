using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeftRightNet.Models
{
    public class SnapShot
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<HeadLine> HeadLines { get; set; }
        public int NewsSiteId { get; set; }
        public NewsSite NewsSite { get; set; }
    }
}
