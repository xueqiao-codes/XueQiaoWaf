using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XueQiaoFoundation.Shared.Model
{
    public class LimitedSizeStack<T>
    {
        private readonly LinkedList<T> linkedList = new LinkedList<T>();
        private readonly int _maxSize;
        public LimitedSizeStack(int maxSize)
        {
            _maxSize = maxSize;
        }

        public void Push(T item)
        {
            linkedList.AddFirst(item);
            if (linkedList.Count > _maxSize)
                linkedList.RemoveLast();
        }

        public T Pop()
        {
            var item = linkedList.First.Value;
            linkedList.RemoveFirst();
            return item;
        }

        public T Peek()
        {
            var item = linkedList.First.Value;
            return item;
        }
    }
}
