using System;
using System.Collections.Generic;
using System.Globalization;
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
            if (!type.GetGenericTypeDefinition().GetInterfaces().Any(i => i.Name.Contains("IEnumerable")))
                throw new ArgumentException("Object does not inherit from IEnumerable and cannot be converted");
            
            string valueStr = value.ToString();
            ValidateInputString(valueStr);

            var listOfType = type.GetGenericArguments().First();
            valueStr = valueStr.Trim().Trim('[').Trim(']');
            var valueList = valueStr.Split(',');

            var listType = typeof(List<>).MakeGenericType(listOfType);
            dynamic instantiatedList = Activator.CreateInstance(listType);
            
            foreach (var item in valueList)
            {
                var valueItem = item.Trim();
                dynamic convertedValue = Convert.ChangeType(valueItem, listOfType);
                instantiatedList.Add(convertedValue);
            }
            
            return instantiatedList;
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
