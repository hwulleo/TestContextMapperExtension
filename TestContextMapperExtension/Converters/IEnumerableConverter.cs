using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestContextMapperExtension.Converters
{
    internal class IEnumerableConverter
    {
        public static object ConvertToIEnumerable(Type type, object value)
        {
            if (value is null)
                return null;
            
            string valueStr = value.ToString();
            ValidateInputString(valueStr);

            var tType = type.GetGenericArguments().First();
            valueStr = valueStr.Trim().Trim('[').Trim(']');
            var valueList = valueStr.Split(',');

            dynamic instantiatedIEnumerable = Activator.CreateInstance(type);
            foreach (var item in valueList)
            {
                var valueItem = item.Trim();
                if (tType == typeof(string))
                    instantiatedIEnumerable.Add(Convert.ToString(valueItem));
                if(tType == typeof(decimal))
                    instantiatedIEnumerable.Add(Convert.ToDecimal(valueItem));
                if (tType == typeof(int))
                    instantiatedIEnumerable.Add(Convert.ToInt64(valueItem));
                //var listValue = TConverter.ChangeType(tType, item);
                //instantiatedIEnumerable.Add(listValue);
                //instantiatedIEnumerable.Append(item);
            }
            
            
            return instantiatedIEnumerable;
        }

        internal static void ValidateInputString(string valueFromRunsettings)
        {
            if (!valueFromRunsettings.StartsWith('[') || !valueFromRunsettings.EndsWith(']'))
            {
                throw new ArgumentException($"IEnumerable value {valueFromRunsettings} from runsettings " +
                    $"does not begin and end with square brackets");
            }

            

        }
    }
}
