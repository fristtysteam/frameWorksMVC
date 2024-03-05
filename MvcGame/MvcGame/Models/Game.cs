using System.ComponentModel.DataAnnotations;

namespace MvcGame.Models
{
    public class Game
    {
        /// <summary>
        /// Game Models Class
        /// </summary>
        public int Id { get; set; }
        public string Title { get; set; }
        /// <summary>
        /// ? annotation means the property is nullable
        /// </summary>
        public string? Description { get; set; }
        public string? Genre { get; set; }
        public decimal Price {  get; set; }
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }
        public Game() { }
    }
}
