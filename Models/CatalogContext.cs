using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CoroDr.CatalogAPI.Models;

public partial class CatalogContext : DbContext
{
    public CatalogContext()
    {
    }

    public CatalogContext(DbContextOptions<CatalogContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CatalogBrand> CatalogBrands { get; set; }

    public virtual DbSet<CatalogList> CatalogLists { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=corodrcatalog.database.windows.net;Database=Catalog;User Id=admindrcatalog;Password=AdminPassword@01;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CatalogBrand>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CatalogB__3214EC07E9E3BCBC");

            entity.ToTable("CatalogBrand");

            entity.Property(e => e.CountryName)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<CatalogList>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CatalogL__3214EC07E0A36B15");

            entity.ToTable("CatalogList");

            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ImageData).IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.CatalogBrand).WithMany(p => p.CatalogLists)
                .HasForeignKey(d => d.CatalogBrandId)
                .HasConstraintName("FK__CatalogLi__Catal__5EBF139D");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
