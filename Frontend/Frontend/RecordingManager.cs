using System.IO;
using Newtonsoft.Json;

namespace Frontend
{
    /// <summary>
    /// This file contains a method that saves changes in the currently edited recording.
    /// </summary>
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
