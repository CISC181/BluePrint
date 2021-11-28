﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using BluePrint.Shared.Models;

#nullable disable

namespace BluePrint.EF
{
    public partial class BluePrintOracleContext : DbContext
    {
        public BluePrintOracleContext()
        {
        }

        public BluePrintOracleContext(DbContextOptions<BluePrintOracleContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<Enrollment> Enrollments { get; set; }
        public virtual DbSet<Grade> Grades { get; set; }
        public virtual DbSet<GradeConversion> GradeConversions { get; set; }
        public virtual DbSet<GradeType> GradeTypes { get; set; }
        public virtual DbSet<GradeTypeWeight> GradeTypeWeights { get; set; }
        public virtual DbSet<Instructor> Instructors { get; set; }
        public virtual DbSet<Section> Sections { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Zipcode> Zipcodes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("STU")
                .HasAnnotation("Relational:Collation", "USING_NLS_COMP");

            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasKey(e => e.CourseNo)
                    .HasName("CRSE_PK");

                entity.Property(e => e.CourseNo).HasPrecision(8);

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.CreatedDate).ValueGeneratedOnAdd();

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.ModifiedBy)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ModifiedDate).ValueGeneratedOnAdd();

                entity.Property(e => e.Prerequisite).HasPrecision(8);

                entity.HasOne(d => d.PrerequisiteNavigation)
                    .WithMany(p => p.InversePrerequisiteNavigation)
                    .HasForeignKey(d => d.Prerequisite)
                    .HasConstraintName("CRSE_CRSE_FK");
            });

            modelBuilder.Entity<Enrollment>(entity =>
            {
                entity.HasKey(e => new { e.StudentId, e.SectionId })
                    .HasName("ENR_PK");

                entity.Property(e => e.StudentId).HasPrecision(8);

                entity.Property(e => e.SectionId).HasPrecision(8);

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.CreatedDate).ValueGeneratedOnAdd();

                entity.Property(e => e.FinalGrade).HasPrecision(3);

                entity.Property(e => e.ModifiedBy)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ModifiedDate).ValueGeneratedOnAdd();

                entity.HasOne(d => d.Section)
                    .WithMany(p => p.Enrollments)
                    .HasForeignKey(d => d.SectionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ENR_SECT_FK");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.Enrollments)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ENR_STU_FK");
            });

            modelBuilder.Entity<Grade>(entity =>
            {
                entity.HasKey(e => new { e.StudentId, e.SectionId, e.GradeTypeCode, e.GradeCodeOccurrence })
                    .HasName("GR_PK");

                entity.Property(e => e.StudentId).HasPrecision(8);

                entity.Property(e => e.SectionId).HasPrecision(8);

                entity.Property(e => e.GradeTypeCode)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Comments).IsUnicode(false);

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.CreatedDate).ValueGeneratedOnAdd();

                entity.Property(e => e.ModifiedBy)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ModifiedDate).ValueGeneratedOnAdd();

                entity.Property(e => e.NumericGrade)
                    .HasPrecision(3)
                    .HasDefaultValueSql("0");

                entity.HasOne(d => d.GradeTypeWeight)
                    .WithMany(p => p.Grades)
                    .HasForeignKey(d => new { d.SectionId, d.GradeTypeCode })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("GR_GRTW_FK");

                entity.HasOne(d => d.S)
                    .WithMany(p => p.Grades)
                    .HasForeignKey(d => new { d.StudentId, d.SectionId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("GR_ENR_FK");
            });

            modelBuilder.Entity<GradeConversion>(entity =>
            {
                entity.HasKey(e => e.LetterGrade)
                    .HasName("GRCON_PK");

                entity.Property(e => e.LetterGrade).IsUnicode(false);

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.CreatedDate).ValueGeneratedOnAdd();

                entity.Property(e => e.GradePoint).HasDefaultValueSql("0");

                entity.Property(e => e.MaxGrade).HasPrecision(3);

                entity.Property(e => e.MinGrade).HasPrecision(3);

                entity.Property(e => e.ModifiedBy)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ModifiedDate).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<GradeType>(entity =>
            {
                entity.HasKey(e => e.GradeTypeCode)
                    .HasName("GRTYP_PK");

                entity.Property(e => e.GradeTypeCode)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.CreatedDate).ValueGeneratedOnAdd();

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.ModifiedBy)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ModifiedDate).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<GradeTypeWeight>(entity =>
            {
                entity.HasKey(e => new { e.SectionId, e.GradeTypeCode })
                    .HasName("GRTW_PK");

                entity.Property(e => e.SectionId).HasPrecision(8);

                entity.Property(e => e.GradeTypeCode)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.CreatedDate).ValueGeneratedOnAdd();

                entity.Property(e => e.DropLowest)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.ModifiedBy)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ModifiedDate).ValueGeneratedOnAdd();

                entity.Property(e => e.NumberPerSection).HasPrecision(3);

                entity.Property(e => e.PercentOfFinalGrade).HasPrecision(3);

                entity.HasOne(d => d.GradeTypeCodeNavigation)
                    .WithMany(p => p.GradeTypeWeights)
                    .HasForeignKey(d => d.GradeTypeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("GRTW_GRTYP_FK");

                entity.HasOne(d => d.Section)
                    .WithMany(p => p.GradeTypeWeights)
                    .HasForeignKey(d => d.SectionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("GRTW_SECT_FK");
            });

            modelBuilder.Entity<Instructor>(entity =>
            {
                entity.Property(e => e.InstructorId).HasPrecision(8);

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.CreatedDate).ValueGeneratedOnAdd();

                entity.Property(e => e.FirstName).IsUnicode(false);

                entity.Property(e => e.LastName).IsUnicode(false);

                entity.Property(e => e.ModifiedBy)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ModifiedDate).ValueGeneratedOnAdd();

                entity.Property(e => e.Phone).IsUnicode(false);

                entity.Property(e => e.Salutation).IsUnicode(false);

                entity.Property(e => e.StreetAddress).IsUnicode(false);

                entity.Property(e => e.Zip).IsUnicode(false);

                entity.HasOne(d => d.ZipNavigation)
                    .WithMany(p => p.Instructors)
                    .HasForeignKey(d => d.Zip)
                    .HasConstraintName("INST_ZIP_FK");
            });

            modelBuilder.Entity<Section>(entity =>
            {
                entity.Property(e => e.SectionId).HasPrecision(8);

                entity.Property(e => e.Capacity).HasPrecision(3);

                entity.Property(e => e.CourseNo).HasPrecision(8);

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.CreatedDate).ValueGeneratedOnAdd();

                entity.Property(e => e.InstructorId).HasPrecision(8);

                entity.Property(e => e.Location).IsUnicode(false);

                entity.Property(e => e.ModifiedBy)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ModifiedDate).ValueGeneratedOnAdd();

                entity.Property(e => e.SectionNo).HasPrecision(3);

                entity.HasOne(d => d.CourseNoNavigation)
                    .WithMany(p => p.Sections)
                    .HasForeignKey(d => d.CourseNo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("SECT_CRSE_FK");

                entity.HasOne(d => d.Instructor)
                    .WithMany(p => p.Sections)
                    .HasForeignKey(d => d.InstructorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("SECT_INST_FK");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.Property(e => e.StudentId).HasPrecision(8);

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.CreatedDate).ValueGeneratedOnAdd();

                entity.Property(e => e.Employer).IsUnicode(false);

                entity.Property(e => e.FirstName).IsUnicode(false);

                entity.Property(e => e.LastName).IsUnicode(false);

                entity.Property(e => e.ModifiedBy)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ModifiedDate).ValueGeneratedOnAdd();

                entity.Property(e => e.Phone).IsUnicode(false);

                entity.Property(e => e.Salutation).IsUnicode(false);

                entity.Property(e => e.StreetAddress).IsUnicode(false);

                entity.Property(e => e.Zip).IsUnicode(false);

                entity.HasOne(d => d.ZipNavigation)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.Zip)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("STU_ZIP_FK");
            });

            modelBuilder.Entity<Zipcode>(entity =>
            {
                entity.HasKey(e => e.Zip)
                    .HasName("ZIP_PK");

                entity.Property(e => e.Zip).IsUnicode(false);

                entity.Property(e => e.City).IsUnicode(false);

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.CreatedDate).ValueGeneratedOnAdd();

                entity.Property(e => e.ModifiedBy)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ModifiedDate).ValueGeneratedOnAdd();

                entity.Property(e => e.State).IsUnicode(false);
            });

            modelBuilder.HasSequence("COURSE_NO_SEQ");

            modelBuilder.HasSequence("INSTRUCTOR_ID_SEQ");

            modelBuilder.HasSequence("SECTION_ID_SEQ");

            modelBuilder.HasSequence("STUDENT_ID_SEQ");

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
