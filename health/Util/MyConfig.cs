using System.IO;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Hosting;
using System;

namespace health.Util
{
    [Serializable]
    public class MyConfig
    {        
        public string AdminEmail { get; set; }

        public string SmtpServer { get; set; }

        public int SmtpPort { get; set; }

        public string AdminEmailPassword { get; set; }

        public string WorkEmail { get; set; }

        public string FooterCode { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }
        public string SocialCode { get; set; }
        public MyConfig() { }
    }

    public interface IMyConfig
    {
        MyConfig Config { get; }
    }
    public class MyConfigRepository:IMyConfig
    {
        private readonly IWebHostEnvironment _env;
        string _file;
        MyConfig _conf;
        XmlSerializer _formatter = new XmlSerializer(typeof(MyConfig));
        public MyConfigRepository(IWebHostEnvironment env)
        {
            _env = env;
            _file = env.ContentRootPath + "/AppData/myconfig.xml";
        }

        MyConfig IMyConfig.Config { get => GetMyConfig();}

        public MyConfig GetMyConfig()
        {
            if (File.Exists(_file))
            {
                try
                {
                    using (FileStream fs = new FileStream(_file, FileMode.OpenOrCreate))
                    {
                        _conf = (MyConfig)_formatter.Deserialize(fs);
                    }
                }
                catch
                {
                    _conf = new MyConfig();
                }
            }
            else
            {
                _conf = new MyConfig();
            }
            return _conf;
        }

        public void Save(MyConfig config)
        {
            using (FileStream fs = new FileStream(_file, FileMode.Create))
            {
                _formatter.Serialize(fs, config);
            }
        }
    }
}
