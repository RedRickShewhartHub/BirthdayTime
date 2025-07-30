using BirthdayTime.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BirthdayTime.Data.Configurations;

public class BirthdayEntryConfiguration : IEntityTypeConfiguration<BirthdayEntry>
{
    public void Configure(EntityTypeBuilder<BirthdayEntry> builder)
    {
        builder.ToTable("Birthdays");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.FullName)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(b => b.DateOfBirth)
               .IsRequired()
               .HasColumnType("timestamp without time zone");

        builder.Property(b => b.Email)
               .HasMaxLength(200);

        builder.Property(b => b.Photo)
               .HasColumnType("bytea")
               .IsRequired(false);
    }
}
