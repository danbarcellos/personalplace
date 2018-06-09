using System;

namespace Pactor.Infra.Crosscutting.Log
{
    [Flags]
    public enum Appender
    {
        RollingFile = (1 << 1),
        Udp         = (1 << 2),
        AdoNet      = (1 << 3)
    }
}