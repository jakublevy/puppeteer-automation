using System.IO;
using Newtonsoft.Json;

namespace Frontend
{
    class RecordingManager
    {
        public static void SaveCurrentEdit(CurrentEdit edit)
        {
            ThumbnailManager.SaveThumbnail(edit.Thumbnail);

            if (!Directory.Exists("recordings"))
                Directory.CreateDirectory("recordings");

            dynamic recording = new {StartupHints = edit.StartupHints, Recordings = edit.Recordings, NextId = edit.NextAvailableId};

            string json = JsonConvert.SerializeObject(recording, ConfigManager.JsonSettings);
            File.WriteAllText($"recordings/{edit.Thumbnail.Id}.json", json);
        }
    }
}
