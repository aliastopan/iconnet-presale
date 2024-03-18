using Microsoft.Extensions.Configuration;
using IConnet.Presale.Domain.Aggregates.Identity;
using IConnet.Presale.Domain.Aggregates.Identity.ValueObjects;
using IConnet.Presale.Domain.Enums;
using IConnet.Presale.Domain.Entities;
using IConnet.Presale.Shared.Common;

[assembly: InternalsVisibleTo("IConnet.Presale.Tests")]
namespace IConnet.Presale.Infrastructure.Services;

internal sealed class DataSeedingProvider : IDataSeedingService
{
    private readonly AppDbContextFactory _dbContextFactory;
    private readonly IPasswordService _passwordService;
    private readonly IDateTimeService _dateTimeService;
    private readonly IConfiguration _configuration;

    public DataSeedingProvider(AppDbContextFactory dbContextFactory,
        IPasswordService passwordService,
        IDateTimeService dateTimeService,
        IConfiguration configuration)
    {
        _dbContextFactory = dbContextFactory;
        _passwordService = passwordService;
        _dateTimeService = dateTimeService;
        _configuration = configuration;
    }


    public async Task<int> GenerateSuperUserAsync()
    {
        Log.Information("Generating SuperUser");

        using var dbContext = _dbContextFactory.CreateDbContext();

        dbContext.UserAccounts.Add(new UserAccount
        {
            UserAccountId = Guid.Parse("06f631d6-14a8-4967-a9c0-78471586ca72"),
            User = new User
            {
                Username = "aliastopan",
                EmploymentStatus = EmploymentStatus.Internal,
                UserRole = UserRole.SuperUser,
                UserPrivileges = new List<UserPrivilege>()
                {
                    UserPrivilege.Viewer,
                    UserPrivilege.Editor,
                    UserPrivilege.Administrator
                },
                JobTitle = "Developer"
            },
            PasswordHash = _passwordService.HashPassword(_configuration["Credentials:SuperUser"]!, out var salt),
            PasswordSalt = salt,
            CreationDate = _dateTimeService.DateTimeOffsetNow,
            LastSignedIn = _dateTimeService.DateTimeOffsetNow
        });

        return await dbContext.SaveChangesAsync();
    }

    public async Task<int> GenerateUsersAsync()
    {
        var administrator01 = new UserAccount
        {
            UserAccountId = Guid.Parse("9dd0aa01-3a6e-4159-8c7b-8ee4caa1d4ea"),
            User = new User
            {
                Username = "erasmus",
                EmploymentStatus = EmploymentStatus.Internal,
                UserRole = UserRole.SuperUser,
                UserPrivileges = new List<UserPrivilege>()
                {
                    UserPrivilege.Viewer,
                    UserPrivilege.Editor,
                    UserPrivilege.Administrator
                },
                JobTitle = "Developer"
            },
            PasswordHash = _passwordService.HashPassword(_configuration["Credentials:Administrator"]!, out var salt),
            PasswordSalt = salt,
            CreationDate = _dateTimeService.DateTimeOffsetNow,
            LastSignedIn = _dateTimeService.DateTimeOffsetNow
        };

        var helpdesk01 = new UserAccount
        {
            UserAccountId = Guid.Parse("73946fa9-d92d-453a-a5c4-1f3daea5d66f"),
            User = new User
            {
                Username = "helpdesk",
                UserRole = UserRole.Helpdesk,
                UserPrivileges = new List<UserPrivilege>()
                {
                    UserPrivilege.Viewer,
                    UserPrivilege.Editor,
                },
                JobTitle = "Helpdesk"
            },
            PasswordHash = _passwordService.HashPassword("pwd", out salt),
            PasswordSalt = salt,
            CreationDate = _dateTimeService.DateTimeOffsetNow,
            LastSignedIn = _dateTimeService.DateTimeOffsetNow
        };

        var pac01 = new UserAccount
        {
            UserAccountId = Guid.Parse("3eadb218-b95c-48e9-886c-7859de74ba76"),
            User = new User
            {
                Username = "pac",
                UserRole = UserRole.PAC,
                UserPrivileges = new List<UserPrivilege>()
                {
                    UserPrivilege.Viewer,
                    UserPrivilege.Editor,
                },
                JobTitle = "Planning Asset Coverage"
            },
            PasswordHash = _passwordService.HashPassword("pwd", out salt),
            PasswordSalt = salt,
            CreationDate = _dateTimeService.DateTimeOffsetNow,
            LastSignedIn = _dateTimeService.DateTimeOffsetNow
        };

        using var dbContext = _dbContextFactory.CreateDbContext();

        dbContext.UserAccounts.Add(administrator01);
        dbContext.UserAccounts.Add(helpdesk01);
        dbContext.UserAccounts.Add(pac01);

        return await dbContext.SaveChangesAsync();
    }

    public async Task<int> GenerateChatTemplatesAsync()
    {
        Log.Information("Generating ChatTemplates");

        string templateName = "default";
        string greeting = "Selamat pagi kak, kami dari Helpdesk *ICONNET* ingin mengkorfimasi pemasangan baru 😊";
        string question = "Konfirmasi Data Calon Pelanggan\n\n" +
                $"Apakah benar nomor telpon aktif terdaftar *{PlaceholderText.NomorTelepon}*?\n\n" +
                $"Apakah benar Nama Pelanggan terdaftar atas nama *{PlaceholderText.NamaPelanggan}*?\n\n" +
                $"Apakah benar alamat email aktif terdaftar *{PlaceholderText.AlamatEmail}*?\n\n" +
                $"Apakah benar alamat lokasi pemasangan *{PlaceholderText.AlamatPemasangan}*?\n\n" +
                $"Apakah benar ID PLN terdaftar adalah *{PlaceholderText.IdPln}*?";
        string confirmation = "Konfirmasi Pendaftaran\n\n" +
                "1. Apakah sebelumnya sudah pernah berlangganan *ICONNET* di alamat terdaftar?\n\n" +
                "2. Mohon bisa share location dari WhatsApp agar mempermudah tim teknis saat pemasangan Wi-Fi.";
        string request = "Mohon pertanyaan diatas dikonfirmasi dan dijawab yah, Kak. Serta pastikan jawaban tidak ada manipulasi dari pihak-pihak lain, karena akan berpengaruh dengan layanan kakak yang terpasang kedepannya.";
        string closing = "Baik, Terimah Kasih telah menjawab pertanyaannya Kak. Saya mohon ijin pamit dan tutup chat-nya.";
        string note = "Mohon jangan lupa install aplikasi *MyICON+* yah kak agar memudahkan pembayaran billing & laporan gangguan 😊";

        var chatGreeting = new ChatTemplate(templateName, 0, greeting);
        var chatQuestion = new ChatTemplate(templateName, 1, question);
        var chatConfirmation = new ChatTemplate(templateName, 2, confirmation);
        var chatRequest = new ChatTemplate(templateName, 3, request);
        var chatClosing = new ChatTemplate(templateName, 4, closing);
        var chatNote = new ChatTemplate(templateName, 5, note);

        using var dbContext = _dbContextFactory.CreateDbContext();

        dbContext.ChatTemplates.Add(chatGreeting);
        dbContext.ChatTemplates.Add(chatQuestion);
        dbContext.ChatTemplates.Add(chatConfirmation);
        dbContext.ChatTemplates.Add(chatRequest);
        dbContext.ChatTemplates.Add(chatClosing);
        dbContext.ChatTemplates.Add(chatNote);

        return await dbContext.SaveChangesAsync();
    }

    public async Task<int> GenerateRepresentativeOfficesAsync()
    {
        Log.Information("Generating RepresentativeOffices");

        var kantorPerwakilan = new List<RepresentativeOffice>
        {
            new RepresentativeOffice(0, "SEMUA KANTOR PERWAKILAN"),
            new RepresentativeOffice(1, "SURABAYA"),
            new RepresentativeOffice(2, "JEMBER"),
            new RepresentativeOffice(3, "MADIUN"),
            new RepresentativeOffice(4, "MALANG"),
        };

        using var dbContext = _dbContextFactory.CreateDbContext();

        dbContext.RepresentativeOffices.AddRange(kantorPerwakilan);

        return await dbContext.SaveChangesAsync();
    }

    public async Task<int> GenerateRootCausesAsync()
    {
        Log.Information("Generating RootCauses");

        var rootCauses = new List<RootCause>
        {
            new RootCause(0, "Batal Pasang"),
            new RootCause(1, "Crossing Jalan Raya"),
            new RootCause(2, "Crossing Rel Kereta"),
            new RootCause(3, "Crossing Rumah"),
            new RootCause(4, "Crossing Sungai"),
            new RootCause(5, "Data Tidak Valid"),
            new RootCause(6, "Hanya Ada Tiang Penerangan"),
            new RootCause(7, "Jarak dari Tiang Terdekat Lebih dari 50 Meter"),
            new RootCause(8, "Kesalahan Data"),
            new RootCause(9, "Masuk Gang"),
            new RootCause(10, "Melewati Tiang Provider Lain"),
            new RootCause(11, "Perangkat Masih Ada di Rumah User"),
            new RootCause(12, "Pernah Refund"),
            new RootCause(13, "Perumahan Underground"),
            new RootCause(14, "Redaman Tinggi"),
            new RootCause(15, "Terhalang Tembok"),
            new RootCause(16, "Tidak Ada Respon 2 Hari"),
            new RootCause(17, "Tiang dalam Rumah"),
            new RootCause(18, "Tidak Ada Tiang PLN"),
            new RootCause(19, "Tidak Bisa Streetview"),
            new RootCause(20, "Tidak Tercover FAT Full"),
            new RootCause(21, "Tidak Tercover GPON Full"),
            new RootCause(22, "Tidak Tercover Jarak"),
            new RootCause(23, "Tikor di Sungai, Ladang, atau Jalan Raya"),
            new RootCause(24, "User Existing"),
        };

        using var dbContext = _dbContextFactory.CreateDbContext();

        dbContext.RootCauses.AddRange(rootCauses);

        return await dbContext.SaveChangesAsync();
    }
}
