using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeftRightNet.Models.ViewModels
{
    public class HomeViewModel
    {
        public Dictionary<string, List<HeadLine>> SiteHeadLines { get; set; } = new Dictionary<string, List<HeadLine>>();
      
    }
}
