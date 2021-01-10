using System.Collections;
using System.Collections.Generic;

namespace ComputerTheory
{
    public class InfiniteStack<T> : IEnumerable<T>
    {
        private readonly T empty;
        
        public InfiniteStack(T empty)
        {
            this.empty = empty;
        }
        
        private readonly Stack<T> internalStack = new();

        public void Push(in T c)
        {
            internalStack.Push(c);
        }

        public T Pop()
        {
            return internalStack.Count == 0 ? empty : internalStack.Pop();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return internalStack.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}