using System.IO;
using Newtonsoft.Json;

namespace Frontend
{
    /// <summary>
    /// This class contains various methods for saving, retrieving, and obtaining default configuration.
    /// </summary>
    class ConfigManager
    {
        public static JsonSerializerSettings JsonSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            NullValueHandling = NullValueHandling.Ignore,
            Formatting = Formatting.Indented
        };

        public static void SavePuppeteerConfiguration(PuppeteerOptions po)
        {
            Configuration dc = null;
            if (File.Exists(Constants.CONF_FILE))
            {
                string conf = File.ReadAllText(Constants.CONF_FILE);
                dc = JsonConvert.DeserializeObject<Configuration>(conf, JsonSettings);
                dc.PuppeteerConfig = po;
                File.WriteAllText(Constants.CONF_FILE, JsonConvert.SerializeObject(dc, JsonSettings));
            }
            else
            {
                dc = new Configuration {PuppeteerConfig = po};
                File.WriteAllText(Constants.CONF_FILE, JsonConvert.SerializeObject(dc, JsonSettings));
            }
        }

        public static void SaveRecordedEventsConfiguration(RecordedEvents po)
        {
            Configuration dc = null;
            if (File.Exists(Constants.CONF_FILE))
            {
                string conf = File.ReadAllText(Constants.CONF_FILE);
                dc = JsonConvert.DeserializeObject<Configuration>(conf, JsonSettings);
                dc.RecordedEvents = po;
                File.WriteAllText(Constants.CONF_FILE, JsonConvert.SerializeObject(dc, JsonSettings));
            }
            else
            {
                dc = new Configuration { RecordedEvents = po };
                File.WriteAllText(Constants.CONF_FILE, JsonConvert.SerializeObject(dc, JsonSettings));
            }
        }

        public static RecordedEvents GetRecordedEventsOptions()
        {
            if (File.Exists(Constants.CONF_FILE))
            {
                Configuration dc =
                    JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(Constants.CONF_FILE), JsonSettings);
                if (dc.RecordedEvents != null)
                    return dc.RecordedEvents;
                return new RecordedEvents();

            }
            else
            {
                return new RecordedEvents();
            }
        }


        public static CodeGeneratorOptions GetCodeGeneratorOptions()
        {
            if (File.Exists(Constants.CONF_FILE))
            {
                Configuration dc =
                    JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(Constants.CONF_FILE), JsonSettings);
                if (dc.CodeGeneratorConfig != null)
                    return dc.CodeGeneratorConfig;
                return DefaultCodeGeneratorOptions();

            }
            else
            {
                return new CodeGeneratorOptions();
            }
        }

        public static void SaveCodeGeneratorOptions(CodeGeneratorOptions cgo)
        {
            Configuration dc = null;
            if (File.Exists(Constants.CONF_FILE))
            {
                string conf = File.ReadAllText(Constants.CONF_FILE);
                dc = JsonConvert.DeserializeObject<Configuration>(conf, JsonSettings);
                dc.CodeGeneratorConfig = cgo;
                File.WriteAllText(Constants.CONF_FILE, JsonConvert.SerializeObject(dc, JsonSettings));
            }
            else
            {
                dc = new Configuration { CodeGeneratorConfig = cgo };
                File.WriteAllText(Constants.CONF_FILE, JsonConvert.SerializeObject(dc, JsonSettings));
            }
        }

        public static PuppeteerOptions GetPuppeteerConfiguration()
        {
            if (File.Exists(Constants.CONF_FILE))
            {
                Configuration dc =
                    JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(Constants.CONF_FILE), JsonSettings);
                if(dc.PuppeteerConfig != null)
                    return dc.PuppeteerConfig;
                return DefaultPuppeteerOptions();
            }

            return DefaultPuppeteerOptions();
        }

        public static void SavePlayerOptions(PlayerOptions po)
        {
            Configuration dc = null;
            if (File.Exists(Constants.CONF_FILE))
            {
                string conf = File.ReadAllText(Constants.CONF_FILE);
                dc = JsonConvert.DeserializeObject<Configuration>(conf, JsonSettings);
                dc.PlayerOptions = po;
                File.WriteAllText(Constants.CONF_FILE, JsonConvert.SerializeObject(dc, JsonSettings));
            }
            else
            {
                dc = new Configuration { PlayerOptions = po };
                File.WriteAllText(Constants.CONF_FILE, JsonConvert.SerializeObject(dc, JsonSettings));
            }
        }

        public static PlayerOptions GetPlayerOptions()
        {
            if (File.Exists(Constants.CONF_FILE))
            {
                Configuration dc =
                    JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(Constants.CONF_FILE), JsonSettings);
                if (dc.PlayerOptions != null)
                    return dc.PlayerOptions;
                return DefaultPlayerOptions();
            }

            return DefaultPlayerOptions();
        }

        private static CodeGeneratorOptions DefaultCodeGeneratorOptions()
        {
            return new CodeGeneratorOptions();
        }

        private static PuppeteerOptions DefaultPuppeteerOptions()
        {
            return new LaunchPuppeteerOptions
            {
                DevTools = false,
                ExecutablePath = "C:\\Program Files (x86)\\Google\\Chrome\\Application\\chrome.exe",
                SlowMo = 100,
                Viewport = new Viewport
                {
                    DeviceScaleFactor = 1.0,
                    HasTouch = false,
                    Height = 1080,
                    Width = 1920,
                    IsLandscape = false,
                    IsMobile = false
                }
            };
        }

        private static PlayerOptions DefaultPlayerOptions()
        {
            return new PlayerOptions();
        }


        public static void SaveNodeJsOptions(NodeJsOptions no)
        {
            Configuration dc = null;
            if (File.Exists(Constants.CONF_FILE))
            {
                string conf = File.ReadAllText(Constants.CONF_FILE);
                dc = JsonConvert.DeserializeObject<Configuration>(conf, JsonSettings);
                dc.NodeJsOptions = no;
                File.WriteAllText(Constants.CONF_FILE, JsonConvert.SerializeObject(dc, JsonSettings));
            }
            else
            {
                dc = new Configuration { NodeJsOptions = no };
                File.WriteAllText(Constants.CONF_FILE, JsonConvert.SerializeObject(dc, JsonSettings));
            }
        }

        public static NodeJsOptions GetNodeJsOptions()
        {
            if (File.Exists(Constants.CONF_FILE))
            {
                Configuration dc =
                    JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(Constants.CONF_FILE), JsonSettings);
                if (dc.NodeJsOptions != null)
                    return dc.NodeJsOptions;
                return DefaultNodeJsOptions();
            }

            return DefaultNodeJsOptions();
        }


        private static NodeJsOptions DefaultNodeJsOptions()
        {
            return new NodeJsOptions();
        }
    }

}
