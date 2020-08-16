using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Escola.Models
{
    public partial class EscolaContext : DbContext
    {
        public EscolaContext()
        {
        }

        public EscolaContext(DbContextOptions<EscolaContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Alunos> Alunos { get; set; }
        public virtual DbSet<Professores> Professores { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

                optionsBuilder.UseSqlServer("Server=.\\SQLExpress;Database=Escola;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Alunos>(entity =>
            {
                entity.Property(e => e.DataVencimento).HasColumnType("date");

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsFixedLength();

                entity.Property(e => e.ValorMensalidade).HasColumnType("money");
            });

            modelBuilder.Entity<Professores>(entity =>
            {
                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsFixedLength();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
