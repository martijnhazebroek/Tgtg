using Colorful;
using Hazebroek.Tgtg.Auth;

namespace Hazebroek.Tgtg.Infra
{
    internal sealed class PrintDebugStep
    {
        private readonly UsersContextRepository _usersContextRepo;

        public PrintDebugStep(
            UsersContextRepository usersContextRepo
        )
        {
            _usersContextRepo = usersContextRepo;
        }

        public void Execute()
        {
            Console.WriteLine($"User context locatie: {_usersContextRepo.JsonPath.FullName}");
        }
    }
}