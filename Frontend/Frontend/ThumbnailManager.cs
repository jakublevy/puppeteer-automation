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
                    int maxId = thumbnails.Max(x => x.Id);
                    nextAvailableId = maxId + 1;
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
                File.Create(Constants.RECORDING_LIST_FILE);
                nextAvailableId = 1;

                return new List<Thumbnail>();
            }
        }

        public static Thumbnail NewThumbnail()
        {
            return new Thumbnail
                {Created = DateTime.Now, Id = nextAvailableId, Name = "Untitled", Websites = new List<Uri>()};
        }
    }
}
