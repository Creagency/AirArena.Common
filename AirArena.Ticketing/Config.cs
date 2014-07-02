using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
namespace AirArena.Ticketing
{
    public class Config : ConfigurationSection
    {
        [ConfigurationProperty("DataConPrefix", DefaultValue = "Ticketing", IsRequired = false)]
        public string DataConPrefix 
        {
            get { return this["DataConPrefix"].ToString(); }
        }

        public static Config CurrentConfig
        {
            get
            {
                Config rv = (Config)System.Configuration.ConfigurationManager.GetSection("AirArena.Ticketing");
                if (rv == null)
                    rv = new Config();
                return rv;
            }
        }
    }
}