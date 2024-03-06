using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcGame.Models
{
    public class Game
    {
        /// <summary>
        /// Game Models Class
        /// </summary>
        public int Id { get; set; }


        [StringLength(60, MinimumLength = 2)]
        [Required]
        public string Title { get; set; }



        /// <summary>
        /// ? annotation means the property is nullable
        /// </summary>
        /// 
        public string? Description { get; set; }


        [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$")]
        [Required]
        [StringLength(30)]
        public string? Genre { get; set; }



        [Range(1, 100)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price {  get; set; }



        [Display(Name = "Release Date")]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }



        [StringLength(5)]
        [Required]
        public string? Rating { get; set; }



    }
}
