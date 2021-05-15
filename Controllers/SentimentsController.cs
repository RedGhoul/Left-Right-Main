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
    public class SentimentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SentimentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Sentiments
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Sentiments.Include(s => s.HeadLine);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Sentiments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sentiment = await _context.Sentiments
                .Include(s => s.HeadLine)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sentiment == null)
            {
                return NotFound();
            }

            return View(sentiment);
        }

        // GET: Sentiments/Create
        public IActionResult Create()
        {
            ViewData["HeadLineId"] = new SelectList(_context.HeadLines, "Id", "Id");
            return View();
        }

        // POST: Sentiments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,pos,compound,neu,neg,HeadLineId")] Sentiment sentiment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sentiment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["HeadLineId"] = new SelectList(_context.HeadLines, "Id", "Id", sentiment.HeadLineId);
            return View(sentiment);
        }

        // GET: Sentiments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sentiment = await _context.Sentiments.FindAsync(id);
            if (sentiment == null)
            {
                return NotFound();
            }
            ViewData["HeadLineId"] = new SelectList(_context.HeadLines, "Id", "Id", sentiment.HeadLineId);
            return View(sentiment);
        }

        // POST: Sentiments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,pos,compound,neu,neg,HeadLineId")] Sentiment sentiment)
        {
            if (id != sentiment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sentiment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SentimentExists(sentiment.Id))
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
            ViewData["HeadLineId"] = new SelectList(_context.HeadLines, "Id", "Id", sentiment.HeadLineId);
            return View(sentiment);
        }

        // GET: Sentiments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sentiment = await _context.Sentiments
                .Include(s => s.HeadLine)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sentiment == null)
            {
                return NotFound();
            }

            return View(sentiment);
        }

        // POST: Sentiments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sentiment = await _context.Sentiments.FindAsync(id);
            _context.Sentiments.Remove(sentiment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SentimentExists(int id)
        {
            return _context.Sentiments.Any(e => e.Id == id);
        }
    }
}
