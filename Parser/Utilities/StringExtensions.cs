using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    public static class StringExtensions
    {
        /// <summary>
        /// Разделяет строку на строки с искомыми началом и концом
        /// </summary>
        /// <param name="str"></param>
        /// <param name="startSeparatorString">Строка, с которой должна начинаться</param>
        /// <param name="endSeparatorString">Строка, которой должна заканчиваться</param>
        public static IEnumerable<string> SplitBetweenStrings(this string str, string startSeparatorString, string endSeparatorString)
        {
            var startIndex = 0;

            while (startIndex != -1)
            {
                startIndex = str.IndexOf(startSeparatorString, startIndex);
                
                if(startIndex == -1) break;

                var endIndex = str.IndexOf(endSeparatorString, startIndex);

                if (startIndex != -1 && endIndex != -1) yield return str.Substring(startIndex, endIndex - startIndex).Trim();
                else yield return str;

                startIndex = endIndex;
            }
        }

        public static IEnumerable<string> SplitByString(this string str, string separatorString)
        {
            var startIndex = str.IndexOf(separatorString);

            var endIndex = str.IndexOf(separatorString, startIndex);

            if (startIndex != -1 && endIndex == -1) endIndex = str.Length - 1;

            var element = str.Substring(startIndex, endIndex - startIndex).Trim();

            yield return element;
        }
    }
}