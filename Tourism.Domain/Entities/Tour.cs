namespace Tourism.Domain.Entities
{
    public class Tour
    {
        public int TourID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
    }
}