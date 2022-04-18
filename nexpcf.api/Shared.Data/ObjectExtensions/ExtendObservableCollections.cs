using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectExtensions
{
	public static class ExtendObservableCollections
	{
		public static void AddRange<T>(this ObservableCollection<T> collection, IEnumerable<T> list)
		{
			foreach(T item in list)
			{
				collection.Add(item);
			}
		}
	}
}
