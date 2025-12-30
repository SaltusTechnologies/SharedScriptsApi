using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Linq.Expressions;

namespace SharedScriptsApi.DataModels.ModelConfiguration
{
    public class ScriptConfiguration : IEntityTypeConfiguration<Script>
    {
        public void Configure(EntityTypeBuilder<Script> entity)
        {
            Expression<Func<DateTime, DateTime>> normalizeDate = d => DateTime.SpecifyKind(d, DateTimeKind.Local);
            var dateTimeNormalizer = new ValueConverter<DateTime, DateTime>(normalizeDate, normalizeDate);

            entity.HasKey(["Name", "Version"])
                .HasName("PK_Script");

            entity.Property(e => e.ScriptId)
                .UseIdentityColumn(1, 1);

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.Version)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            // [WebTeam] not sure this is implemented the best way
            // but it works for now
            // https://learn.microsoft.com/en-us/ef/core/miscellaneous/collations-and-case-sensitivity
            // https://learn.microsoft.com/en-us/sql/relational-databases/collations/collation-and-unicode-support?view=sql-server-ver16#Column-level-collations
            entity.Property(e => e.Value)
            .HasColumnType("nvarchar(max)")
            .UseCollation("Latin1_General_100_CI_AI_SC_UTF8")
            .IsUnicode(true);

            entity.Property(e => e.ModifiedDate)
                .HasColumnType("datetime2")
                .HasConversion(dateTimeNormalizer);

            entity.Property(e => e.Value)
                .IsRequired()
                .IsUnicode(false);

            entity.HasMany(s => s.ScriptConstraints);

            entity.ToTable("Script", tb => tb.HasTrigger("LogScriptChanges"));
        }
    }
}
