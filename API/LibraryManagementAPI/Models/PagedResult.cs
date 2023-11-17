﻿namespace LibraryManagementAPI.Models
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; }
        public int TotalItems { get; set; }
        public int Page { get; set; }
        public int TotalPages { get; set; }
    }
}