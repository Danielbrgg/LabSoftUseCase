using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Clinica.Models;

public partial class DbClinicaContext : DbContext
{
    public DbClinicaContext()
    {
    }

    public DbClinicaContext(DbContextOptions<DbClinicaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Consulta> Consulta { get; set; }

    public virtual DbSet<Médico> Médicos { get; set; }

    public virtual DbSet<Paciente> Pacientes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConexaoSqlServer");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Consulta>(entity =>
        {
            entity.HasKey(e => e.Codigo);

            entity.Property(e => e.DataHora).HasColumnType("datetime");
            entity.Property(e => e.StatusConsulta)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Medico).WithMany(p => p.Consulta)
                .HasForeignKey(d => d.MedicoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Consulta_Médico");

            entity.HasOne(d => d.Paciente).WithMany(p => p.Consulta)
                .HasForeignKey(d => d.PacienteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Consulta_Paciente");
        });

        modelBuilder.Entity<Médico>(entity =>
        {
            entity.HasKey(e => e.Codigo);

            entity.ToTable("Médico");

            entity.Property(e => e.Crm)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CRM");
            entity.Property(e => e.Especialidade)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Paciente>(entity =>
        {
            entity.HasKey(e => e.Codigo);

            entity.ToTable("Paciente");

            entity.HasIndex(e => e.Codigo, "IX_Paciente");

            entity.Property(e => e.Cpf)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CPF");
            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Telefone)
                .HasMaxLength(14)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
