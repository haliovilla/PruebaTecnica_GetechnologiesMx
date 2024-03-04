using EDM.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAC.EntityMaps
{
    public class PersonMap : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.PaternalLastname)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.MaternalLastname)
                .IsRequired(false)
                .HasMaxLength(50);

            builder.HasMany(p=>p.Invoices)
                .WithOne(i=>i.Person)
                .HasForeignKey(fk=>fk.PersonId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}