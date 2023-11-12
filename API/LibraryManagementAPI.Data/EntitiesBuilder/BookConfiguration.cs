using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using LibraryManagementAPI.Data.Entities;

namespace LibraryManagementAPI.Data.EntitiesBuilder
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.HasKey(b => b.Id);
            builder.Property(b => b.ISBN).IsRequired().HasMaxLength(17);
            builder.HasIndex(b => b.ISBN).IsUnique();
            builder.Property(b => b.Title).IsRequired();
            builder.Property(b => b.Genre).IsRequired();
            builder.Property(b => b.Author).IsRequired();
            builder.Property(b => b.Description);
            builder.Property(b => b.CheckedOutDate);
            builder.Property(b => b.DueDate);
        }
    }
}