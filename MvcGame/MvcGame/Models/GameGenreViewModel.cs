using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MvcGame.Models
{
    public class GameGenreViewModel
    {
        public List<Game>? Games { get; set; }
        public SelectList? Genres { get; set; }
        public string? GameGenre { get; set; }
        public string? SearchString { get; set; }
        public List<Game>? ApiGames { get; set; }
        public List<Game>? OpenCriticGames { get; set; }

        public string? Name { get; set; }
        public int TopCriticScore { get; set; }
        public string? Tier { get; set; }
    }
}
