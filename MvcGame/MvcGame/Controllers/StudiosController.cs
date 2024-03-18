using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcGame.Data;
using MvcGame.Models;

namespace MvcGame.Controllers
{
    public class StudiosController : Controller
    {
        private readonly MvcGameContext _context;

        public StudiosController(MvcGameContext context)
        {
            _context = context;
        }

        // GET: Studios

        /// <summary>
        /// Displays a list of studios.
        /// </summary>
        /// <returns>Returns a view displaying the filtered list of Studios.</returns>
        public async Task<IActionResult> Index()
        {
            return View(await _context.Studio.ToListAsync());
        }

        // GET: Studios/Details/5
        /// <summary>
        /// Displays details of Studios by the id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A view displaying the details of the Studio by getting its id.</returns>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studio = await _context.Studio
                .FirstOrDefaultAsync(m => m.Id == id);
            if (studio == null)
            {
                return NotFound();
            }

            return View(studio);
        }

        // GET: Studios/Create
        /// <summary>
        /// Create a Studio.
        /// </summary>
        /// <returns>A view displaying info to create a Studio</returns>
        public IActionResult Create()
        {
            return View();
        }

        // POST: Studios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Binds the details to Create a studio
        /// </summary>
        /// <param name="studio"></param>
        /// <returns>A view Displaying the studio</returns>        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,ReleaseDate")] Studio studio)
        {
            if (ModelState.IsValid)
            {
                _context.Add(studio);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(studio);
        }

        // GET: Studios/Edit/5
        /// <summary>
        /// A view displaying a form to edit studio by its id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A view that has a form to edit a studio.</returns>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studio = await _context.Studio.FindAsync(id);
            if (studio == null)
            {
                return NotFound();
            }
            return View(studio);
        }

        // POST: Studios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Binds the edited Studio
        /// </summary>
        /// <param name="id"></param>
        /// <param name="studio"></param>
        /// <returns>Binds the new information of the edited studio</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,ReleaseDate")] Studio studio)
        {
            if (id != studio.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(studio);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudioExists(studio.Id))
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
            return View(studio);
        }

        // GET: Studios/Delete/5
        /// <summary>
        /// Deletes a Studio by its ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Displays a view that lets you confirm wheter or not to delete that studio.</returns>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studio = await _context.Studio
                .FirstOrDefaultAsync(m => m.Id == id);
            if (studio == null)
            {
                return NotFound();
            }

            return View(studio);
        }

        // POST: Studios/Delete/5
        /// <summary>
        /// Deletes Studio and redirects to Studio View / index.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A view of index.</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var studio = await _context.Studio.FindAsync(id);
            if (studio != null)
            {
                _context.Studio.Remove(studio);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudioExists(int id)
        {
            return _context.Studio.Any(e => e.Id == id);
        }
    }
}
