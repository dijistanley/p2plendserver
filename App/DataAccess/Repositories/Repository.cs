using DataAccess.Core;
using DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public abstract class Repository<T>: IRepository<T>
    {
        protected const string CREATE = "CREATE";
        protected const string UPDATE = "UPDATE";
        protected const string DELETE = "DELETE";

        protected DatabaseContext DBContext { get; private set; }
        
        public Repository()
        {
            DBContext = new DatabaseContext();
        }

        public abstract Task<bool> Create(T t);
        public abstract Task<T> Find(string id);
        public abstract Task<bool> Update(T t);
        public abstract Task<bool> Delete(T t);

        public virtual void Dispose()
        {
            DBContext.Dispose();
        }
    }
}
