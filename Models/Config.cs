using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeftRightNet.Models
{
    public class Config
    {
        public int Id { get; set; }
        public string URLForScrapping { get; set; }
        public string URLForSentiment { get; set; }
    }
}
