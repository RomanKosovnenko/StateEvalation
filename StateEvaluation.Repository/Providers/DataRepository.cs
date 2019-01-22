namespace StateEvaluation.Repository.Providers
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using StateEvaluation.Repository.Models;

    public partial class DataRepository : DbContext
    {
        public DataRepository()
            : base("name=DataRepository")
        {
        }

        public virtual DbSet<Color> Colors { get; set; }
        public virtual DbSet<People> People { get; set; }
        public virtual DbSet<Preference> Preference { get; set; }
        public virtual DbSet<RelaxTable1> RelaxTable1 { get; set; }
        public virtual DbSet<RelaxTable2> RelaxTable2 { get; set; }
        public virtual DbSet<SubjectiveFeeling> SubjectiveFeelings { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Color>()
                .Property(e => e.Colors)
                .IsFixedLength();

            modelBuilder.Entity<People>()
                .Property(e => e.UserId)
                .IsFixedLength();

            modelBuilder.Entity<People>()
                .Property(e => e.Firstname)
                .IsFixedLength();

            modelBuilder.Entity<People>()
                .Property(e => e.Lastname)
                .IsFixedLength();

            modelBuilder.Entity<People>()
                .Property(e => e.Birthday)
                .IsFixedLength();

            modelBuilder.Entity<People>()
                .Property(e => e.Workposition)
                .IsFixedLength();

            modelBuilder.Entity<People>()
                .Property(e => e.Middlename)
                .IsFixedLength();

            modelBuilder.Entity<People>()
                .HasMany(e => e.Preference)
                .WithRequired(e => e.People)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<People>()
                .HasMany(e => e.SubjectiveFeelings)
                .WithRequired(e => e.People)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Preference>()
                .Property(e => e.UserId)
                .IsFixedLength();

            modelBuilder.Entity<Preference>()
                .Property(e => e.ShortOder1)
                .IsFixedLength();

            modelBuilder.Entity<Preference>()
                .Property(e => e.Oder1)
                .IsFixedLength();

            modelBuilder.Entity<Preference>()
                .Property(e => e.Preference1)
                .IsFixedLength();

            modelBuilder.Entity<Preference>()
                .Property(e => e.ShortOder2)
                .IsFixedLength();

            modelBuilder.Entity<Preference>()
                .Property(e => e.Oder2)
                .IsFixedLength();

            modelBuilder.Entity<Preference>()
                .Property(e => e.Preference2)
                .IsFixedLength();

            modelBuilder.Entity<Preference>()
                .Property(e => e.Compare)
                .IsFixedLength();

            modelBuilder.Entity<RelaxTable1>()
                .HasMany(e => e.Preference)
                .WithOptional(e => e.RelaxTable11)
                .HasForeignKey(e => e.RelaxTable1);

            modelBuilder.Entity<RelaxTable2>()
                .HasMany(e => e.Preference)
                .WithOptional(e => e.RelaxTable21)
                .HasForeignKey(e => e.RelaxTable2);

            modelBuilder.Entity<SubjectiveFeeling>()
                .Property(e => e.UserId)
                .IsFixedLength();
        }
    }
}
