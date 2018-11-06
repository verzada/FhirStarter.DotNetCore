using System;
using System.Collections.Generic;

namespace FhirStarter.Bonfire.DotNetCore.SparkEngine.Search.Support
{
    public static class StringExtensions
    {
        public static string[] SplitNotEscaped(this string value, char separator)
        {
            var word = string.Empty;
            var result = new List<string>();
            var seenEscape = false;

            for (var i = 0; i < value.Length; i++)
            {
                if (i == '\\')
                {
                    seenEscape = true;
                    continue;
                }
               
                if (i == separator && !seenEscape)
                {
                    result.Add(word);
                    word = string.Empty;
                    continue;
                }

                if (seenEscape)
                {
                    word += '\\';
                    seenEscape = false;
                }

                word += i;
            }

            result.Add(word);

            return result.ToArray();
        }

        public static Tuple<string,string> SplitLeft(this string text, char separator)
        {
            var pos = text.IndexOf(separator);

            if (pos == -1)
                return Tuple.Create(text, (string)null);     // Nothing to split
            var key = text.Substring(0, pos);
            var value = text.Substring(pos + 1);

            return Tuple.Create(key, value);
        }
    }
}
