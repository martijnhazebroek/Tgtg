namespace Hazebroek.Tgtg.Auth
{
    internal sealed class LoginAttempt
    {
        public string Email { get; set; }

        public string UserDisplayName { get; set; }

        public LoginStatus Status { get; set; }

        public static LoginAttempt UserHasToAuthenticate(string userDisplayName)
        {
            return new LoginAttempt {UserDisplayName = userDisplayName, Status = LoginStatus.Reauthenticate};
        }

        public static LoginAttempt KnownUser(string email, string displayName)
        {
            return new LoginAttempt
                {Email = email, UserDisplayName = displayName, Status = LoginStatus.Success};
        }
    }

    public enum LoginStatus
    {
        Success,
        Reauthenticate
    }
}