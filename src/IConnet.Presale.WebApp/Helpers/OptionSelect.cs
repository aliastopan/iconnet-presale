namespace IConnet.Presale.WebApp.Helpers;

public static class OptionSelect
{
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
        public static string ClosedLost => StatusApprovalOptions.Skip(1).First();
        public static string Reject => StatusApprovalOptions.Skip(2).First();
        public static string Approve => StatusApprovalOptions.Last();
    }

    public static class StatusKepegawaian
    {
        public static IEnumerable<string> StatusKepegawaianOptions => EnumProcessor.GetStringOptions<EmploymentStatus>();
    }

    public static class Role
    {
        public static IEnumerable<string> RoleOptions => EnumProcessor.GetStringOptions<UserRole>(skipFirst: true);
        public static string SuperUser => RoleOptions.Last();
    }

    public static class Shift
    {
        public static IEnumerable<string> ShiftOptions => EnumProcessor.GetStringOptions<JobShift>();
    }
}
