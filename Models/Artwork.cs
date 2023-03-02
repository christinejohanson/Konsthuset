using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;

namespace Konsthuset.Models
{
    public class Artwork
    {
        //vilka properties o data som ska lagras
        public int Id { get; set; }
        [Display(Name = "Namn på konstverket:")]
        public string? ArtName { get; set; }
        [Display(Name = "Skapandeår:")]
        public int ArtYear { get; set; }
        [Display(Name = "Namn på konstnären:")]
        public string? ArtistName { get; set; }
        [Display(Name = "Teknik:")]
        public string? ArtTechnique { get; set; }
        [Display(Name = "Pris i kronor:")]
        public int ArtPrice { get; set; }
        [Display(Name = "Bredd i centimeter:")]
        public int ArtWidth { get; set; }
        [Display(Name = "Höjd i centimeter:")]
        public int ArtHeight { get; set; }

        //lagras i db.
        [Display(Name = "Filnamn bild:")]
        public string? ImageName { get; set; }

        //lagras i gränssnitt men ej i db
        [NotMapped]
        [Display(Name = "Bild från Modeln")]
        public IFormFile? ImageFile { get; set; }

    }
}