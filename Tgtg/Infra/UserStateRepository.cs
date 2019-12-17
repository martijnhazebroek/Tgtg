using System;
using System.IO;
using System.Threading.Tasks;
using Hazebroek.Tgtg.Auth;
using Newtonsoft.Json;

namespace Hazebroek.Tgtg.Infra
{
    public sealed class UserStateRepository
    {
        private readonly FileInfo _path;
        private readonly LoginContext _loginContext;

        public UserStateRepository(LoginContext loginContext)
        {
            _path = new FileInfo(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/tgtg.json");
            _loginContext = loginContext;
        }

        public Task SafeLoginContext()
        {
            using StreamWriter file = File.CreateText(_path.FullName);
            JsonSerializer serializer = new JsonSerializer();
            serializer.Serialize(file, _loginContext);  
            return Task.CompletedTask;
        }

        public bool TryRestoreLoginContext()
        {
            if (!_path.Exists)
            {
                return false;
            }
            
            using StreamReader file = File.OpenText(_path.FullName);
            using JsonTextReader reader = new JsonTextReader(file);
            JsonSerializer serializer = new JsonSerializer();
            
            var loginContext = serializer.Deserialize<LoginContext>(reader);
            if (loginContext == null)
            {
                return false;
            }
            _loginContext.UserId = loginContext.UserId;
            _loginContext.AccessToken = loginContext.AccessToken;
            _loginContext.RefreshToken = loginContext.RefreshToken;
            _loginContext.IftttTokens = loginContext.IftttTokens;

            return true;
        }
    }
}