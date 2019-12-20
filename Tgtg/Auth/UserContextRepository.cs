using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Hazebroek.Tgtg.Auth
{
    internal sealed class UserContextRepository
    {
        public UserContextRepository(UserContext inMemoryValue)
        {
            CurrentContext = inMemoryValue;
        }

        public UserContext CurrentContext { get; private set; }

        public static void Remove(in long id)
        {
            PathForContextFile(id).Delete();
        }

        public Task Persist()
        {
            if (!CurrentContext.UserId.HasValue) throw new InvalidOperationException("UserId can not be null");
            using var file = File.CreateText(PathForContextFile(CurrentContext.UserId.Value).FullName);
            var serializer = new JsonSerializer();
            serializer.Serialize(file, CurrentContext);
            return Task.CompletedTask;
        }

        public bool TryRestore(long userId, out UserContext userContext)
        {
            userContext = new UserContext();

            var pathForConfig = PathForContextFile(userId);
            if (!pathForConfig.Exists) return false;

            using var file = File.OpenText(pathForConfig.FullName);
            using var reader = new JsonTextReader(file);
            var serializer = new JsonSerializer();

            var restored = serializer.Deserialize<UserContext>(reader);
            if (restored == null) return false;

            userContext = restored;
            CurrentContext = restored;

            return true;
        }

        private static FileInfo PathForContextFile(long userId)
        {
            return new FileInfo(
                $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}/tgtg/tgtg_{userId}.json");
        }
    }
}