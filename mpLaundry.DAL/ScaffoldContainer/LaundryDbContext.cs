using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace mpLaundry.DAL.ScaffoldContainer;

public partial class LaundryDbContext : DbContext
{
    public LaundryDbContext(DbContextOptions<LaundryDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<MLayanan> MLayanan { get; set; }

    public virtual DbSet<MPelanggan> MPelanggan { get; set; }

    public virtual DbSet<TrLaundry> TrLaundry { get; set; }

    public virtual DbSet<TrLaundryDetail> TrLaundryDetail { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MLayanan>(entity =>
        {
            entity.HasKey(e => e.PkLayananId);

            entity.ToTable("mLayanan");

            entity.HasIndex(e => e.TxtKodeLayanan, "UQ_mLayanan_txtKodeLayanan").IsUnique();

            entity.Property(e => e.PkLayananId)
                .HasDefaultValueSql("(newsequentialid())", "DF_mLayanan_pk")
                .HasColumnName("pkLayananId");
            entity.Property(e => e.BolActive).HasColumnName("bolActive");
            entity.Property(e => e.DecHarga)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("decHarga");
            entity.Property(e => e.DtInserted)
                .HasColumnType("datetime")
                .HasColumnName("dtInserted");
            entity.Property(e => e.DtUpdated)
                .HasColumnType("datetime")
                .HasColumnName("dtUpdated");
            entity.Property(e => e.IntEstimasiHari).HasColumnName("intEstimasiHari");
            entity.Property(e => e.RefInsertedBy)
                .HasMaxLength(50)
                .HasColumnName("refInsertedBy");
            entity.Property(e => e.RefUpdatedBy)
                .HasMaxLength(50)
                .HasColumnName("refUpdatedBy");
            entity.Property(e => e.TxtKodeLayanan)
                .HasMaxLength(10)
                .HasColumnName("txtKodeLayanan");
            entity.Property(e => e.TxtNamaLayanan)
                .HasMaxLength(100)
                .HasColumnName("txtNamaLayanan");
            entity.Property(e => e.TxtSatuan)
                .HasMaxLength(10)
                .HasColumnName("txtSatuan");
        });

        modelBuilder.Entity<MPelanggan>(entity =>
        {
            entity.HasKey(e => e.PkPelangganId);

            entity.ToTable("mPelanggan");

            entity.HasIndex(e => e.TxtKodePelanggan, "UQ_mPelanggan_txtKodePelanggan").IsUnique();

            entity.Property(e => e.PkPelangganId)
                .HasDefaultValueSql("(newsequentialid())", "DF_mPelanggan_pk")
                .HasColumnName("pkPelangganId");
            entity.Property(e => e.BolActive).HasColumnName("bolActive");
            entity.Property(e => e.DtInserted)
                .HasColumnType("datetime")
                .HasColumnName("dtInserted");
            entity.Property(e => e.DtUpdated)
                .HasColumnType("datetime")
                .HasColumnName("dtUpdated");
            entity.Property(e => e.RefInsertedBy)
                .HasMaxLength(50)
                .HasColumnName("refInsertedBy");
            entity.Property(e => e.RefUpdatedBy)
                .HasMaxLength(50)
                .HasColumnName("refUpdatedBy");
            entity.Property(e => e.TxtAlamat)
                .HasMaxLength(200)
                .HasColumnName("txtAlamat");
            entity.Property(e => e.TxtEmail)
                .HasMaxLength(100)
                .HasColumnName("txtEmail");
            entity.Property(e => e.TxtKodePelanggan)
                .HasMaxLength(10)
                .HasColumnName("txtKodePelanggan");
            entity.Property(e => e.TxtNama)
                .HasMaxLength(100)
                .HasColumnName("txtNama");
            entity.Property(e => e.TxtNoTelepon)
                .HasMaxLength(20)
                .HasColumnName("txtNoTelepon");
        });

        modelBuilder.Entity<TrLaundry>(entity =>
        {
            entity.HasKey(e => e.PkLaundryId);

            entity.ToTable("trLaundry");

            entity.HasIndex(e => e.RefPelangganId, "IX_trLaundry_refPelangganId");

            entity.HasIndex(e => e.TxtStatus, "IX_trLaundry_txtStatus");

            entity.HasIndex(e => e.TxtNoOrder, "UQ_trLaundry_txtNoOrder").IsUnique();

            entity.Property(e => e.PkLaundryId)
                .HasDefaultValueSql("(newsequentialid())", "DF_trLaundry_pk")
                .HasColumnName("pkLaundryId");
            entity.Property(e => e.BolLunas).HasColumnName("bolLunas");
            entity.Property(e => e.DecBayar)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("decBayar");
            entity.Property(e => e.DecTotalHarga)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("decTotalHarga");
            entity.Property(e => e.DtDiambil)
                .HasColumnType("datetime")
                .HasColumnName("dtDiambil");
            entity.Property(e => e.DtEstimasiSelesai)
                .HasColumnType("datetime")
                .HasColumnName("dtEstimasiSelesai");
            entity.Property(e => e.DtInserted)
                .HasColumnType("datetime")
                .HasColumnName("dtInserted");
            entity.Property(e => e.DtMasuk)
                .HasColumnType("datetime")
                .HasColumnName("dtMasuk");
            entity.Property(e => e.DtUpdated)
                .HasColumnType("datetime")
                .HasColumnName("dtUpdated");
            entity.Property(e => e.RefInsertedBy)
                .HasMaxLength(50)
                .HasColumnName("refInsertedBy");
            entity.Property(e => e.RefPelangganId).HasColumnName("refPelangganId");
            entity.Property(e => e.RefUpdatedBy)
                .HasMaxLength(50)
                .HasColumnName("refUpdatedBy");
            entity.Property(e => e.TxtKeterangan)
                .HasMaxLength(250)
                .HasColumnName("txtKeterangan");
            entity.Property(e => e.TxtNoOrder)
                .HasMaxLength(20)
                .HasColumnName("txtNoOrder");
            entity.Property(e => e.TxtStatus)
                .HasMaxLength(20)
                .HasColumnName("txtStatus");

            entity.HasOne(d => d.RefPelanggan).WithMany(p => p.TrLaundry)
                .HasForeignKey(d => d.RefPelangganId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_trLaundry_mPelanggan");
        });

        modelBuilder.Entity<TrLaundryDetail>(entity =>
        {
            entity.HasKey(e => e.PkLaundryDetailId);

            entity.ToTable("trLaundryDetail");

            entity.HasIndex(e => e.RefLaundryId, "IX_trLaundryDetail_refLaundryId");

            entity.HasIndex(e => e.RefLayananId, "IX_trLaundryDetail_refLayananId");

            entity.Property(e => e.PkLaundryDetailId)
                .HasDefaultValueSql("(newsequentialid())", "DF_trLaundryDetail_pk")
                .HasColumnName("pkLaundryDetailId");
            entity.Property(e => e.DecHargaSatuan)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("decHargaSatuan");
            entity.Property(e => e.DecQty)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("decQty");
            entity.Property(e => e.DecSubtotal)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("decSubtotal");
            entity.Property(e => e.DtInserted)
                .HasColumnType("datetime")
                .HasColumnName("dtInserted");
            entity.Property(e => e.DtUpdated)
                .HasColumnType("datetime")
                .HasColumnName("dtUpdated");
            entity.Property(e => e.RefInsertedBy)
                .HasMaxLength(50)
                .HasColumnName("refInsertedBy");
            entity.Property(e => e.RefLaundryId).HasColumnName("refLaundryId");
            entity.Property(e => e.RefLayananId).HasColumnName("refLayananId");
            entity.Property(e => e.RefUpdatedBy)
                .HasMaxLength(50)
                .HasColumnName("refUpdatedBy");
            entity.Property(e => e.TxtCatatan)
                .HasMaxLength(100)
                .HasColumnName("txtCatatan");

            entity.HasOne(d => d.RefLaundry).WithMany(p => p.TrLaundryDetail)
                .HasForeignKey(d => d.RefLaundryId)
                .HasConstraintName("FK_trLaundryDetail_trLaundry");

            entity.HasOne(d => d.RefLayanan).WithMany(p => p.TrLaundryDetail)
                .HasForeignKey(d => d.RefLayananId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_trLaundryDetail_mLayanan");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
