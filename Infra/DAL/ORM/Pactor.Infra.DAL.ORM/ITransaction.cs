using System;
using System.Data;

namespace Pactor.Infra.DAL.ORM
{
    public interface ITransaction : IDisposable
    {
        bool IsActive { get; }
        bool WasCommitted { get; }
        bool WasRolledBack { get; }
        void Begin();
        void Begin(IsolationLevel isolationLevel);
        void Commit();
        void Rollback();
    }
}