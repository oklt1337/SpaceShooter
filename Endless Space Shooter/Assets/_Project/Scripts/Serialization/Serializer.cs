using System;
using System.IO;
using _Project.Scripts.Encryption;
using Newtonsoft.Json;

namespace _Project.Scripts.Serialization
{
    public static class Serializer
    {
        public static void Serialize(Scoreboard.Scoreboard data)
        {
            var path = Environment.SpecialFolder.LocalApplicationData + "/SpaceShooter";

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            
            var filePath = path + "/Scoreboard.json";
            
            var json = JsonConvert.SerializeObject(data);
            var encrypted = Encrypter.Encrypt(json);
            
            if (!File.Exists(filePath))
                File.WriteAllText(filePath, encrypted);
            else
            {
                File.Delete(filePath);
                File.WriteAllText(filePath, encrypted);
            }
        }
    }
}
