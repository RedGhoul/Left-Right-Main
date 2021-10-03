using LeftRightNet.Data;
using LeftRightNet.Models;
using LeftRightNet.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace LeftRightNet.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _ctx;
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext ctx)
        {
            _logger = logger;
            _ctx = ctx;
        }

        public IActionResult Index()
        {
            var ss = new HomeViewModel();
            var newSites = _ctx.NewsSites.ToList();
            foreach (var item in newSites)
            {
                var currentTime = DateTime.UtcNow;
                var pastTime = DateTime.UtcNow.AddDays(-2);
                ss.SiteHeadLines.Add(
                    item.Name.ToUpper(), 
                    _ctx.HeadLines.
                        Include(x => x.Sentiment).
                        Include(x => x.SnapShot).
                        ThenInclude(x => x.NewsSite)
                    .Where(x => x.SnapShot.NewsSite.Id == item.Id
                     && x.CreatedAt < currentTime && x.CreatedAt > pastTime &&
                     x.Sentiment != null &&
                     (x.Sentiment.compound != 0 || x.Sentiment.neg != 0))
                    .OrderByDescending(x => x.CreatedAt).Take(10).ToList());
            }

            return View(ss);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
