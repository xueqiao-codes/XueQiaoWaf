using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XueQiaoFoundation.Shared.Helper
{
    public static class CollectionExtensions
    {
        public static ObservableCollection<T> AddRange<T>(this ObservableCollection<T> observableCollection, IEnumerable<T> list)
        {
            if (list?.Any() == true)
            {
                foreach (var i in list)
                {
                    observableCollection.Add(i);
                }
            }
            return observableCollection;
        }

        public static ObservableCollection<T> RemoveAll<T>(this ObservableCollection<T> observableCollection, Predicate<T> predicate)
        {
            if (predicate == null) throw new ArgumentNullException("predicate");
            var copCol = observableCollection.ToArray();
            foreach (var i in copCol)
            {
                if (predicate.Invoke(i))
                {
                    observableCollection.Remove(i);
                }
            }
            return observableCollection;
        }
    }
}
