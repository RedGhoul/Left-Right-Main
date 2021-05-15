using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeftRightNet.Models
{
    public class NewsSite
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public List<SnapShot> SnapShots { get; set; }
    }
}
