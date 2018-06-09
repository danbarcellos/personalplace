using System.Configuration;

namespace Pactor.Infra.Crosscutting.Log.Configuration
{
    public class AdoNetAppenderElement : AppenderElement
    {
        [ConfigurationProperty("connectionString", 
                               DefaultValue = "Data Source=(local);Initial Catalog=AlarisLog;Integrated Security=True", 
                               IsRequired = false)]
        public virtual string ConnectionString
        {
            get
            {
                return (string)this["connectionString"];
            }
            set
            {
                this["connectionString"] = value;
            }
        }
    }
}