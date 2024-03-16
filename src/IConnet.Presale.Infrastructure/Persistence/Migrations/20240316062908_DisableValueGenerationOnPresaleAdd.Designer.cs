﻿// <auto-generated />
using System;
using System.Collections.Generic;
using IConnet.Presale.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace IConnet.Presale.Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240316062908_DisableValueGenerationOnPresaleAdd")]
    partial class DisableValueGenerationOnPresaleAdd
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("IConnet.Presale.Domain.Aggregates.Identity.RefreshToken", b =>
                {
                    b.Property<Guid>("RefreshTokenId")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(36)
                        .HasColumnType("char(36)")
                        .HasColumnName("id");

                    b.Property<DateTimeOffset>("CreationDate")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("creation_date");

                    b.Property<DateTimeOffset>("ExpiryDate")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("expiry_date");

                    b.Property<Guid>("FkUserAccountId")
                        .HasColumnType("char(36)")
                        .HasColumnName("fk_user_account_id");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("is_deleted");

                    b.Property<bool>("IsInvalidated")
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("is_invalidated");

                    b.Property<bool>("IsUsed")
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("is_used");

                    b.Property<string>("Jti")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("jti");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("token");

                    b.HasKey("RefreshTokenId");

                    b.HasIndex("FkUserAccountId");

                    b.ToTable("dbo.refresh_token", (string)null);
                });

            modelBuilder.Entity("IConnet.Presale.Domain.Aggregates.Identity.UserAccount", b =>
                {
                    b.Property<Guid>("UserAccountId")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(36)
                        .HasColumnType("char(36)")
                        .HasColumnName("id")
                        .HasColumnOrder(0);

                    b.Property<DateTimeOffset>("CreationDate")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("creation_date")
                        .HasColumnOrder(9);

                    b.Property<DateTimeOffset>("LastSignedIn")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("last_signed_in")
                        .HasColumnOrder(10);

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(96)
                        .HasColumnType("varchar(96)")
                        .HasColumnName("password_hash")
                        .HasColumnOrder(7);

                    b.Property<string>("PasswordSalt")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("varchar(16)")
                        .HasColumnName("password_salt")
                        .HasColumnOrder(8);

                    b.ComplexProperty<Dictionary<string, object>>("User", "IConnet.Presale.Domain.Aggregates.Identity.UserAccount.User#User", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<int>("EmploymentStatus")
                                .HasColumnType("int")
                                .HasColumnName("employment_status")
                                .HasColumnOrder(4);

                            b1.Property<string>("JobTitle")
                                .IsRequired()
                                .HasColumnType("longtext")
                                .HasColumnName("job_title")
                                .HasColumnOrder(5);

                            b1.Property<string>("UserPrivileges")
                                .IsRequired()
                                .HasColumnType("longtext")
                                .HasColumnName("user_privileges")
                                .HasColumnOrder(3);

                            b1.Property<int>("UserRole")
                                .HasColumnType("int")
                                .HasColumnName("user_role")
                                .HasColumnOrder(2);

                            b1.Property<string>("Username")
                                .IsRequired()
                                .HasMaxLength(32)
                                .HasColumnType("varchar(32)")
                                .HasColumnName("username")
                                .HasColumnOrder(1);
                        });

                    b.HasKey("UserAccountId");

                    b.ToTable("dbo.user_account", (string)null);
                });

            modelBuilder.Entity("IConnet.Presale.Domain.Aggregates.Presales.ApprovalOpportunity", b =>
                {
                    b.Property<Guid>("ApprovalOpportunityId")
                        .HasMaxLength(36)
                        .HasColumnType("char(36)")
                        .HasColumnName("id");

                    b.Property<string>("IdPermohonan")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("id_permohonan");

                    b.Property<string>("JenisPermohonan")
                        .HasColumnType("longtext")
                        .HasColumnName("jenis_permohonan");

                    b.Property<string>("Layanan")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("layanan");

                    b.Property<string>("Splitter")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("splitter");

                    b.Property<int>("StatusImport")
                        .HasColumnType("int")
                        .HasColumnName("status_import");

                    b.Property<string>("StatusPermohonan")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("status_permohonan");

                    b.Property<string>("SumberPermohonan")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("sumber_permohonan");

                    b.Property<DateTime>("TglPermohonan")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("tgl_permohonan");

                    b.ComplexProperty<Dictionary<string, object>>("Agen", "IConnet.Presale.Domain.Aggregates.Presales.ApprovalOpportunity.Agen#Salesperson", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Email")
                                .IsRequired()
                                .HasColumnType("longtext")
                                .HasColumnName("agen_email");

                            b1.Property<string>("Mitra")
                                .IsRequired()
                                .HasColumnType("longtext")
                                .HasColumnName("agen_mitra");

                            b1.Property<string>("NamaLengkap")
                                .IsRequired()
                                .HasColumnType("longtext")
                                .HasColumnName("agen_nama_lengkap");

                            b1.Property<string>("NomorTelepon")
                                .IsRequired()
                                .HasColumnType("longtext")
                                .HasColumnName("agen_nomor_telepon");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("Pemohon", "IConnet.Presale.Domain.Aggregates.Presales.ApprovalOpportunity.Pemohon#Applicant", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Alamat")
                                .IsRequired()
                                .HasColumnType("longtext")
                                .HasColumnName("pemohon_alamat");

                            b1.Property<string>("Email")
                                .IsRequired()
                                .HasColumnType("longtext")
                                .HasColumnName("pemohon_email");

                            b1.Property<string>("IdPln")
                                .IsRequired()
                                .HasColumnType("longtext")
                                .HasColumnName("pemohon_id_pln");

                            b1.Property<string>("Keterangan")
                                .HasColumnType("longtext")
                                .HasColumnName("pemohon_keterangan");

                            b1.Property<string>("NamaPelanggan")
                                .IsRequired()
                                .HasColumnType("longtext")
                                .HasColumnName("pemohon_nama_pelanggan");

                            b1.Property<string>("Nik")
                                .HasColumnType("longtext")
                                .HasColumnName("pemohon_nik");

                            b1.Property<string>("NomorTelepon")
                                .IsRequired()
                                .HasColumnType("longtext")
                                .HasColumnName("pemohon_nomor_telepon");

                            b1.Property<string>("Npwp")
                                .HasColumnType("longtext")
                                .HasColumnName("pemohon_npwp");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("Regional", "IConnet.Presale.Domain.Aggregates.Presales.ApprovalOpportunity.Regional#Regional", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Bagian")
                                .IsRequired()
                                .HasColumnType("longtext")
                                .HasColumnName("regional_bagian");

                            b1.Property<string>("Kabupaten")
                                .IsRequired()
                                .HasColumnType("longtext")
                                .HasColumnName("regional_kabupaten");

                            b1.Property<string>("KantorPerwakilan")
                                .IsRequired()
                                .HasColumnType("longtext")
                                .HasColumnName("regional_kantor_perwakilan");

                            b1.Property<string>("Kecamatan")
                                .IsRequired()
                                .HasColumnType("longtext")
                                .HasColumnName("regional_kecamatan");

                            b1.Property<string>("Kelurahan")
                                .IsRequired()
                                .HasColumnType("longtext")
                                .HasColumnName("regional_kelurahan");

                            b1.Property<string>("Provinsi")
                                .IsRequired()
                                .HasColumnType("longtext")
                                .HasColumnName("regional_provinsi");

                            b1.ComplexProperty<Dictionary<string, object>>("Koordinat", "IConnet.Presale.Domain.Aggregates.Presales.ApprovalOpportunity.Regional#Regional.Koordinat#Coordinate", b2 =>
                                {
                                    b2.IsRequired();

                                    b2.Property<string>("Latitude")
                                        .IsRequired()
                                        .HasColumnType("longtext")
                                        .HasColumnName("regional_koordinat_latitude");

                                    b2.Property<string>("Longitude")
                                        .IsRequired()
                                        .HasColumnType("longtext")
                                        .HasColumnName("regional_koordinat_longitude");
                                });
                        });

                    b.ComplexProperty<Dictionary<string, object>>("SignatureImport", "IConnet.Presale.Domain.Aggregates.Presales.ApprovalOpportunity.SignatureImport#ActionSignature", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<Guid>("AccountIdSignature")
                                .HasColumnType("char(36)")
                                .HasColumnName("sign_import_account_id");

                            b1.Property<string>("Alias")
                                .IsRequired()
                                .HasColumnType("longtext")
                                .HasColumnName("sign_import_alias");

                            b1.Property<DateTime>("TglAksi")
                                .HasColumnType("datetime(6)")
                                .HasColumnName("sign_import_tgl_aksi");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("SignatureVerifikasiImport", "IConnet.Presale.Domain.Aggregates.Presales.ApprovalOpportunity.SignatureVerifikasiImport#ActionSignature", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<Guid>("AccountIdSignature")
                                .HasColumnType("char(36)")
                                .HasColumnName("sign_import_verifikasi_account_id");

                            b1.Property<string>("Alias")
                                .IsRequired()
                                .HasColumnType("longtext")
                                .HasColumnName("sign_import_verifikasi_alias");

                            b1.Property<DateTime>("TglAksi")
                                .HasColumnType("datetime(6)")
                                .HasColumnName("sign_import_verifikasi_tgl_aksi");
                        });

                    b.HasKey("ApprovalOpportunityId");

                    b.ToTable("dbo.approval_opportunity", (string)null);
                });

            modelBuilder.Entity("IConnet.Presale.Domain.Aggregates.Presales.WorkPaper", b =>
                {
                    b.Property<Guid>("WorkPaperId")
                        .HasMaxLength(36)
                        .HasColumnType("char(36)")
                        .HasColumnName("id");

                    b.Property<Guid>("FkApprovalOpportunityId")
                        .HasColumnType("char(36)")
                        .HasColumnName("fk_ao_id");

                    b.Property<string>("Shift")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("shift");

                    b.Property<int>("WorkPaperLevel")
                        .HasColumnType("int")
                        .HasColumnName("work_paper_level");

                    b.ComplexProperty<Dictionary<string, object>>("ProsesApproval", "IConnet.Presale.Domain.Aggregates.Presales.WorkPaper.ProsesApproval#ApprovalProcess", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<int>("JarakICrmPlus")
                                .HasColumnType("int")
                                .HasColumnName("approval_jarak_icrmplus");

                            b1.Property<int>("JarakShareLoc")
                                .HasColumnType("int")
                                .HasColumnName("approval_jarak_shareloc");

                            b1.Property<string>("Keterangan")
                                .HasColumnType("longtext")
                                .HasColumnName("keterangan_approval");

                            b1.Property<string>("RootCause")
                                .HasColumnType("longtext")
                                .HasColumnName("root_cause");

                            b1.Property<int>("StatusApproval")
                                .HasColumnType("int")
                                .HasColumnName("approval_status");

                            b1.Property<DateTime>("VaTerbit")
                                .HasColumnType("datetime(6)")
                                .HasColumnName("approval_va_terbit");

                            b1.ComplexProperty<Dictionary<string, object>>("SignatureApproval", "IConnet.Presale.Domain.Aggregates.Presales.WorkPaper.ProsesApproval#ApprovalProcess.SignatureApproval#ActionSignature", b2 =>
                                {
                                    b2.IsRequired();

                                    b2.Property<Guid>("AccountIdSignature")
                                        .HasColumnType("char(36)")
                                        .HasColumnName("sign_approval_account_id");

                                    b2.Property<string>("Alias")
                                        .IsRequired()
                                        .HasColumnType("longtext")
                                        .HasColumnName("sign_approval_alias");

                                    b2.Property<DateTime>("TglAksi")
                                        .HasColumnType("datetime(6)")
                                        .HasColumnName("sign_approval_tgl_aksi");
                                });
                        });

                    b.ComplexProperty<Dictionary<string, object>>("ProsesValidasi", "IConnet.Presale.Domain.Aggregates.Presales.WorkPaper.ProsesValidasi#ValidationProcess", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Keterangan")
                                .HasColumnType("longtext")
                                .HasColumnName("keterangan_validasi");

                            b1.Property<string>("LinkChatHistory")
                                .IsRequired()
                                .HasColumnType("longtext")
                                .HasColumnName("link_chat_history");

                            b1.Property<DateTime>("WaktuTanggalRespons")
                                .HasColumnType("datetime(6)")
                                .HasColumnName("waktu_tgl_respons");

                            b1.ComplexProperty<Dictionary<string, object>>("ParameterValidasi", "IConnet.Presale.Domain.Aggregates.Presales.WorkPaper.ProsesValidasi#ValidationProcess.ParameterValidasi#ValidationParameter", b2 =>
                                {
                                    b2.IsRequired();

                                    b2.Property<int>("ValidasiAlamat")
                                        .HasColumnType("int")
                                        .HasColumnName("validasi_alamat");

                                    b2.Property<int>("ValidasiEmail")
                                        .HasColumnType("int")
                                        .HasColumnName("validasi_email");

                                    b2.Property<int>("ValidasiIdPln")
                                        .HasColumnType("int")
                                        .HasColumnName("validasi_id_pln");

                                    b2.Property<int>("ValidasiNama")
                                        .HasColumnType("int")
                                        .HasColumnName("validasi_nama");

                                    b2.Property<int>("ValidasiNomorTelepon")
                                        .HasColumnType("int")
                                        .HasColumnName("validasi_nomor_telepon");

                                    b2.ComplexProperty<Dictionary<string, object>>("ShareLoc", "IConnet.Presale.Domain.Aggregates.Presales.WorkPaper.ProsesValidasi#ValidationProcess.ParameterValidasi#ValidationParameter.ShareLoc#Coordinate", b3 =>
                                        {
                                            b3.IsRequired();

                                            b3.Property<string>("Latitude")
                                                .IsRequired()
                                                .HasColumnType("longtext")
                                                .HasColumnName("regional_koordinat_latitude");

                                            b3.Property<string>("Longitude")
                                                .IsRequired()
                                                .HasColumnType("longtext")
                                                .HasColumnName("regional_koordinat_longitude");
                                        });
                                });

                            b1.ComplexProperty<Dictionary<string, object>>("PembetulanValidasi", "IConnet.Presale.Domain.Aggregates.Presales.WorkPaper.ProsesValidasi#ValidationProcess.PembetulanValidasi#ValidationCorrection", b2 =>
                                {
                                    b2.IsRequired();

                                    b2.Property<string>("PembetulanAlamat")
                                        .HasColumnType("longtext")
                                        .HasColumnName("pembetulan_alamat");

                                    b2.Property<string>("PembetulanEmail")
                                        .HasColumnType("longtext")
                                        .HasColumnName("pembetulan_email");

                                    b2.Property<string>("PembetulanIdPln")
                                        .HasColumnType("longtext")
                                        .HasColumnName("pembetulan_id_pln");

                                    b2.Property<string>("PembetulanNama")
                                        .HasColumnType("longtext")
                                        .HasColumnName("pembetulan_nama");

                                    b2.Property<string>("PembetulanNomorTelepon")
                                        .HasColumnType("longtext")
                                        .HasColumnName("pembetulan_nomor_telepon");
                                });

                            b1.ComplexProperty<Dictionary<string, object>>("SignatureChatCallMulai", "IConnet.Presale.Domain.Aggregates.Presales.WorkPaper.ProsesValidasi#ValidationProcess.SignatureChatCallMulai#ActionSignature", b2 =>
                                {
                                    b2.IsRequired();

                                    b2.Property<Guid>("AccountIdSignature")
                                        .HasColumnType("char(36)")
                                        .HasColumnName("sign_chat_call_mulai_account_id");

                                    b2.Property<string>("Alias")
                                        .IsRequired()
                                        .HasColumnType("longtext")
                                        .HasColumnName("sign_chat_call_mulai_alias");

                                    b2.Property<DateTime>("TglAksi")
                                        .HasColumnType("datetime(6)")
                                        .HasColumnName("sign_chat_call_mulai_tgl_aksi");
                                });

                            b1.ComplexProperty<Dictionary<string, object>>("SignatureChatCallRespons", "IConnet.Presale.Domain.Aggregates.Presales.WorkPaper.ProsesValidasi#ValidationProcess.SignatureChatCallRespons#ActionSignature", b2 =>
                                {
                                    b2.IsRequired();

                                    b2.Property<Guid>("AccountIdSignature")
                                        .HasColumnType("char(36)")
                                        .HasColumnName("sign_chat_call_respons_account_id");

                                    b2.Property<string>("Alias")
                                        .IsRequired()
                                        .HasColumnType("longtext")
                                        .HasColumnName("sign_chat_call_respons_alias");

                                    b2.Property<DateTime>("TglAksi")
                                        .HasColumnType("datetime(6)")
                                        .HasColumnName("sign_chat_call_respons_tgl_aksi");
                                });
                        });

                    b.ComplexProperty<Dictionary<string, object>>("SignatureHelpdeskInCharge", "IConnet.Presale.Domain.Aggregates.Presales.WorkPaper.SignatureHelpdeskInCharge#ActionSignature", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<Guid>("AccountIdSignature")
                                .HasColumnType("char(36)")
                                .HasColumnName("sign_ph_in_charge_account_id");

                            b1.Property<string>("Alias")
                                .IsRequired()
                                .HasColumnType("longtext")
                                .HasColumnName("sign_ph_in_charge_alias");

                            b1.Property<DateTime>("TglAksi")
                                .HasColumnType("datetime(6)")
                                .HasColumnName("sign_ph_in_charge_tgl_aksi");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("SignaturePlanningAssetCoverageInCharge", "IConnet.Presale.Domain.Aggregates.Presales.WorkPaper.SignaturePlanningAssetCoverageInCharge#ActionSignature", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<Guid>("AccountIdSignature")
                                .HasColumnType("char(36)")
                                .HasColumnName("sign_pac_in_charge_account_id");

                            b1.Property<string>("Alias")
                                .IsRequired()
                                .HasColumnType("longtext")
                                .HasColumnName("sign_pac_in_charge_alias");

                            b1.Property<DateTime>("TglAksi")
                                .HasColumnType("datetime(6)")
                                .HasColumnName("sign_pac_in_charge_tgl_aksi");
                        });

                    b.HasKey("WorkPaperId");

                    b.HasIndex("FkApprovalOpportunityId")
                        .IsUnique();

                    b.ToTable("dbo.work_paper", (string)null);
                });

            modelBuilder.Entity("IConnet.Presale.Domain.Entities.ChatTemplate", b =>
                {
                    b.Property<Guid>("ChatTemplateId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasColumnName("id");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("content");

                    b.Property<int>("Sequence")
                        .HasColumnType("int")
                        .HasColumnName("sequence");

                    b.Property<string>("TemplateName")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("varchar(32)")
                        .HasColumnName("template_name");

                    b.HasKey("ChatTemplateId");

                    b.ToTable("dbo.chat_template", (string)null);
                });

            modelBuilder.Entity("IConnet.Presale.Domain.Entities.RepresentativeOffice", b =>
                {
                    b.Property<Guid>("KantorPerwakilanId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasColumnName("id");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValue(false)
                        .HasColumnName("is_deleted");

                    b.Property<int>("Order")
                        .HasColumnType("int")
                        .HasColumnName("order");

                    b.Property<string>("Perwakilan")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("perwakilan");

                    b.HasKey("KantorPerwakilanId");

                    b.ToTable("dbo.kantor_perwakilan", (string)null);
                });

            modelBuilder.Entity("IConnet.Presale.Domain.Entities.RootCause", b =>
                {
                    b.Property<Guid>("RootCauseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasColumnName("id");

                    b.Property<string>("Cause")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("root_cause");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValue(false)
                        .HasColumnName("is_deleted");

                    b.Property<int>("Order")
                        .HasColumnType("int")
                        .HasColumnName("order");

                    b.HasKey("RootCauseId");

                    b.ToTable("dbo.root_cause", (string)null);
                });

            modelBuilder.Entity("IConnet.Presale.Domain.Aggregates.Identity.RefreshToken", b =>
                {
                    b.HasOne("IConnet.Presale.Domain.Aggregates.Identity.UserAccount", "UserAccount")
                        .WithMany("RefreshTokens")
                        .HasForeignKey("FkUserAccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserAccount");
                });

            modelBuilder.Entity("IConnet.Presale.Domain.Aggregates.Presales.WorkPaper", b =>
                {
                    b.HasOne("IConnet.Presale.Domain.Aggregates.Presales.ApprovalOpportunity", "ApprovalOpportunity")
                        .WithOne()
                        .HasForeignKey("IConnet.Presale.Domain.Aggregates.Presales.WorkPaper", "FkApprovalOpportunityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ApprovalOpportunity");
                });

            modelBuilder.Entity("IConnet.Presale.Domain.Aggregates.Identity.UserAccount", b =>
                {
                    b.Navigation("RefreshTokens");
                });
#pragma warning restore 612, 618
        }
    }
}
