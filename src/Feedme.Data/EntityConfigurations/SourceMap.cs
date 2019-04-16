using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Feedme.Domain.Models;

namespace Feedme.Data.EntityConfigurations
{
    public class SourceMap : IEntityTypeConfiguration<Source>
    {
        public void Configure(EntityTypeBuilder<Source> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Url).IsRequired();
            builder.Property(x => x.Type)
                .HasConversion(new EnumToStringConverter<SourceType>())
                .HasColumnType("varchar(20)")
                .IsRequired();
        }
    }
}