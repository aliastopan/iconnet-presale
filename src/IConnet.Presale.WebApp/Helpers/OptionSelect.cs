namespace IConnet.Presale.WebApp.Helpers;

public static class OptionSelect
{
    public static class Pagination
    {
        private static readonly List<string> _itemsPerPageOptions = ["25", "50", "75", "100"];
        public static List<string> ItemsPerPageOptions => _itemsPerPageOptions;
    }

    public static class StatusVerifikasi
    {
        public static IEnumerable<string> StatusVerifikasiOptions => EnumProcessor.GetStringOptions<VerificationStatus>();
        public static string MenungguVerifikasi => StatusVerifikasiOptions.First();
        public static string DataTidakSesuai => StatusVerifikasiOptions.Skip(1).First();
        public static string DataSesuai => StatusVerifikasiOptions.Last();
    }

    public static class StatusValidasi
    {
        public static IEnumerable<string> StatusValidasiOptions => EnumProcessor.GetStringOptions<ValidationStatus>();
        public static string MenungguValidasi => StatusValidasiOptions.First();
        public static string TidakSesuai => StatusValidasiOptions.Skip(1).First();
        public static string Sesuai => StatusValidasiOptions.Last();
    }

    public static class StatusApproval
    {
        public static IEnumerable<string> StatusApprovalOptions => EnumProcessor.GetStringOptions<ApprovalStatus>();
        public static string OnProgress => StatusApprovalOptions.First();
        public static string CloseLost => StatusApprovalOptions.Skip(1).First();
        public static string Reject => StatusApprovalOptions.Skip(2).First();
        public static string Expansion => StatusApprovalOptions.Skip(3).First();
        public static string Approve => StatusApprovalOptions.Last();
    }

    public static class StatusKepegawaian
    {
        public static IEnumerable<string> StatusKepegawaianOptions => EnumProcessor.GetStringOptions<EmploymentStatus>();
    }

    public static class Role
    {
        public static IEnumerable<string> RoleOptions => EnumProcessor.GetStringOptions<UserRole>();
        public static string SuperUser => RoleOptions.Last();
    }
}
