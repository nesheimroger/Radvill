using System;
using Radvill.Services.DataFactory.Repositories;

namespace Radvill.Services.DataFactory
{
    public interface IDataFactory : IDisposable
    {
        IUserRepository UserRepository { get; set; }

        void Commit();
    }
}