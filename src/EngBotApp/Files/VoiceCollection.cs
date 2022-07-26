using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngBotApp.Files
{
    public static class VoiceCollection
    {
        public static Stream GetStream(long id)
        {
            var path = Path.Combine(Environment.CurrentDirectory, $"Resources//Sounds//{id}.wav");

            if (File.Exists(path))
            {
                return File.OpenRead(path);
            }

            return null;
        }

        public static TimeSpan GetSoundDuration(long id)
        {
            var file = TagLib.File.Create(Path.Combine(Environment.CurrentDirectory, $"Resources//Sounds//{id}.wav"));

            return file.Properties.Duration;
        }
    }
}
