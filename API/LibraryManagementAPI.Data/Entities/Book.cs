namespace LibraryManagementAPI.Data.Entities
{
    public class Book : BaseEntity
    {
        public string? ISBN { get; set; }
        public string? Title { get; set; }
        public string? Genre { get; set; }
        public string? Description { get; set; }
        public string? Author { get; set; }
        public DateTime? CheckedOutDate { get; set; }
        public DateTime? DueDate { get; set; }
    }
}