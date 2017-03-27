using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class ObjectCollection<T>: ICollection<T>
    {
        List<T> innerList;
        public ObjectCollection() {
            innerList = new List<T>();
        }

        public void Add(T item)
        {
            innerList.Add(item);
        }

        public void Clear()
        {
            innerList.Clear();
        }

        public bool Contains(T item)
        {
            return innerList.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get 
            {
                return innerList.Count;
            }
        }

        public virtual bool IsReadOnly
        {
            get 
            {
                return false;
            }
        }

        public bool Remove(T item)
        {
            return innerList.Remove(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return innerList.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
