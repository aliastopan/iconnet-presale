using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using IConnet.Presale.Domain.Aggregates.Presales;
using IConnet.Presale.Domain.Aggregates.Presales.ValueObjects;
using IConnet.Presale.Domain.Enums;

namespace IConnet.Presale.Infrastructure.Persistence.Configurations.Presale;

internal sealed class ApprovalOpportunityConfiguration : IEntityTypeConfiguration<ApprovalOpportunity>
{
    public void Configure(EntityTypeBuilder<ApprovalOpportunity> builder)
    {
        builder.ToTable("dbo.approval_opportunity");

        // primary key
        builder.HasKey(ao => ao.ApprovalOpportunityId);

        // configure properties
        builder.Property(ao => ao.ApprovalOpportunityId)
            .HasColumnName("id")
            .HasMaxLength(36)
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(ao => ao.IdPermohonan)
            .HasColumnName("id_permohonan")
            .IsRequired();

        builder.HasIndex(ao => ao.IdPermohonan)
            .HasDatabaseName("IX_id_permohonan");

        builder.Property(ao => ao.TglPermohonan)
            .HasColumnName("tgl_permohonan")
            .IsRequired();

        builder.Property(ao => ao.StatusPermohonan)
            .HasColumnName("status_permohonan")
            .IsRequired();

        builder.Property(ao => ao.Layanan)
            .HasColumnName("layanan")
            .IsRequired();

        builder.Property(ao => ao.SumberPermohonan)
            .HasColumnName("sumber_permohonan")
            .IsRequired();

        builder.Property(ao => ao.JenisPermohonan)
            .HasColumnName("jenis_permohonan");

        builder.Property(ao => ao.Splitter)
            .HasColumnName("splitter")
            .IsRequired();

        builder.ComplexProperty(ao => ao.Pemohon,
            pemohon =>
            {
                pemohon.IsRequired();

                pemohon.Property(p => p.IdPln)
                    .HasColumnName("pemohon_id_pln")
                    .IsRequired();

                pemohon.Property(p => p.NamaPelanggan)
                    .HasColumnName("pemohon_nama_pelanggan")
                    .IsRequired();

                pemohon.Property(p => p.NomorTelepon)
                    .HasColumnName("pemohon_nomor_telepon")
                    .IsRequired();

                pemohon.Property(p => p.Email)
                    .HasColumnName("pemohon_email")
                    .IsRequired();

                pemohon.Property(p => p.Alamat)
                    .HasColumnName("pemohon_alamat")
                    .IsRequired();

                pemohon.Property(p => p.Nik)
                    .HasColumnName("pemohon_nik");

                pemohon.Property(p => p.Npwp)
                    .HasColumnName("pemohon_npwp");

                pemohon.Property(p => p.Keterangan)
                    .HasColumnName("pemohon_keterangan");
            });

        builder.ComplexProperty(ao => ao.Agen,
            agen =>
            {
                agen.IsRequired();

                agen.Property(a => a.NamaLengkap)
                    .HasColumnName("agen_nama_lengkap")
                    .IsRequired();

                agen.Property(a => a.Email)
                    .HasColumnName("agen_email")
                    .IsRequired();

                agen.Property(a => a.NomorTelepon)
                    .HasColumnName("agen_nomor_telepon")
                    .IsRequired();

                agen.Property(a => a.Mitra)
                    .HasColumnName("agen_mitra")
                    .IsRequired();
            });

        builder.ComplexProperty(ao => ao.Regional,
            regional =>
            {
                regional.IsRequired();

                regional.Property(r => r.Bagian)
                    .HasColumnName("regional_bagian")
                    .IsRequired();

                regional.Property(r => r.KantorPerwakilan)
                    .HasColumnName("regional_kantor_perwakilan")
                    .IsRequired();

                regional.Property(r => r.Provinsi)
                    .HasColumnName("regional_provinsi")
                    .IsRequired();

                regional.Property(r => r.Kabupaten)
                    .HasColumnName("regional_kabupaten")
                    .IsRequired();

                regional.Property(r => r.Kecamatan)
                    .HasColumnName("regional_kecamatan")
                    .IsRequired();

                regional.Property(r => r.Kelurahan)
                    .HasColumnName("regional_kelurahan")
                    .IsRequired();

                regional.ComplexProperty(r => r.Koordinat,
                    koordinat =>
                    {
                        koordinat.IsRequired();

                        koordinat.Property(c => c.Latitude)
                            .HasColumnName("regional_koordinat_latitude")
                            .IsRequired();

                        koordinat.Property(c => c.Longitude)
                            .HasColumnName("regional_koordinat_longitude")
                            .IsRequired();
                    });
            });

        builder.Property(ao => ao.StatusImport)
            .HasColumnName("status_import")
            .IsRequired();

        builder.ComplexProperty(ao => ao.SignatureImport,
            signatureImport =>
            {
                signatureImport.IsRequired();

                signatureImport.Property(s => s.AccountIdSignature)
                    .HasColumnName("sign_import_account_id")
                    .IsRequired();

                signatureImport.Property(s => s.Alias)
                    .HasColumnName("sign_import_alias")
                    .IsRequired();

                signatureImport.Property(s => s.TglAksi)
                    .HasColumnName("sign_import_tgl_aksi")
                    .IsRequired();
            });

        builder.ComplexProperty(ao => ao.SignatureVerifikasiImport,
            signatureVerifikasiImport =>
            {
                signatureVerifikasiImport.IsRequired();

                signatureVerifikasiImport.Property(s => s.AccountIdSignature)
                    .HasColumnName("sign_import_verifikasi_account_id")
                    .IsRequired();

                signatureVerifikasiImport.Property(s => s.Alias)
                    .HasColumnName("sign_import_verifikasi_alias")
                    .IsRequired();

                signatureVerifikasiImport.Property(s => s.TglAksi)
                    .HasColumnName("sign_import_verifikasi_tgl_aksi")
                    .IsRequired();
            });
    }
}
