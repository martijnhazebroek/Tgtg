using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Hazebroek.Tgtg.Auth
{
    internal sealed class UsersContextRepository
    {
        private static readonly DirectoryInfo TgtgRoot =
            new DirectoryInfo($"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}/tgtg");

        private readonly UserContextRepository _userContextRepo;
        private bool _restored;

        private UsersContext _usersContext;

        public UsersContextRepository(
            UserContextRepository userContextRepo
        )
        {
            _usersContext = new UsersContext();
            _userContextRepo = userContextRepo;
            JsonPath = new FileInfo(
                $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}/tgtg/tgtg.json");
        }

        public FileInfo JsonPath { get; }

        public void AddUser(in int userId)
        {
            AssureRestored();
            _usersContext!.UserIds.Add(userId);
            Persist();
        }

        public IEnumerable<UserContext> FetchUsers()
        {
            AssureRestored();
            return _usersContext.UserIds
                .ToList()
                .Select(uid =>
                {
                    _userContextRepo.TryRestore(uid, out var userContext);
                    return userContext;
                });
        }

        public void Remove(long id)
        {
            AssureRestored();
            _usersContext!.UserIds.Remove(id);
            UserContextRepository.Remove(id);
            Persist();
        }

        private void AssureRestored()
        {
            if (_restored) return;
            TryRestore(out _usersContext);
            _restored = true;
        }

        private void Persist()
        {
            if (!TgtgRoot.Exists) TgtgRoot.Create();
            using var file = File.CreateText(JsonPath.FullName);
            var serializer = new JsonSerializer();
            serializer.Serialize(file, _usersContext);
        }

        public bool TryRestore(out UsersContext usersContext)
        {
            usersContext = new UsersContext();
            if (!JsonPath.Exists)
            {
                return false;
            }

            using var file = File.OpenText(JsonPath.FullName);
            using var reader = new JsonTextReader(file);
            var serializer = new JsonSerializer();

            var restored = serializer.Deserialize<UsersContext>(reader);
            if (restored == null) return false;

            usersContext = restored;

            return true;
        }
    }
}