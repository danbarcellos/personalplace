using System.Configuration;

namespace Pactor.Infra.Crosscutting.Log.Configuration
{
    public class AppenderElement : ConfigurationElement
    {
#if DEBUG
        [ConfigurationProperty("level", DefaultValue = LogLevel.Debug, IsRequired = false)]
#else
        [ConfigurationProperty("level", DefaultValue = LogLevel.Warning, IsRequired = false)]
#endif
        public virtual LogLevel Level
        {
            get
            {
                return (LogLevel)this["level"];
            }
            set
            {
                this["level"] = value;
            }
        }
    }
}