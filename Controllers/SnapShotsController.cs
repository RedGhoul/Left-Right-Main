using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LeftRightNet.Data;
using LeftRightNet.Models;
using Microsoft.AspNetCore.Authorization;

namespace LeftRightNet.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SnapShotsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SnapShotsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SnapShots
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.SnapShots.Include(s => s.NewsSite);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: SnapShots/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var snapShot = await _context.SnapShots
                .Include(s => s.NewsSite)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (snapShot == null)
            {
                return NotFound();
            }

            return View(snapShot);
        }

        // GET: SnapShots/Create
        public IActionResult Create()
        {
            ViewData["NewsSiteId"] = new SelectList(_context.NewsSites, "Id", "Id");
            return View();
        }

        // POST: SnapShots/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ImageUrl,CreatedAt,NewsSiteId")] SnapShot snapShot)
        {
            if (ModelState.IsValid)
            {
                _context.Add(snapShot);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["NewsSiteId"] = new SelectList(_context.NewsSites, "Id", "Id", snapShot.NewsSiteId);
            return View(snapShot);
        }

        // GET: SnapShots/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var snapShot = await _context.SnapShots.FindAsync(id);
            if (snapShot == null)
            {
                return NotFound();
            }
            ViewData["NewsSiteId"] = new SelectList(_context.NewsSites, "Id", "Id", snapShot.NewsSiteId);
            return View(snapShot);
        }

        // POST: SnapShots/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ImageUrl,CreatedAt,NewsSiteId")] SnapShot snapShot)
        {
            if (id != snapShot.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(snapShot);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SnapShotExists(snapShot.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["NewsSiteId"] = new SelectList(_context.NewsSites, "Id", "Id", snapShot.NewsSiteId);
            return View(snapShot);
        }

        // GET: SnapShots/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var snapShot = await _context.SnapShots
                .Include(s => s.NewsSite)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (snapShot == null)
            {
                return NotFound();
            }

            return View(snapShot);
        }

        // POST: SnapShots/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var snapShot = await _context.SnapShots.FindAsync(id);
            _context.SnapShots.Remove(snapShot);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SnapShotExists(int id)
        {
            return _context.SnapShots.Any(e => e.Id == id);
        }
    }
}
