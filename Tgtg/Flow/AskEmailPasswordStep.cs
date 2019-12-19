using Colorful;
using Hazebroek.Tgtg.Auth;
using McMaster.Extensions.CommandLineUtils;

namespace Hazebroek.Tgtg.Flow
{
    internal sealed class AskEmailPasswordStep
    {
        public static Credentials Execute(LoginAttempt loginAttempt = null)
        {
            string email = null;
            if (loginAttempt?.Email == null)
                email = Prompt.GetString("Email: ");
            else
                Console.WriteLine($"Email: {loginAttempt.Email}");

            var password = Prompt.GetPassword("Wachtwoord: ");
            
            Console.WriteLine();
            
            return new Credentials
            {
                Email = email,
                Password = password
            };
        }
    }
}