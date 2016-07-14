using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;

namespace AcmeCorp.Engagements.FarmConfiguration
{
    public class SortedProperties
    {
        private string _key;
        private string _value;

        public string Key
        {
            get { return _key; }
            set { _key = value; }
        }

        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public SortedProperties(string key, string value)
        {
            _key = key;
            _value = value;
        }

    }

    public sealed class GenericComparer<T> : IComparer<T>
    {

        public enum SortOrder { Ascending, Descending };
        private string sortColumn;
        private SortOrder sortingOrder;

        public GenericComparer(string sortColumn, SortOrder sortingOrder)
        {
            this.sortColumn = sortColumn;
            this.sortingOrder = sortingOrder;
        }

        public string SortColumn
        {
            get { return sortColumn; }
        }

        public SortOrder SortingOrder
        {
            get { return sortingOrder; }
        }


        public int Compare(T x, T y)
        {

            PropertyInfo propertyInfo = typeof(T).GetProperty(sortColumn);
            IComparable obj1 = (IComparable)propertyInfo.GetValue(x, null);
            IComparable obj2 = (IComparable)propertyInfo.GetValue(y, null);
            if (sortingOrder == SortOrder.Ascending)
            {
                return (obj1.CompareTo(obj2));
            }
            else
            {
                return (obj2.CompareTo(obj1));
            }
        }

    }
}