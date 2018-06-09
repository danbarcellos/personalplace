using System;
using System.Collections.Generic;

namespace Pactor.Infra.Crosscutting.IoC
{
    public interface IContainer : IDisposable
    {
        object Resolve(Type type, params Parameter[] parameters);
        T Resolve<T>(params Parameter[] parameters);
        TService ResolveKeyed<TService>(object serviceKey);
        TService ResolveKeyed<TService>(object serviceKey, params Parameter[] parameters);
        T ResolveNamed<T>(string name);
        object ResolveNamed(string name, Type serviceType);
        IEnumerable<T> ResolveAll<T>();
        IEnumerable<object> ResolveAll(Type type);
        Meta<object> ResolveMeta(Type type, params Parameter[] parameters);
        Meta<T> ResolveMeta<T>(params Parameter[] parameters);
        Meta<object> ResolveNamedMeta(string name, Type serviceType);
        Meta<T> ResolveNamedMeta<T>(string name);
        IEnumerable<Meta<object>> ResolveAllMeta(Type type);
        IEnumerable<Meta<T>> ResolveAllMeta<T>();
        bool IsRegistered<T>();
        bool IsRegistered(Type type);
        bool TryResolve<T>(out T instance);
        bool TryResolve(Type serviceType, out object instance);
        bool TryResolveNamed(string serviceName, Type serviceType, out object instance);
        bool TryResolveNamed<T>(string serviceName, out object instance);
        T InjectProperties<T>(T instance);
        T InjectUnsetProperties<T>(T instance);
        IContainer BeginLifetimeScope();
        IContainer BeginLifetimeScope(object tag);
    }
}
