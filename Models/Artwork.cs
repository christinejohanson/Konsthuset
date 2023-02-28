namespace Konsthuset.Models
{
    public class Artwork
    {
        //vilka properties o data som ska lagras
        public int Id { get; set; }
        public string? ArtName { get; set; }
        public int ArtYear { get; set; }
        public string? ArtistName { get; set; }
        public string? ArtTechnique { get; set; }
        public int ArtPrice { get; set; }
        public int ArtWidth { get; set; }
        public int ArtHeight { get; set; }
    }
}