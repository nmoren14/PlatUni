using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PlatUni.Models;

namespace PlatUni;

public partial class PlatUniContext : DbContext
{
    public PlatUniContext()
    {
    }

    public PlatUniContext(DbContextOptions<DbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Enrollment> Enrollments { get; set; }

    public virtual DbSet<Professor> Professors { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=PlatUni;Integrated Security=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configuración de la entidad Course
        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.CourseId).HasName("PK_Courses");
            entity.Property(e => e.CourseId).ValueGeneratedNever();
            entity.Property(e => e.CourseName).HasMaxLength(100);
        });

        // Configuración de la entidad Professor
        modelBuilder.Entity<Professor>(entity =>
        {
            entity.HasKey(e => e.ProfessorId).HasName("PK__Professo__9003594969E46C20");

            entity.Property(e => e.ProfessorId).ValueGeneratedNever();
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);

            entity.HasOne(d => d.FisrtClassNavigation).WithMany(p => p.ProfessorFisrtClassNavigations)
                .HasForeignKey(d => d.FisrtClass)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Professor__Fisrt__6E01572D");

            entity.HasOne(d => d.SecondClassNavigation).WithMany(p => p.ProfessorSecondClassNavigations)
                .HasForeignKey(d => d.SecondClass)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Professor__Secon__6EF57B66");
        });

        // Configuración de la entidad Student
        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StudentId).HasName("PK_Students");
            entity.Property(e => e.StudentId).ValueGeneratedNever();
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(100);
        });

        // Configuración de la entidad Enrollment
        modelBuilder.Entity<Enrollment>(entity =>
        {
            // Definir clave primaria compuesta
            entity.HasKey(e => new { e.StudentId, e.CourseId }).HasName("PK_Enrollments");

            // Configuración de índices únicos
            entity.HasIndex(e => e.StudentId).IsUnique().HasName("UQ_Enrollments_StudentId");
            entity.HasIndex(e => new { e.StudentId, e.ProfessorId }).IsUnique().HasName("UQ_Enrollments_Student_Professor");

            // Configuración de relaciones con otras entidades
            entity.HasOne(e => e.Student)
                .WithOne(s => s.Enrollment)
                .HasForeignKey<Enrollment>(e => e.StudentId)
                .OnDelete(DeleteBehavior.Cascade) // Eliminación en cascada al eliminar un estudiante
                .HasConstraintName("FK_Enrollments_Students");

            entity.HasOne(e => e.Course)
                .WithMany(c => c.Enrollments)
                .HasForeignKey(e => e.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull) // Establecer como nulo al eliminar un curso
                .HasConstraintName("FK_Enrollments_Courses");

            entity.HasOne(e => e.Professor)
                .WithMany(p => p.Enrollments)
                .HasForeignKey(e => e.ProfessorId)
                .HasConstraintName("FK_Enrollments_Professors");
        });


        OnModelCreatingPartial(modelBuilder);
    }


    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
