using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace OSancJoiner
{
    public class FileHandler
    {
        private static readonly string fileLocation = Environment.CurrentDirectory + "/settings.json";
        public static async Task SaveInfoToFile(OSancRequest data)
        {
            string text = JsonConvert.SerializeObject(data);
            await File.WriteAllTextAsync(fileLocation, text);
        }

        public static async Task<OSancRequest> LoadInfoFromFile()
        {
            if (File.Exists(fileLocation))
            {
                var lines = await File.ReadAllTextAsync(fileLocation);
                return JsonConvert.DeserializeObject<OSancRequest>(lines);
            }
            return null;

        }
    }
}
