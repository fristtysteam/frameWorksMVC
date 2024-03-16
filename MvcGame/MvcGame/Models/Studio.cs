using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MvcGame.Models
{
    public class Studio
    {
        /// <summary>
        /// Studio Models Class
        /// </summary>
        public int Id { get; set; }


        [StringLength(60, MinimumLength = 2)]
        [Required]
        public string Name { get; set; }



        /// <summary>
        /// ? annotation means the property is nullable
        /// </summary>
        /// 
        public string? Description { get; set; }



        [Display(Name = "Founded in:")]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }



    }
}
