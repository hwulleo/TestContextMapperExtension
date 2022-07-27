using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestContextMapperExtension.Converters
{
    internal class IListConverter
    {
        public static object ConvertToIList(Type type, object value)
        {
            if (value is null)
                return null;
            if (!type.GetGenericTypeDefinition().GetInterfaces().Any(i => i.Name.Contains("IList")))
                throw new ArgumentException("Object does not inherit from IList and cannot be converted");
            
            string valueStr = value.ToString();
            ValidateInputString(valueStr);

            var listOfType = type.GetGenericArguments().First();
            valueStr = valueStr.Trim().Trim('[').Trim(']');
            var valueList = valueStr.Split(',');

            dynamic instantiatedIList = Activator.CreateInstance(type);
            
            foreach (var item in valueList)
            {
                var valueItem = item.Trim();
                dynamic convertedValue = Convert.ChangeType(valueItem, listOfType);
                instantiatedIList.Add(convertedValue);
            }
            
            return instantiatedIList;
        }

        internal static void ValidateInputString(string valueFromRunsettings)
        {
            if (!valueFromRunsettings.StartsWith('[') || !valueFromRunsettings.EndsWith(']'))
            {
                throw new ArgumentException($"IList value {valueFromRunsettings} from runsettings " +
                    $"does not begin and end with square brackets");
            }

            

        }
    }
}
