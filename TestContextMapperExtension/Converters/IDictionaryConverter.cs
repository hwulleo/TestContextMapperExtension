using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TestContextMapperExtension.Converters
{
    internal class IDictionaryConverter
    {
        public static object ConvertToIDictionary(Type type, object value)
        {
            if (value is null)
                return null;
            if (!type.GetGenericTypeDefinition().GetInterfaces().Any(i => i.Name.Contains("IDictionary")))
                throw new ArgumentException("Object does not inherit from IDictionary and cannot be converted");
            
            string valueStr = value.ToString();
            ValidateInputString(valueStr);

            valueStr = valueStr.Trim();
            valueStr = valueStr.Remove(0, 1);
            valueStr = valueStr.Remove(valueStr.Length - 1, 1);
            Regex curlyBraceRegex = new Regex("[^{\\}]+(?=})");
            var keyValueMatches = curlyBraceRegex.Matches(valueStr);
            List<KeyValuePair<string, string>> keyValuePairs = new List<KeyValuePair<string, string>>();
            foreach (Match potentialKvp in keyValueMatches)
            {
                var kvp = potentialKvp.Value.Split(',');
                keyValuePairs.Add(new KeyValuePair<string, string>(kvp[0]?.Trim(), kvp[1]?.Trim()));
            }

            dynamic instantiatedIDict = Activator.CreateInstance(type);

            var typeOfKey = type.GetGenericArguments().First();
            var typeOfValue = type.GetGenericArguments().Last();
            foreach (var item in keyValuePairs)
            {
                dynamic keyValue = Convert.ChangeType(item.Key, typeOfKey);
                dynamic valueValue = Convert.ChangeType(item.Value, typeOfValue);
                //dynamic convertedKvp = new KeyValuePair<dynamic, dynamic>(keyValue, valueValue);
                instantiatedIDict.Add(keyValue, valueValue);
            }
            
            return instantiatedIDict;
        }

        internal static void ValidateInputString(string valueFromRunsettings)
        {
            if (!valueFromRunsettings.StartsWith('{') || !valueFromRunsettings.EndsWith('}'))
            {
                throw new ArgumentException($"IDictionary value {valueFromRunsettings} from runsettings " +
                    $"does not begin and end with curly braces ( {{}} )");
            }

            if(valueFromRunsettings.Count(x => x == '{') != valueFromRunsettings.Count(x => x == '}'))
            {
                throw new ArgumentException($"IDictionary value {valueFromRunsettings} from runsettings " +
                    $"does not have an equal number of opening ({{) and closing (}}) curly braces");
            }

            

        }
    }
}
