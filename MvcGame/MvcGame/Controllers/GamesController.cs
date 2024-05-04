using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcGame.Data;
using MvcGame.Models;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MvcGame.Controllers
{
    public class GamesController : Controller
    {
        private readonly MvcGameContext _context;
        private readonly string _rawgApiKey = "0bf1db4562bb4b6bb1452b58c12fb7f9";
        private readonly HttpClient _httpClient;

        public GamesController(MvcGameContext context)
        {
            _context = context;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://api.rawg.io/api/");
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "MvcGame");
        }

        private async Task<List<Game>> GetGamesFromOpenCriticApi()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://opencritic-api.p.rapidapi.com/game/popular"),
                Headers =
        {
            { "X-RapidAPI-Key", "38202c4960msh7e78e26b55966dap13853bjsnf7e4e0b38f52" },
            { "X-RapidAPI-Host", "opencritic-api.p.rapidapi.com" },
        },
            };

            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();

                // Deserialize JSON response into a list of Game objects
                var games = JsonConvert.DeserializeObject<List<Game>>(body);

                return games;
            }
        }
        public async Task<IActionResult> OpenCriticGames()
        {
            var openCriticGames = await GetGamesFromOpenCriticApi();

            if (openCriticGames != null && openCriticGames.Any())
            {
                var openCriticViewModel = new GameGenreViewModel
                {
                    OpenCriticGames = openCriticGames
                };

                return View(openCriticViewModel);
            }
            else
            {
                return View(new GameGenreViewModel());
            }
        }




        // GET: Games
        public async Task<IActionResult> Index(string gameGenre, string searchString)
        {
            if (_context.Game == null)
            {
                return Problem("Entity set 'MvcGameContext.Game' is null.");
            }

            IQueryable<string> genreQuery = from g in _context.Game
                                            orderby g.Genre
                                            select g.Genre;
            var games = _context.Game.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                games = games.Where(s => s.Title.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(gameGenre))
            {
                games = games.Where(x => x.Genre == gameGenre);
            }

            // Fetch API games
            List<Game> apiGames = new List<Game>();
            if (!string.IsNullOrEmpty(searchString))
            {
                apiGames = await GetGamesFromRawgApi(searchString);

                if (apiGames != null && apiGames.Any())
                {
                    var allGames = games.ToList();
                    allGames.AddRange(apiGames);

                    var gameGenreVM = new GameGenreViewModel
                    {
                        Genres = new SelectList(await genreQuery.Distinct().ToListAsync()),
                        Games = allGames
                    };

                    return View(gameGenreVM);
                }
            }

            // If API games are null or empty, return only the local games
            var localGameGenreVM = new GameGenreViewModel
            {
                Genres = new SelectList(await genreQuery.Distinct().ToListAsync()),
                Games = await games.ToListAsync()
            };

            return View(localGameGenreVM);
        }

        [HttpPost]
        public string Index(string searchString, bool notUsed)
        {
            return "From [HttpPost]Index: filter on " + searchString;
        }

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

        public IActionResult Create()
        {
            return View();
        }

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

        private bool GameExists(int id)
        {
            return _context.Game.Any(e => e.Id == id);
        }

        private async Task<List<Game>> GetGamesFromRawgApi(string searchString)
        {
            var response = await _httpClient.GetAsync($"games?search={searchString}&key={_rawgApiKey}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var jsonObject = JsonConvert.DeserializeObject<JObject>(content);
                var gamesArray = jsonObject["results"].ToObject<JArray>();

                var games = new List<Game>();
                foreach (var gameToken in gamesArray)
                {
                    var gameObject = gameToken.ToObject<JObject>();
                    var title = (string)gameObject["name"];
                    var description = "";
                    var genre = "";
                    var priceStr = (string)gameObject["metacritic"];
                    var releaseDateStr = (string)gameObject["released"];
                    var rating = (string)gameObject["rating"];

                    var genresArray = gameObject["genres"].ToObject<JArray>();
                    if (genresArray.Any())
                    {
                        genre = (string)genresArray[0]["name"];
                    }

                    DateTime releaseDate;
                    if (!DateTime.TryParse(releaseDateStr, out releaseDate))
                    {
                        releaseDate = DateTime.MinValue;
                    }

                    decimal price;
                    if (!decimal.TryParse(priceStr, out price))
                    {
                        price = 0m;
                    }

                    if (string.IsNullOrEmpty(description))
                    {
                        description = "Description not available";
                    }

                    var game = new Game
                    {
                        Title = title,
                        Description = description,
                        Genre = genre,
                        Price = price,
                        ReleaseDate = releaseDate,
                        Rating = rating
                    };

                    games.Add(game);
                }

                return games;
            }
            else
            {
                return null;
            }
        }
    }
}
