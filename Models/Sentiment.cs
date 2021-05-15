using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeftRightNet.Models
{
    public class Sentiment
    {
        public int Id { get; set; }
        public float pos { get; set; }
        public float compound { get; set; }
        public float neu { get; set; }
        public float neg { get; set; }
        public HeadLine HeadLine { get; set; }
        public int HeadLineId { get; set; }
    }
}
