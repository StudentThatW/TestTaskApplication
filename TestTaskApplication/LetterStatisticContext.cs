using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace TestTaskApplication
{
    public partial class LetterStatisticContext : DbContext
    {
        public LetterStatisticContext()
        {
        }

        public LetterStatisticContext(DbContextOptions<LetterStatisticContext> options)
            : base(options)
        {
        }

        public virtual DbSet<LetterFrequency> LetterFrequencies { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database={your_database};Username={your_login};Password={your_password}");
                
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LetterFrequency>(entity =>
            {
                entity.HasKey(e => new { e.Letter, e.MessageNumber, e.OwnerId })
                    .HasName("pk_owner_message_letter");

                entity.ToTable("LetterFrequency");

                entity.Property(e => e.Letter)
                    .HasColumnType("character varying")
                    .HasColumnName("letter");

                entity.Property(e => e.MessageNumber).HasColumnName("message_number");

                entity.Property(e => e.OwnerId).HasColumnName("owner_id");

                entity.Property(e => e.Frequency).HasColumnName("frequency");

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.LetterFrequencies)
                    .HasForeignKey(d => d.OwnerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_owner_id");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.UserId)
                    .ValueGeneratedNever()
                    .HasColumnName("user_id");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
