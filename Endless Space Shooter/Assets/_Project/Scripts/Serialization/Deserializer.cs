using System;
using System.IO;
using _Project.Scripts.Encryption;
using Newtonsoft.Json;

namespace _Project.Scripts.Serialization
{
    public static class Deserializer
    {
        public static Scoreboard.Scoreboard Deserialize()
        {
            var filePath = Environment.SpecialFolder.LocalApplicationData + "/SpaceShooter/Scoreboard.json";
            
            if (!File.Exists(filePath))
                return new Scoreboard.Scoreboard();
            
            var json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<Scoreboard.Scoreboard>(Decrypter.Decrypt(json));
        }
    }
}
