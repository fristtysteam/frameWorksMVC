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
    public class GamesController : Controller
    {
        private readonly MvcGameContext _context;

        public GamesController(MvcGameContext context)
        {
            _context = context;
        }

        // GET: Games
        /// <summary>
        /// Displays a list of games filtered by genre and search string.
        /// </summary>
        /// <param name="gameGenre">Genre to filter by.</param>
        /// <param name="searchString">Search string to filter games by title.</param>
        /// <returns>Returns a view displaying the filtered list of games.</returns>
        public async Task<IActionResult> Index(string gameGenre, string searchString)
        {
            if (_context.Game == null)
            {
                return Problem("Entity set 'MvcGameContext.Game'  is null.");
            }

            IQueryable<string> genreQuery = from g in _context.Game
                                            orderby g.Genre
                                            select g.Genre;
            var games = from g in _context.Game
                        select g;

            if (!string.IsNullOrEmpty(searchString))
            {
                games = games.Where(s => s.Title!.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(gameGenre))
            {
                games = games.Where(x => x.Genre == gameGenre);
            }

            var gameGenreVM = new GameGenreViewModel
            {
                Genres = new SelectList(await genreQuery.Distinct().ToListAsync()),
                Games = await games.ToListAsync()
            };

            return View(gameGenreVM);
        }

        /// <summary>
        /// Hhandles form submission for filtering games by search string.
        /// </summary>
        /// <param name="searchString">Search string entered by the user</param>
        /// <param name="notUsed">Unused parameter required for method signature</param>
        /// <returns>Returns a string indicating the applied filter.</returns>
        [HttpPost]
        public string Index(string searchString, bool notUsed)
        {
            return "From [HttpPost]Index: filter on " + searchString;
        }




        // GET: Games/Details/5
        /// <summary>
        /// Displays details of a specific game.
        /// </summary>
        /// <param name="id">ID of the game to display details for.</param>
        /// <returns>Returns a view displaying the details of the specified game.</returns>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Game
                .FirstOrDefaultAsync(m => m.Id == id);
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        
        [HttpPost]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Games/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Genre,Price,ReleaseDate,Rating")] Game game)
        {
            if (ModelState.IsValid)
            {
                _context.Add(game);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(game);
        }

        // GET: Games/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Game.FindAsync(id);
            if (game == null)
            {
                return NotFound();
            }
            return View(game);
        }

        // POST: Games/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Handles form submission for editing an existing game.
        /// </summary>
        /// <param name="id">ID of the game to edit.</param>
        /// <param name="game">Game object containing updated details.</param>
        /// <returns>Returns a view indicating success or failure of the update operation.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Genre,Price,ReleaseDate,Rating")] Game game)
        {
            if (id != game.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(game);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GameExists(game.Id))
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
            return View(game);
        }

        // GET: Games/Delete/5
        /// <summary>
        /// Displays a confirmation page for deleting a game.
        /// </summary>
        /// <param name="id">ID of the game to delete.</param>
        /// <param name="notUsed">Unused parameter required for method signature.</param>
        /// <returns>Returns a view for confirming the deletion of the specified game.</returns>
        public async Task<IActionResult> Delete(int? id, bool notUsed)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Game
                .FirstOrDefaultAsync(m => m.Id == id);
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        // POST: Games/Delete/5
        /// <summary>
        /// Handles form submission for confirming the deletion of a game.
        /// </summary>
        /// <param name="id">ID of the game to delete</param>
        /// <returns>Returns a view indicating success or failure of the deletion operation.</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var game = await _context.Game.FindAsync(id);
            if (game != null)
            {
                _context.Game.Remove(game);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        /// <summary>
        /// Checks if a game with the specified ID exists in the database.
        /// </summary>
        /// <param name="id">ID of the game to check</param>
        /// <returns>Returns true if a game with the specified ID exists; otherwise, false.</returns>
        private bool GameExists(int id)
        {
            return _context.Game.Any(e => e.Id == id);
        }

        // POST: Movies/Delete/6



    }
}
