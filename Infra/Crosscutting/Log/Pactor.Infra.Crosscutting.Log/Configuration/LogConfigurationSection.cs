using System.Configuration;

namespace Pactor.Infra.Crosscutting.Log.Configuration
{
    public class LogConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("udpAppender", IsRequired = false)]
        public virtual UDPAppenderElement UDPAppender
        {
            get
            {
                return (UDPAppenderElement)this["udpAppender"];
            }
            set
            {
                this["udpAppender"] = value;
            }
        }

        [ConfigurationProperty("rollingFileAppender", IsRequired = false)]
        public virtual RollingFileAppenderElement RollingFileAppender
        {
            get
            {
                return (RollingFileAppenderElement)this["rollingFileAppender"];
            }
            set
            {
                this["rollingFileAppender"] = value;
            }
        }

        [ConfigurationProperty("adoNetAppenderElement", IsRequired = false)]
        public virtual AdoNetAppenderElement AdoNetAppenderElement
        {
            get
            {
                return (AdoNetAppenderElement)this["adoNetAppenderElement"];
            }
            set
            {
                this["adoNetAppenderElement"] = value;
            }
        }
    }
}
