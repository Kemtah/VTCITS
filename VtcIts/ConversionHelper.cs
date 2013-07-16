using System;


namespace VtcIts {

    public static class ConversionHelper {

        public static T SafeConvert<T>(this object target, T defaultValue = default(T)) {
            if (target == null || target == DBNull.Value) {
                return defaultValue;
            }

            var type = typeof (T);
            
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof (Nullable<>)) {
                var underlyingType = Nullable.GetUnderlyingType(type);
                return InternalConvert(target, underlyingType, defaultValue);
            }

            return InternalConvert(target, type, defaultValue);
        }



        private static T InternalConvert<T>(object target, Type type, T defaultValue = default(T)) {
            // TL;DR: execution time suffers without this check, because we usually want an int or an enum 
            // but Oracle always returns numbers as decimal or long
            if ((type != typeof (decimal) && target is decimal) || (type != typeof (long) && target is long)) {
                if (!type.IsEnum) {
                    try {
                        // Need to explicitly convert since we can't cast decimal or long to much else
                        return (T) Convert.ChangeType(target, type);
                    }
                    catch {
                        return defaultValue;
                    }
                }
            }
            
            try {
                if (!type.IsEnum) {
                    // Just cast it and see what happens
                    return (T) target;
                }

                // For enums, we usually start off with its integer value, so we need an extra conversion first
                var intValue = Convert.ChangeType(target, typeof (int));
                return intValue != null && Enum.IsDefined(type, intValue)
                           ? (T) Enum.ToObject(type, intValue)
                           : defaultValue;
            }
            catch {
                // No implicit conversion exists. Let's intelligently try to parse out what we're working with here.
                try {
                    // Type is a standard conversion
                    return (T) Convert.ChangeType(target, type);
                }
                catch {
                    // No explicit conversion exists, either, so we're stuck with the default value.
                    return defaultValue;
                }
            }
        }

    }
}
