namespace IConnet.Presale.Api.Endpoints;

public static class ApiEndpoint
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
    }

    public static class ChatTemplate
    {
        public const string GetChatTemplate = "/api/chat-template/get-{templateName}";
    }
}
