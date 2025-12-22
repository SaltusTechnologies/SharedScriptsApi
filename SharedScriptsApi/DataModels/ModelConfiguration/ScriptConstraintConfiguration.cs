using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Linq.Expressions;

namespace SharedScriptsApi.DataModels.ModelConfiguration
{
    public class ScriptConstraintConfiguration : IEntityTypeConfiguration<ScriptConstraint>
    {
        public void Configure(EntityTypeBuilder<ScriptConstraint> entity)
        {
            Expression<Func<DateTime, DateTime>> normalizeDate = d => DateTime.SpecifyKind(d, DateTimeKind.Local);
            var dateTimeNormalizer = new ValueConverter<DateTime, DateTime>(normalizeDate, normalizeDate);

            entity.Property(e => e.ScriptConstraintId)
                    .UseIdentityColumn(1, 1);

            entity.HasOne(d => d.Script)
               .WithMany(p => p.ScriptConstraints)
               .HasForeignKey(["Name", "Version"])
               .OnDelete(DeleteBehavior.ClientSetNull)
               .HasConstraintName("FK_Script_Constraint");

            entity.Property(e => e.ModifiedDate)
                .HasColumnType("datetime2")
                .HasConversion(dateTimeNormalizer);

            entity.Property(e => e.Constraint)
                .IsRequired()
                .IsUnicode(false);
            entity.ToTable(tb => tb.HasTrigger("LogScriptConstraintChanges"));
            entity.ToTable(tb => tb.HasTrigger("UpdateScriptModifiedDate"));
        }
    }
}
