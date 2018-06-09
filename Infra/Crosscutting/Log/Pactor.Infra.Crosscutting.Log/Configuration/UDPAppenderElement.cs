using System.Configuration;

namespace Pactor.Infra.Crosscutting.Log.Configuration
{
    public class UDPAppenderElement : AppenderElement
    {
        [ConfigurationProperty("address", DefaultValue = "127.0.0.1", IsRequired = false)]
        public virtual string Address
        {
            get
            {
                return (string)this["address"];
            }
            set
            {
                this["address"] = value;
            }
        }

        [ConfigurationProperty("port", DefaultValue = 7071, IsRequired = false)]
        public virtual int Port
        {
            get
            {
                return (int)this["port"];
            }
            set
            {
                this["port"] = value;
            }
        }
    }
}