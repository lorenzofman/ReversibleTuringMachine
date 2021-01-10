using System.Collections.Generic;
using System.Linq;

namespace ComputerTheory
{
    public static class EnumeratorExtensions
    {
        public static IEnumerator<T> Read<T>(this IEnumerator<T> enumerator, out T value)
        {
            enumerator.MoveNext();
            value = enumerator.Current;
            return enumerator;
        }
        
        public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> source)
        {
            return source.Select((item, index) => (item, index));
        }
    }
}