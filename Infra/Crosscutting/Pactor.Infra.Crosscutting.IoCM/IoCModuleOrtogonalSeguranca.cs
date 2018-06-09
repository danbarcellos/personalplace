using Autofac;
using Pactor.Infra.Crosscutting.Security;

namespace Pactor.Infra.Crosscutting.IoCM
{
    public class IoCModuleOrtogonalSeguranca : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new EncryptionServiceRijndael())
                   .As<IEncryptionService>()
                   .SingleInstance()
                   .OnRelease(cr => cr.Dispose());

            builder.Register(c => new ShuffleService())
                   .As<IShuffleService>()
                   .SingleInstance()
                   .OnRelease(cr => cr.Dispose());
        } 
    }
}