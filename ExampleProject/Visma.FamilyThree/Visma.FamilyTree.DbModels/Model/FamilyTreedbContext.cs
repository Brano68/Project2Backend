using Microsoft.EntityFrameworkCore;

namespace Visma.FamilyTree.DbModels.Model
{
    public partial class FamilyTreedbContext : DbContext
    {
        public FamilyTreedbContext()
        {
        }

        public FamilyTreedbContext(DbContextOptions<FamilyTreedbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Child> Child { get; set; }
        public virtual DbSet<Person> Person { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.0-rtm-35687");

            modelBuilder.Entity<Child>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Birthday)/*HasColumnType("datetime")*/;

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.Surname)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.Child)
                    .HasForeignKey(d => d.PersonId)
                    /*.HasConstraintName("FK_Child_Person")*/;
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Birthday)/*.HasColumnType("datetime")*/;

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.Surname)
                    .IsRequired()
                    .HasMaxLength(250);
            });
        }
    }
}
