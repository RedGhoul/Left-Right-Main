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
    public class HeadLinesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HeadLinesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: headLines
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.HeadLines.Include(h => h.SnapShot);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: headLines/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var headLine = await _context.HeadLines
                .Include(h => h.SnapShot)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (headLine == null)
            {
                return NotFound();
            }

            return View(headLine);
        }

        // GET: headLines/Create
        public IActionResult Create()
        {
            ViewData["SnapShotId"] = new SelectList(_context.SnapShots, "Id", "Id");
            return View();
        }

        // POST: headLines/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ValueText,CreatedAt,ValueTextSentiment,SnapShotId")] HeadLine headLine)
        {
            if (ModelState.IsValid)
            {
                _context.Add(headLine);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SnapShotId"] = new SelectList(_context.SnapShots, "Id", "Id", headLine.SnapShotId);
            return View(headLine);
        }

        // GET: headLines/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var headLine = await _context.HeadLines.FindAsync(id);
            if (headLine == null)
            {
                return NotFound();
            }
            ViewData["SnapShotId"] = new SelectList(_context.SnapShots, "Id", "Id", headLine.SnapShotId);
            return View(headLine);
        }

        // POST: headLines/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ValueText,CreatedAt,ValueTextSentiment,SnapShotId")] HeadLine headLine)
        {
            if (id != headLine.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(headLine);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HeadLineExists(headLine.Id))
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
            ViewData["SnapShotId"] = new SelectList(_context.SnapShots, "Id", "Id", headLine.SnapShotId);
            return View(headLine);
        }

        // GET: headLines/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var headLine = await _context.HeadLines
                .Include(h => h.SnapShot)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (headLine == null)
            {
                return NotFound();
            }

            return View(headLine);
        }

        // POST: headLines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var headLine = await _context.HeadLines.FindAsync(id);
            _context.HeadLines.Remove(headLine);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HeadLineExists(int id)
        {
            return _context.HeadLines.Any(e => e.Id == id);
        }
    }
}
