using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using IConnet.Presale.Domain.Aggregates.Presales;
using IConnet.Presale.Domain.Aggregates.Presales.ValueObjects;
using IConnet.Presale.Domain.Enums;

namespace IConnet.Presale.Infrastructure.Persistence.Configurations.Presale;

internal sealed class WorkPaperConfiguration : IEntityTypeConfiguration<WorkPaper>
{
    public void Configure(EntityTypeBuilder<WorkPaper> builder)
    {
        builder.ToTable("dbo.work_paper");

        // primary key
        builder.HasKey(wp => wp.WorkPaperId);

        // configure properties
        builder.Property(wp => wp.WorkPaperId)
            .HasColumnName("work_paper_id")
            .HasMaxLength(36)
            .IsRequired();

        builder.Property(wp => wp.WorkPaperLevel)
            .HasColumnName("work_paper_level")
            .IsRequired();

        builder.Property(wp => wp.Shift)
            .HasColumnName("shift")
            .IsRequired();

        builder.OwnsOne(wp => wp.SignatureHelpdeskInCharge,
            signatureHelpdeskInCharge =>
            {
                signatureHelpdeskInCharge.Property(s => s.AccountIdSignature)
                    .HasColumnName("sign_ph_in_charge_account_id")
                    .IsRequired();

                signatureHelpdeskInCharge.Property(s => s.Alias)
                    .HasColumnName("sign_ph_in_charge_alias")
                    .IsRequired();

                signatureHelpdeskInCharge.Property(s => s.TglAksi)
                    .HasColumnName("sign_ph_in_charge_tgl_aksi")
                    .IsRequired();
            });

        builder.OwnsOne(wp => wp.SignaturePlanningAssetCoverageInCharge,
            signaturePlanningAssetCoverageInCharge =>
            {
                signaturePlanningAssetCoverageInCharge.Property(s => s.AccountIdSignature)
                    .HasColumnName("sign_pac_in_charge_account_id")
                    .IsRequired();

                signaturePlanningAssetCoverageInCharge.Property(s => s.Alias)
                    .HasColumnName("sign_pac_in_charge_alias")
                    .IsRequired();

                signaturePlanningAssetCoverageInCharge.Property(s => s.TglAksi)
                    .HasColumnName("sign_pac_in_charge_tgl_aksi")
                    .IsRequired();
            });

        builder.OwnsOne(wp => wp.ProsesValidasi,
            prosesValidasi =>
            {
                prosesValidasi.OwnsOne(pv => pv.SignatureChatCallMulai,
                    signatureChatCallMulai =>
                    {
                        signatureChatCallMulai.Property(s => s.AccountIdSignature)
                            .HasColumnName("sign_chat_call_mulai_account_id")
                            .IsRequired();

                        signatureChatCallMulai.Property(s => s.Alias)
                            .HasColumnName("sign_chat_call_mulai_alias")
                            .IsRequired();

                        signatureChatCallMulai.Property(s => s.TglAksi)
                            .HasColumnName("sign_chat_call_mulai_tgl_aksi")
                            .IsRequired();
                    });

                prosesValidasi.OwnsOne(pv => pv.SignatureChatCallRespons,
                    signatureChatCallRespons =>
                    {
                        signatureChatCallRespons.Property(s => s.AccountIdSignature)
                            .HasColumnName("sign_chat_call_respons_account_id")
                            .IsRequired();

                        signatureChatCallRespons.Property(s => s.Alias)
                            .HasColumnName("sign_chat_call_respons_alias")
                            .IsRequired();

                        signatureChatCallRespons.Property(s => s.TglAksi)
                            .HasColumnName("sign_chat_call_respons_tgl_aksi")
                            .IsRequired();
                    });

                prosesValidasi.Property(pv => pv.WaktuTanggalRespons)
                    .HasColumnName("waktu_tgl_respons")
                    .IsRequired();

                prosesValidasi.Property(pv => pv.LinkChatHistory)
                    .HasColumnName("link_chat_history")
                    .IsRequired();

                prosesValidasi.OwnsOne(pv => pv.ParameterValidasi,
                    parameterValidasi =>
                    {
                        parameterValidasi.Property(p => p.ValidasiIdPln)
                            .HasColumnName("validasi_id_pln")
                            .IsRequired();

                        parameterValidasi.Property(p => p.ValidasiNama)
                            .HasColumnName("validasi_nama")
                            .IsRequired();

                        parameterValidasi.Property(p => p.ValidasiNomorTelepon)
                            .HasColumnName("validasi_nomor_telepon")
                            .IsRequired();

                        parameterValidasi.Property(p => p.ValidasiEmail)
                            .HasColumnName("validasi_email")
                            .IsRequired();

                        parameterValidasi.Property(p => p.ValidasiAlamat)
                            .HasColumnName("validasi_alamat")
                            .IsRequired();

                        parameterValidasi.OwnsOne(r => r.ShareLoc,
                            koordinat =>
                            {
                                koordinat.Property(c => c.Latitude)
                                    .HasColumnName("regional_koordinat_latitude")
                                    .IsRequired();

                                koordinat.Property(c => c.Longitude)
                                    .HasColumnName("regional_koordinat_longitude")
                                    .IsRequired();
                            });
                    });

                prosesValidasi.OwnsOne(pv => pv.PembetulanValidasi,
                    pembetulanValidasi =>
                    {
                        pembetulanValidasi.Property(p => p.PembetulanIdPln)
                            .HasColumnName("pembetulan_id_pln");

                        pembetulanValidasi.Property(p => p.PembetulanNama)
                            .HasColumnName("pembetulan_nama");

                        pembetulanValidasi.Property(p => p.PembetulanNomorTelepon)
                            .HasColumnName("pembetulan_nomor_telepon");

                        pembetulanValidasi.Property(p => p.PembetulanEmail)
                            .HasColumnName("pembetulan_email");

                        pembetulanValidasi.Property(p => p.PembetulanAlamat)
                            .HasColumnName("pembetulan_alamat");
                    });

                prosesValidasi.Property(pv => pv.Keterangan)
                    .HasColumnName("keterangan_validasi");
            });

        builder.OwnsOne(wp => wp.ProsesApproval,
            prosesApproval =>
            {
                prosesApproval.OwnsOne(pa => pa.SignatureApproval,
                    signatureApproval =>
                    {
                        signatureApproval.Property(s => s.AccountIdSignature)
                            .HasColumnName("sign_approval_account_id")
                            .IsRequired();

                        signatureApproval.Property(s => s.Alias)
                            .HasColumnName("sign_approval_alias")
                            .IsRequired();

                        signatureApproval.Property(s => s.TglAksi)
                            .HasColumnName("sign_approval_tgl_aksi")
                            .IsRequired();
                    });

                prosesApproval.Property(pa => pa.StatusApproval)
                    .HasColumnName("approval_status")
                    .IsRequired();

                prosesApproval.Property(pa => pa.RootCause)
                    .HasColumnName("root_cause");

                prosesApproval.Property(pa => pa.Keterangan)
                    .HasColumnName("keterangan_approval");

                prosesApproval.Property(pa => pa.JarakShareLoc)
                    .HasColumnName("approval_jarak_shareloc");

                prosesApproval.Property(pa => pa.JarakICrmPlus)
                    .HasColumnName("approval_jarak_icrmplus");

                prosesApproval.Property(pa => pa.VaTerbit)
                    .HasColumnName("approval_va_terbit");

            });

        // configure relationships
        builder.HasOne(wp => wp.ApprovalOpportunity)
            .WithOne()
            .HasForeignKey<WorkPaper>(wp => wp.FkApprovalOpportunityId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
