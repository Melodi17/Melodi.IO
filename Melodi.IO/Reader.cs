using System;
using System.Collections.Generic;
using System.Linq;

namespace Melodi.IO
{
    public class Reader<T>
    {
        private List<T> Items = new();
        public bool AtEnd => Items.Count == 0;
        public Reader(T[] items)
        {
            Items = items.ToList();
        }
        public T Read()
        {
            T ln = Read();
            Items.RemoveAt(0);
            return ln;
        }
        public T Peek()
        {
            if (!AtEnd)
            {
                return Items[0];
            }
            else
            {
                throw new IndexOutOfRangeException("There are no lines left to read");
            }
        }
        public override bool Equals(object obj)
        {
            if (obj is not Reader<T>) return false;
            return ((Reader<T>)obj).Peek().Equals(Peek());
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return string.Join('\n', Items);
        }
    }
}