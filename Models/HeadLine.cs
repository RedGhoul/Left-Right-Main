using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeftRightNet.Models
{
    public class HeadLine
    {
        public int Id { get; set; }
        public string ValueText { get; set; }
        public DateTime CreatedAt { get; set; }
        public SnapShot SnapShot { get; set; }
        public int SnapShotId { get; set; }
        public Sentiment Sentiment { get; set; }
        public NpgsqlTsVector SearchVector { get; set; }
    }
}
