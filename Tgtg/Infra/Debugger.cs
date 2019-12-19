using System.Threading.Tasks;
using Colorful;
using Hazebroek.Tgtg.Auth;

namespace Hazebroek.Tgtg.Infra
{
    internal sealed class Debugger
    {
        private readonly UsersContextRepository _usersContextRepo;

        public Debugger(
            UsersContextRepository usersContextRepo
        )
        {
            this._usersContextRepo = usersContextRepo;
        }

        public void Execute()
        {
            Console.WriteLine($"User context locatie: {_usersContextRepo.JsonPath.FullName}");
        }
    }
}