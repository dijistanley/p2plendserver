using System;
using System.Threading.Tasks;

namespace DataAccess.Interfaces
{
    public interface IRepository<T> : IDisposable
    {
        Task<bool> Create(T t);
        Task<T> Find(string id);
        Task<bool> Update(T t);
        Task<bool> Delete(T t);
    }
}
