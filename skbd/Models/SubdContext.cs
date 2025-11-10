using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace skbd.Models;

public partial class SubdContext : DbContext
{
    public SubdContext()
    {
    }

    public SubdContext(DbContextOptions<SubdContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Dbase> Dbases { get; set; }

    public virtual DbSet<Field> Fields { get; set; }

    public virtual DbSet<Row> Rows { get; set; }

    public virtual DbSet<Table> Tables { get; set; }

    public virtual DbSet<Type> Types { get; set; }

    public virtual DbSet<Value> Values { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    if (!optionsBuilder.IsConfigured)
    {
        optionsBuilder.UseSqlServer("Server=PROBOOK455\\SQLEXPRESS;Database=subd;Trusted_Connection=True;TrustServerCertificate=True;");
    }
}

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Server=PROBOOK455\\SQLEXPRESS;Database=subd;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Dbase>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Dtbs");

            entity.ToTable("Dbase");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Field>(entity =>
        {
            entity.ToTable("Field");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.TableId).HasColumnName("table_id");
            entity.Property(e => e.TypeId).HasColumnName("type_id");

            entity.HasOne(d => d.Table).WithMany(p => p.Fields)
                .HasForeignKey(d => d.TableId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Table_Field");

            entity.HasOne(d => d.Type).WithMany(p => p.Fields)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Type_Field");
        });

        modelBuilder.Entity<Row>(entity =>
        {
            entity.ToTable("Row");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.TableId).HasColumnName("table_id");

            entity.HasOne(d => d.Table).WithMany(p => p.Rows)
                .HasForeignKey(d => d.TableId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Row_Table");
        });

        modelBuilder.Entity<Table>(entity =>
        {
            entity.ToTable("Table");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DbId).HasColumnName("db_id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");

            entity.HasOne(d => d.Db).WithMany(p => p.Tables)
                .HasForeignKey(d => d.DbId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Table_Dtbs");
        });

        modelBuilder.Entity<Type>(entity =>
        {
            entity.ToTable("Type");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Value>(entity =>
        {
            entity.ToTable("Value");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FieldId).HasColumnName("field_id");
            entity.Property(e => e.RowId).HasColumnName("row_id");
            entity.Property(e => e.Val)
                .HasMaxLength(50)
                .IsFixedLength()
                .HasColumnName("val");

            entity.HasOne(d => d.Field).WithMany(p => p.Values)
                .HasForeignKey(d => d.FieldId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Value_Field");

            entity.HasOne(d => d.Row).WithMany(p => p.Values)
                .HasForeignKey(d => d.RowId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Value_Row");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
