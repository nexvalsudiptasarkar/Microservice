using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ObjectExtensions
{
    public enum TypeChecker
    {
        String,
        StringBuilder,
        AnyString,

        DateTime,
        DateTimeOffset,
        AnyDateTime,

        Byte,
        UInt,
        UShort,
        ULong,
        AnyUnsignedInt,

        SignedByte,
        Int,
        Short,
        Long,
        AnySignedInt,

        AnyInteger,

        Float,
        Double,
        AnyFloat,

        Decimal,
        Numeric,

        Specified,
        AnyType
    };

    public static class ExtendTypes
    {
        public static bool IsByte(this Type type)
        {
            return (type == typeof(byte));
        }

        public static bool IsUInt(this Type type)
        {
            return (type == typeof(uint));
        }

        public static bool IsUShort(this Type type)
        {
            return (type == typeof(ushort));
        }

        public static bool IsULong(this Type type)
        {
            return (type == typeof(ulong));
        }

        public static bool IsAnyUnsignedInteger(this Type type)
        {
            return (type.IsByte() ||
                    type.IsUInt() ||
                    type.IsUShort() ||
                    type.IsULong());
        }

        public static bool IsSignedByte(this Type type)
        {
            return (type == typeof(sbyte));
        }

        public static bool IsInt(this Type type)
        {
            return (type == typeof(int));
        }

        public static bool IsShort(this Type type)
        {
            return (type == typeof(short));
        }

        public static bool IsLong(this Type type)
        {
            return (type == typeof(long));
        }

        public static bool IsAnySignedInteger(this Type type)
        {
            return (type.IsSignedByte() ||
                    type.IsInt() ||
                    type.IsShort() ||
                    type.IsLong());
        }

        public static bool IsAnyInteger(this Type type)
        {
            return (type.IsAnyUnsignedInteger() || type.IsAnySignedInteger());
        }

        public static bool IsFloat(this Type type)
        {
            return (type == typeof(float));
        }
        public static bool IsDouble(this Type type)
        {
            return (type == typeof(double));
        }
        public static bool IsAnyFloatingPoint(this Type type)
        {
            return (type.IsFloat() || type.IsDouble());
        }

        public static bool IsInteger(this Type type)
        {
            return (type.IsAnyUnsignedInteger() || type.IsAnySignedInteger());
        }
        public static bool IsDecimal(this Type type)
        {
            return (type == typeof(decimal));
        }

        public static bool IsNumeric(this Type type)
        {
            bool result = (type.IsInteger() ||
                           type.IsAnyFloatingPoint() ||
                           type.IsDecimal());
            return result;
        }

        public static bool IsDateTime(this Type type)
        {
            bool result = (type == typeof(DateTime));
            return result;
        }
        public static bool IsDateTimeOffset(this Type type)
        {
            bool result = (type == typeof(DateTimeOffset));
            return result;
        }

        public static bool IsAnyDateTime(this Type type)
        {
            return (type.IsDateTime() || type.IsDateTimeOffset());
        }

        public static bool IsStringBuilder(this Type type)
        {
            bool result = (type == typeof(StringBuilder));
            return result;
        }
        public static bool IsString(this Type type)
        {
            bool result = (type == typeof(string));
            return result;
        }
        public static bool IsAnyString(this Type type)
        {
            bool result = (type.IsStringBuilder() || type.IsString());
            return result;
        }

        public static PropertyInfo GetPropertyInfo(this Type type, string propertyName, TypeChecker typeChecker = TypeChecker.AnyType, Type specifiedType = null)
        {
            PropertyInfo info = null;
            if (!string.IsNullOrEmpty(propertyName))
            {
                info = type.GetProperty(propertyName);
                Type iType = info.PropertyType;
                switch (typeChecker)
                {
                    case TypeChecker.String: info = (iType.IsString()) ? info : null; break;
                    case TypeChecker.StringBuilder: info = (iType.IsStringBuilder()) ? info : null; break;
                    case TypeChecker.AnyString: info = (iType.IsAnyString()) ? info : null; break;

                    case TypeChecker.DateTime: info = (iType.IsDateTime()) ? info : null; break;
                    case TypeChecker.DateTimeOffset: info = (iType.IsDateTimeOffset()) ? info : null; break;
                    case TypeChecker.AnyDateTime: info = (iType.IsAnyDateTime()) ? info : null; break;

                    case TypeChecker.Byte: info = (iType.IsByte()) ? info : null; break;
                    case TypeChecker.UInt: info = (iType.IsUInt()) ? info : null; break;
                    case TypeChecker.UShort: info = (iType.IsUShort()) ? info : null; break;
                    case TypeChecker.ULong: info = (iType.IsULong()) ? info : null; break;
                    case TypeChecker.AnyUnsignedInt: info = (iType.IsAnyUnsignedInteger()) ? info : null; break;

                    case TypeChecker.SignedByte: info = (iType.IsSignedByte()) ? info : null; break;
                    case TypeChecker.Int: info = (iType.IsInt()) ? info : null; break;
                    case TypeChecker.Short: info = (iType.IsShort()) ? info : null; break;
                    case TypeChecker.Long: info = (iType.IsLong()) ? info : null; break;
                    case TypeChecker.AnySignedInt: info = (iType.IsAnySignedInteger()) ? info : null; break;

                    case TypeChecker.AnyInteger: info = (iType.IsAnyInteger()) ? info : null; break;

                    case TypeChecker.Double: info = (iType.IsDouble()) ? info : null; break;
                    case TypeChecker.Float: info = (iType.IsFloat()) ? info : null; break;
                    case TypeChecker.AnyFloat: info = (iType.IsAnyFloatingPoint()) ? info : null; break;

                    case TypeChecker.Decimal: info = (iType.IsDecimal()) ? info : null; break;

                    case TypeChecker.Numeric: info = (iType.IsNumeric()) ? info : null; break;

                    case TypeChecker.Specified: info = (iType == specifiedType) ? info : null; break;
                    default: break;
                }
            }
            return info;
        }

        public static object GetDefaultValue(this Type type)
        {
            return (type.IsValueType) ? Activator.CreateInstance(type) : null;
        }

        public static bool IsEnumerableType(this Type type)
        {
            return (type.GetInterface("IEnumerable") != null);
        }
    }
}
