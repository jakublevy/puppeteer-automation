using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Frontend
{
    /// <summary>
    /// This file contains logic and helper methods for working with thumbnails (recording previews, shown on the application main page).
    /// </summary>
    class ThumbnailManager
    {
        private static int nextAvailableId;

        /// <summary>
        /// Called with the start of the Frontend application. This method loads saved recordings if there exists a file with them.
        /// If there is not file, it creates an empty collection of recordings.
        /// </summary>
        /// <returns></returns>
        public static List<Thumbnail> Init()
        {
            if (File.Exists(Constants.RECORDING_LIST_FILE))
            {
                string json = File.ReadAllText(Constants.RECORDING_LIST_FILE);
                List<Thumbnail> thumbnails = JsonConvert.DeserializeObject<List<Thumbnail>>(json, ConfigManager.JsonSettings);
                if (thumbnails != null)
                {
                    if (thumbnails.Count == 0)
                    {
                        nextAvailableId = 1;
                    }
                    else
                    {
                        int maxId = thumbnails.Max(x => x.Id);
                        nextAvailableId = maxId + 1;
                    }
                }
                else
                {
                    nextAvailableId = 1;
                    return new List<Thumbnail>();
                }

                return thumbnails;
            }
            else
            {
                nextAvailableId = 1;

                return new List<Thumbnail>();
            }
        }

        public static List<Thumbnail> LoadThumbnails()
        {
            if (File.Exists(Constants.RECORDING_LIST_FILE))
            {
                string json = File.ReadAllText(Constants.RECORDING_LIST_FILE);
                List<Thumbnail> thumbnails = JsonConvert.DeserializeObject<List<Thumbnail>>(json, ConfigManager.JsonSettings);
                return thumbnails;
            }
            return new List<Thumbnail>();
        }

        public static Thumbnail NewThumbnail()
        {
            return new Thumbnail
                {Created = DateTime.Now, Id = nextAvailableId++, Name = "Untitled", Websites = new HashSet<string>()};
            
        }

        public static void SaveThumbnail(Thumbnail t)
        {
            List<Thumbnail> thumbnails = new List<Thumbnail>();
            if (File.Exists(Constants.RECORDING_LIST_FILE))
            {
                thumbnails =
                    JsonConvert.DeserializeObject<List<Thumbnail>>(File.ReadAllText(Constants.RECORDING_LIST_FILE),
                        ConfigManager.JsonSettings);
            }

            int idx = thumbnails.FindIndex(x => x.Id == t.Id);
            if (idx == -1)
                thumbnails.Add(t);
            else
                thumbnails[idx] = t;
            
            File.WriteAllText(Constants.RECORDING_LIST_FILE, JsonConvert.SerializeObject(thumbnails, ConfigManager.JsonSettings));
        }

        public static void RemoveThumbnail(Thumbnail t)
        {
            List<Thumbnail> thumbnails = LoadThumbnails();
            int idx = thumbnails.FindIndex(x => x.Id == t.Id);
            if (idx != -1)
            {
                thumbnails.RemoveAt(idx);
                File.WriteAllText(Constants.RECORDING_LIST_FILE, JsonConvert.SerializeObject(thumbnails, ConfigManager.JsonSettings));
            }
        }
    }
}
