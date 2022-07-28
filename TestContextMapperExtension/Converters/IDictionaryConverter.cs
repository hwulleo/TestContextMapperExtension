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
        /// <summary>
        /// Match everything contained within curly braces
        /// </summary>
        static Regex curlyBraceRegex = new Regex("[^{\\}]+(?=})");

        /// <summary>
        /// Converts a string that is following the collection initializer syntax for a dictionary
        /// into a type that inherits from IDictionary. <see href="https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/how-to-initialize-a-dictionary-with-a-collection-initializer">Example syntax</see>
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static object ConvertToIDictionary(Type type, object value)
        {
            if (value is null)
                return null;
            if (!type.GetGenericTypeDefinition().GetInterfaces().Any(i => i.Name.Contains("IDictionary")))
                throw new ArgumentException("Object does not inherit from IDictionary and cannot be converted");
            
            string valueStr = value.ToString();
            ValidateInputString(valueStr);

            List<KeyValuePair<string, string>> keyValuePairs = ExtractKeyValuePairs(valueStr);

            dynamic instantiatedIDict = Activator.CreateInstance(type);

            var typeOfKey = type.GetGenericArguments().First();
            var typeOfValue = type.GetGenericArguments().Last();
            foreach (var kvp in keyValuePairs)
            {
                dynamic keyValue = Convert.ChangeType(kvp.Key, typeOfKey);
                dynamic valueValue = Convert.ChangeType(kvp.Value, typeOfValue);
                instantiatedIDict.Add(keyValue, valueValue);
            }
            
            return instantiatedIDict;
        }

        internal static void ValidateInputString(string valueFromRunsettings)
        {
            if (!valueFromRunsettings.StartsWith('{') || !valueFromRunsettings.EndsWith('}'))
            {
                throw new FormatException($"IDictionary value {valueFromRunsettings} from runsettings " +
                    $"does not begin and end with curly braces ( {{}} )");
            }

            if(valueFromRunsettings.Count(x => x == '{') != valueFromRunsettings.Count(x => x == '}'))
            {
                throw new FormatException($"IDictionary value {valueFromRunsettings} from runsettings " +
                    $"does not have an equal number of opening ({{) and closing (}}) curly braces");
            }
        }

        /// <summary>
        /// Parse the string from the runsettings into a List of key value pairs 
        /// </summary>
        /// <param name="runsettingsValue"></param>
        /// <returns></returns>
        internal static List<KeyValuePair<string,string>> ExtractKeyValuePairs(string runsettingsValue)
        {
            //remove 1 leading and 1 trailing curly brace
            runsettingsValue = runsettingsValue.Trim();
            runsettingsValue = runsettingsValue.Remove(0, 1);
            runsettingsValue = runsettingsValue.Remove(runsettingsValue.Length - 1, 1);

            //curlyBraceRegex matches all contents contained within curly braces
            var keyValueMatches = curlyBraceRegex.Matches(runsettingsValue);
            List<KeyValuePair<string, string>> keyValuePairs = new List<KeyValuePair<string, string>>();
            foreach (Match potentialKvp in keyValueMatches)
            {
                if (!potentialKvp.Value.Contains(','))
                    throw new FormatException($"Potential Dictionary value from runsettings {potentialKvp} does not contain any commas and cannot be parsed");
                if(potentialKvp.Value.Count(x => x == ',') > 1)
                    throw new FormatException($"Potential Dictionary value from runsettings {potentialKvp} contains more than one comma and cannot be parsed");
                var kvp = potentialKvp.Value.Split(',');
                keyValuePairs.Add(new KeyValuePair<string, string>(kvp[0]?.Trim(), kvp[1]?.Trim()));
            }
            return keyValuePairs;
        }
    }
}
