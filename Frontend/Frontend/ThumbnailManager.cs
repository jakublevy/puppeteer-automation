using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Frontend
{
    class ThumbnailManager
    {
        private static int nextAvailableId;
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
            //    File.Create(Constants.RECORDING_LIST_FILE);
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
