using System;
using System.Collections.Generic;
using System.Linq;


namespace VtcIts {

    public static class EnumExt {

        public static TAttribute GetAttribute<TAttribute>(this Enum value)
            where TAttribute : Attribute {
            
            var type = value.GetType();
            var name = Enum.GetName(type, value);

            if (name == null)
                return null;

            return type.GetField(name)
                .GetCustomAttributes(false)
                .OfType<TAttribute>()
                .SingleOrDefault();
        }


        
        public static string ToPrintText(this Enum value) {
            var printTextAttribute = value.GetAttribute<EnumPrintTextAttribute>();
            return (printTextAttribute != null && !String.IsNullOrEmpty(printTextAttribute.PrintText))
                       ? printTextAttribute.PrintText
                       : value.ToString();
        }



        public static int ToOrdinal(this Enum value) {
            var sortOrderAttribute = value.GetAttribute<EnumSortOrderAttribute>();
            return sortOrderAttribute != null
                       ? sortOrderAttribute.Ordinal
                       : Convert.ToInt32(value);
        }

    }



    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class EnumSortOrderAttribute : Attribute {

        public int Ordinal { get; private set; }


        public EnumSortOrderAttribute(int ordinal) {
            Ordinal = ordinal;
        }


        /// <summary>
        /// Will return a List of all the items in the enumeration type, sorted by 
        /// their respective Ordinal.  If no EnumSortOrderAttribute 
        /// was set, the integer value of the enumerated item will be used.
        /// </summary>
        /// <returns>Sorted list of every item in the enumeration</returns>
        public static List<T> GetOrderedEnumeration<T>() {
            var enumList = Enum.GetValues(typeof(T)).Cast<T>().ToList();
            var outputList = new List<KeyValuePair<T, int>>();

            if (enumList.Count > 0) {
                foreach (var enumItem in enumList) {
                    var attr = (enumItem as Enum).GetAttribute<EnumSortOrderAttribute>();
                    var ordinal = (attr == null) ? Convert.ToInt32(enumItem) : attr.Ordinal;
                    outputList.Add(new KeyValuePair<T, int>(enumItem, ordinal));
                }

                outputList.Sort((first, next) => first.Value.CompareTo(next.Value));
                return outputList.Select(item => item.Key).ToList();
            }

            return new List<T>();
        }

    }



    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class EnumPrintTextAttribute : Attribute {

        public string PrintText { get; private set; }



        public EnumPrintTextAttribute(string printText) {
            PrintText = printText;
        }


        /// <summary>
        /// Will return the print text associated with the enumerated item.  If 
        /// no EnumerationPrintTextAttribute was set, the text value of the 
        /// enumerated item will be used.
        /// </summary>
        /// <param name="value">Enumerated item to retrieve text for</param>
        /// <returns>Printable text for the enumeration</returns>
        public static string GetPrintText(Enum value) {
            return value.ToPrintText();
        }

    }

}
