using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IConnet.Presale.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "dbo.approval_opportunity",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", maxLength: 36, nullable: false, collation: "ascii_general_ci"),
                    id_permohonan = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    tgl_permohonan = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    status_permohonan = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    layanan = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    sumber_permohonan = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    jenis_permohonan = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    splitter = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status_import = table.Column<int>(type: "int", nullable: false),
                    agen_email = table.Column<string>(type: "longtext", nullable: false),
                    agen_mitra = table.Column<string>(type: "longtext", nullable: false),
                    agen_nama_lengkap = table.Column<string>(type: "longtext", nullable: false),
                    agen_nomor_telepon = table.Column<string>(type: "longtext", nullable: false),
                    pemohon_alamat = table.Column<string>(type: "longtext", nullable: false),
                    pemohon_email = table.Column<string>(type: "longtext", nullable: false),
                    pemohon_id_pln = table.Column<string>(type: "longtext", nullable: false),
                    pemohon_keterangan = table.Column<string>(type: "longtext", nullable: true),
                    pemohon_nama_pelanggan = table.Column<string>(type: "longtext", nullable: false),
                    pemohon_nik = table.Column<string>(type: "longtext", nullable: true),
                    pemohon_nomor_telepon = table.Column<string>(type: "longtext", nullable: false),
                    pemohon_npwp = table.Column<string>(type: "longtext", nullable: true),
                    regional_bagian = table.Column<string>(type: "longtext", nullable: false),
                    regional_kabupaten = table.Column<string>(type: "longtext", nullable: false),
                    regional_kantor_perwakilan = table.Column<string>(type: "longtext", nullable: false),
                    regional_kecamatan = table.Column<string>(type: "longtext", nullable: false),
                    regional_kelurahan = table.Column<string>(type: "longtext", nullable: false),
                    regional_provinsi = table.Column<string>(type: "longtext", nullable: false),
                    regional_koordinat_latitude = table.Column<string>(type: "longtext", nullable: false),
                    regional_koordinat_longitude = table.Column<string>(type: "longtext", nullable: false),
                    sign_import_account_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    sign_import_alias = table.Column<string>(type: "longtext", nullable: false),
                    sign_import_tgl_aksi = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    sign_import_verifikasi_account_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    sign_import_verifikasi_alias = table.Column<string>(type: "longtext", nullable: false),
                    sign_import_verifikasi_tgl_aksi = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbo.approval_opportunity", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "dbo.chat_template",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    template_name = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    sequence = table.Column<int>(type: "int", nullable: false),
                    content = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbo.chat_template", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "dbo.kantor_perwakilan",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    order = table.Column<int>(type: "int", nullable: false),
                    perwakilan = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    is_deleted = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbo.kantor_perwakilan", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "dbo.root_cause",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    order = table.Column<int>(type: "int", nullable: false),
                    root_cause = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    is_deleted = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbo.root_cause", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "dbo.user_account",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", maxLength: 36, nullable: false, collation: "ascii_general_ci"),
                    username = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false),
                    user_role = table.Column<int>(type: "int", nullable: false),
                    user_privileges = table.Column<string>(type: "longtext", nullable: false),
                    employment_status = table.Column<int>(type: "int", nullable: false),
                    job_title = table.Column<string>(type: "longtext", nullable: false),
                    job_shift = table.Column<int>(type: "int", maxLength: 32, nullable: false),
                    password_hash = table.Column<string>(type: "varchar(96)", maxLength: 96, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    password_salt = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    creation_date = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    last_signed_in = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbo.user_account", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "dbo.work_paper",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", maxLength: 36, nullable: false, collation: "ascii_general_ci"),
                    work_paper_level = table.Column<int>(type: "int", nullable: false),
                    shift = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    fk_ao_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    approval_jarak_icrmplus = table.Column<int>(type: "int", nullable: false),
                    approval_jarak_shareloc = table.Column<int>(type: "int", nullable: false),
                    keterangan_approval = table.Column<string>(type: "longtext", nullable: true),
                    root_cause = table.Column<string>(type: "longtext", nullable: true),
                    approval_status = table.Column<int>(type: "int", nullable: false),
                    approval_va_terbit = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    sign_approval_account_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    sign_approval_alias = table.Column<string>(type: "longtext", nullable: false),
                    sign_approval_tgl_aksi = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    keterangan_validasi = table.Column<string>(type: "longtext", nullable: true),
                    link_chat_history = table.Column<string>(type: "longtext", nullable: false),
                    waktu_tgl_respons = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    validasi_alamat = table.Column<int>(type: "int", nullable: false),
                    validasi_email = table.Column<int>(type: "int", nullable: false),
                    validasi_id_pln = table.Column<int>(type: "int", nullable: false),
                    validasi_nama = table.Column<int>(type: "int", nullable: false),
                    validasi_nomor_telepon = table.Column<int>(type: "int", nullable: false),
                    regional_koordinat_latitude = table.Column<string>(type: "longtext", nullable: false),
                    regional_koordinat_longitude = table.Column<string>(type: "longtext", nullable: false),
                    pembetulan_alamat = table.Column<string>(type: "longtext", nullable: true),
                    pembetulan_email = table.Column<string>(type: "longtext", nullable: true),
                    pembetulan_id_pln = table.Column<string>(type: "longtext", nullable: true),
                    pembetulan_nama = table.Column<string>(type: "longtext", nullable: true),
                    pembetulan_nomor_telepon = table.Column<string>(type: "longtext", nullable: true),
                    sign_chat_call_mulai_account_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    sign_chat_call_mulai_alias = table.Column<string>(type: "longtext", nullable: false),
                    sign_chat_call_mulai_tgl_aksi = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    sign_chat_call_respons_account_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    sign_chat_call_respons_alias = table.Column<string>(type: "longtext", nullable: false),
                    sign_chat_call_respons_tgl_aksi = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    sign_ph_in_charge_account_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    sign_ph_in_charge_alias = table.Column<string>(type: "longtext", nullable: false),
                    sign_ph_in_charge_tgl_aksi = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    sign_pac_in_charge_account_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    sign_pac_in_charge_alias = table.Column<string>(type: "longtext", nullable: false),
                    sign_pac_in_charge_tgl_aksi = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbo.work_paper", x => x.id);
                    table.ForeignKey(
                        name: "FK_dbo.work_paper_dbo.approval_opportunity_fk_ao_id",
                        column: x => x.fk_ao_id,
                        principalTable: "dbo.approval_opportunity",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "dbo.refresh_token",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", maxLength: 36, nullable: false, collation: "ascii_general_ci"),
                    token = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    jti = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    creation_date = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    expiry_date = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    is_used = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    is_invalidated = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    is_deleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    fk_user_account_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbo.refresh_token", x => x.id);
                    table.ForeignKey(
                        name: "FK_dbo.refresh_token_dbo.user_account_fk_user_account_id",
                        column: x => x.fk_user_account_id,
                        principalTable: "dbo.user_account",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_dbo.refresh_token_fk_user_account_id",
                table: "dbo.refresh_token",
                column: "fk_user_account_id");

            migrationBuilder.CreateIndex(
                name: "IX_dbo.work_paper_fk_ao_id",
                table: "dbo.work_paper",
                column: "fk_ao_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "dbo.chat_template");

            migrationBuilder.DropTable(
                name: "dbo.kantor_perwakilan");

            migrationBuilder.DropTable(
                name: "dbo.refresh_token");

            migrationBuilder.DropTable(
                name: "dbo.root_cause");

            migrationBuilder.DropTable(
                name: "dbo.work_paper");

            migrationBuilder.DropTable(
                name: "dbo.user_account");

            migrationBuilder.DropTable(
                name: "dbo.approval_opportunity");
        }
    }
}
