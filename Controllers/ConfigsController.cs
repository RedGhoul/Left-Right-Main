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
    [Authorize(Roles ="Admin")]
    public class ConfigsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ConfigsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Configs
        public async Task<IActionResult> Index()
        {
            return View(await _context.Configs.ToListAsync());
        }

        // GET: Configs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var config = await _context.Configs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (config == null)
            {
                return NotFound();
            }

            return View(config);
        }

        // GET: Configs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Configs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,URLForScrapping,URLForSentiment")] Config config)
        {
            if (ModelState.IsValid)
            {
                _context.Add(config);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(config);
        }

        // GET: Configs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var config = await _context.Configs.FindAsync(id);
            if (config == null)
            {
                return NotFound();
            }
            return View(config);
        }

        // POST: Configs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,URLForScrapping,URLForSentiment")] Config config)
        {
            if (id != config.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(config);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConfigExists(config.Id))
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
            return View(config);
        }

        // GET: Configs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var config = await _context.Configs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (config == null)
            {
                return NotFound();
            }

            return View(config);
        }

        // POST: Configs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var config = await _context.Configs.FindAsync(id);
            _context.Configs.Remove(config);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ConfigExists(int id)
        {
            return _context.Configs.Any(e => e.Id == id);
        }
    }
}
