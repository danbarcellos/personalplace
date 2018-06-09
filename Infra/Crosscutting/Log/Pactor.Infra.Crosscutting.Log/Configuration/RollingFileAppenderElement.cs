using System.Configuration;

namespace Pactor.Infra.Crosscutting.Log.Configuration
{
    public class RollingFileAppenderElement : AppenderElement
    {
        [ConfigurationProperty("logFolder", IsRequired = false)]
        public virtual string LogFolder
        {
            get
            {
                return (string)this["logFolder"];
            }
            set
            {
                this["logFolder"] = value;
            }
        }

        [ConfigurationProperty("logFileName", DefaultValue = "Alaris.log", IsRequired = false)]
        public virtual string LogFileName
        {
            get
            {
                return (string)this["logFileName"];
            }
            set
            {
                this["logFileName"] = value;
            }
        }
    }
}