using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;

namespace Konsthuset.Models
{
    public class Artwork
    {
        //vilka properties o data som ska lagras
        public int Id { get; set; }
        [Required(ErrorMessage = "Fyll i namn på konstverket")]
        [Display(Name = "Namn på konstverket:")]
        public string? ArtName { get; set; }
        [Required(ErrorMessage = "Fyll i årtal")]
        [Display(Name = "Skapandeår:")]
        public int ArtYear { get; set; }
        [Required(ErrorMessage = "Fyll i konstnär")]
        [Display(Name = "Konstnär:")]
        public string? ArtistName { get; set; }
        [Required(ErrorMessage = "Fyll i teknik")]
        [Display(Name = "Teknik:")]
        public string? ArtTechnique { get; set; }
        [Required(ErrorMessage = "Fyll i pris")]
        [Display(Name = "Pris i kronor:")]
        public int ArtPrice { get; set; }
        [Required(ErrorMessage = "Fyll i breddmått")]
        [Display(Name = "Bredd cm:")]
        public int ArtWidth { get; set; }
        [Required(ErrorMessage = "Fyll i höjdmått")]
        [Display(Name = "Höjd cm:")]
        public int ArtHeight { get; set; }

        //lagras i db.
        [Display(Name = "Filnamn bild:")]
        public string? ImageName { get; set; }
        [Display(Name = "Alttext för bild")]
        public string? AltText { get; set; }

        //lagras i gränssnitt men ej i db
        [NotMapped]
        [Display(Name = "Bild:")]
        public IFormFile? ImageFile { get; set; }

    }
}