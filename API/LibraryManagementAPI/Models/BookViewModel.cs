using System.ComponentModel;

namespace LibraryManagementAPI.Models
{
    public class BookViewModel
    {        
        [DefaultValue("978-1-45678-123-4")]
        public string? ISBN { get; set; }
        public string? Title { get; set; }
        public string? Genre { get; set; }
        public string? Description { get; set; }
        public string? Author { get; set; }
        public DateTime? CheckedOutDate { get; set; }
        public DateTime? DueDate { get; set; }
    }
}