namespace IConnet.Presale.Shared.Contracts;

public static class UriEndpoint
{
    public static class Identity
    {
        public const string SignIn = "/api/sign-in";
        public const string SignOut = "/api/sign-out";
        public const string SignUp = "/api/sign-up";
        public const string Refresh = "/api/sign-in/refresh";
        public const string ResetPassword = "/api/identity/reset-password";
        public const string SetRole = "/api/identity/set-role";
        public const string GrantPrivilege = "/api/identity/grant-privilege";
        public const string RevokePrivilege = "/api/identity/revoke-privilege";
        public const string GetUsers = "/api/identity/get-range";
        public const string GetPresaleOperators = "/api/identity/presale/presale-operators";
    }

    public static class Presale
    {
        public const string PushWorkPaper = "/api/presale/push-workpaper";
    }

    public static class ChatTemplate
    {
        public const string GetChatTemplates = "/api/chat-templates/get-{templateName}";
        public const string GetAvailableChatTemplates = "/api/chat-templates/get-range";
    }

    public static class DirectApproval
    {
        public const string GetDirectApprovals = "/api/direct-approvals/get";
        public const string AddDirectApproval = "/api/direct-approval/add";
        public const string ToggleSoftDeletion = "/api/direct-approval/toggle-soft-deletion";
    }

    public static class RepresentativeOffice
    {
        public const string GetRepresentativeOffices = "/api/representative-offices/get";
    }

    public static class RootCauses
    {
        public const string GetRootCauses = "/api/root-causes/get";
        public const string AddRootCause = "/api/root-cause/add";
        public const string ToggleSoftDeletion = "/api/root-cause/toggle-soft-deletion";
    }
}
