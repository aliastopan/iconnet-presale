using Microsoft.Extensions.Configuration;
using IConnet.Presale.Domain.Aggregates.Identity;
using IConnet.Presale.Domain.Enums;
using IConnet.Presale.Domain.Entities;

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

    public async Task<int> GenerateUsersAsync()
    {
        var administrator01 = new UserAccount
        {
            UserAccountId = Guid.Parse("9dd0aa01-3a6e-4159-8c7b-8ee4caa1d4ea"),
            User = new User
            {
                Username = "erasmus",
                EmailAddress = "erasmus@proton.me",
                EmploymentStatus = EmploymentStatus.Permanent,
                UserRole = UserRole.Administrator,
                UserPrivileges = new List<UserPrivilege>()
                {
                    UserPrivilege.Viewer,
                    UserPrivilege.Editor,
                    UserPrivilege.Administrator
                },
                JobShift = JobShift.Siang,
                JobTitle = "Developer"
            },
            UserProfile = new UserProfile
            {
                FirstName = "Taufan",
                LastName = "Augusta",
                DateOfBirth = new DateOnly(year: 1996, month: 8, day: 19)
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
                EmailAddress = "helpdesk.first@mail.me",
                UserRole = UserRole.Helpdesk,
                UserPrivileges = new List<UserPrivilege>()
                {
                    UserPrivilege.Viewer,
                    UserPrivilege.Editor,
                },
                JobShift = JobShift.Siang,
                JobTitle = "Helpdesk"
            },
            UserProfile = new UserProfile
            {
                FirstName = "Andy",
                LastName = "Wijaya",
                DateOfBirth = new DateOnly(year: 1997, month: 5, day: 10)
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
                EmailAddress = "pac.first@mail.me",
                UserRole = UserRole.PlanningAssetCoverage,
                UserPrivileges = new List<UserPrivilege>()
                {
                    UserPrivilege.Viewer,
                    UserPrivilege.Editor,
                },
                JobShift = JobShift.Siang,
                JobTitle = "Planning Asset Coverage"
            },
            UserProfile = new UserProfile
            {
                FirstName = "Rizky",
                LastName = "Moneter",
                DateOfBirth = new DateOnly(year: 1998, month: 11, day: 7)
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

    public async Task<int> GenerateChatTemplateAsync()
    {
        string templateName = "default";
        string greeting = "Selamat pagi kak, kami dari Helpdesk *ICONNET* ingin mengkorfimasi pemasangan baru 😊";
        string question = "Konfirmasi Data Calon Pelanggan\n\n" +
                "Apakah benar nomor telpon aktif terdaftar [$NOMORTELEPON]?\n\n" +
                "Apakah benar Nama Pelanggan terdaftar atas nama [$NAMAPELANGGAN]?\n\n" +
                "Apakah benar alamat email aktif terdaftar [$EMAIL]?\n\n" +
                "Apakah benar alamat lokasi pemasangan [$ALAMAT]?\n\n" +
                "Apakah benar ID PLN terdaftar adalah [$IDPLN]?";
        string confirmation = "Konfirmasi Pendaftaran\n\n" +
                "1. Apakah sebelumnya sudah pernah berlangganan *ICONNET* di alamat terdaftar?\n\n" +
                "2. Mohon bisa share location dari WhatsApp agar mempermudah tim teknis saat pemasangan Wi-Fi.";
        string request = "Mohon pertanyaan diatas dikonfirmasi dan dijawab yah, Kak. Serta pastikan jawaban tidak ada manipulasi dari pihak-pihak lain, karena akan berpengaruh dengan layanan kakak yang terpasang kedepannya.";
        string closing = "Baik, Terimah Kasih telah menjawab pertanyaannya Kak. Saya mohon ijin pamit dan tutup chat-nya.";
        string note = "Mohon jangan lupa _install_ aplikasi *MyICON+* yah kak agar memudahkan pembayaran billing & laporan gangguan 😊";

        var chatGreeting = new ChatTemplate(templateName, 0, greeting);
        var chatQuestion = new ChatTemplate(templateName, 1, question);
        var chatConfirmation = new ChatTemplate(templateName, 2, confirmation);
        var chatRequest = new ChatTemplate(templateName, 3, request);
        var chatClosing = new ChatTemplate(templateName, 4, closing);
        var chatNote = new ChatTemplate(templateName, 4, note);

        using var dbContext = _dbContextFactory.CreateDbContext();

        dbContext.ChatTemplates.Add(chatGreeting);
        dbContext.ChatTemplates.Add(chatQuestion);
        dbContext.ChatTemplates.Add(chatConfirmation);
        dbContext.ChatTemplates.Add(chatRequest);
        dbContext.ChatTemplates.Add(chatClosing);
        dbContext.ChatTemplates.Add(chatNote);

        return await dbContext.SaveChangesAsync();

    }
}
