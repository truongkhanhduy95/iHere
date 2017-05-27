using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections.Specialized;

namespace Xamarin.Core.Utils
{
    public class ObservableCollectionEx<T> : ObservableCollection<T>
    {

        public ObservableCollectionEx()
            : base()
        {
        }

        public ObservableCollectionEx(IEnumerable<T> collection)
            : base(collection)
        {
        }

        public ObservableCollectionEx(List<T> list)
            : base(list)
        {
        }

        public void AddRange(IEnumerable<T> range)
        {
            foreach (var item in range)
            {
                // Use inner items
                Items.Add(item);
            }
            // Notify: Raise event if need
            this.OnPropertyChanged(new PropertyChangedEventArgs("Count"));
            this.OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <summary>
        /// Reset then add range
        /// </summary>
        /// <param name="range">Range.</param>
        public void Reset(IEnumerable<T> range)
        {
            this.Items.Clear();
            if (range != null)
            {
                AddRange(range);
            }
        }
    }
}

