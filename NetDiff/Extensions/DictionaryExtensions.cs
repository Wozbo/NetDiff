using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDiff.Extensions
{
    public static class DictionaryExtensions
    {
        /// <summary>
        /// http://stackoverflow.com/a/33223183/356849
        ///
        /// TODO: move this to csharp-extensions, and make generic
        /// </summary>
        /// <typeparam name="TK"></typeparam>
        /// <typeparam name="TV"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string GetValue(this Dictionary<System.Type, string> dict, System.Type key, string defaultValue = default(string))
        {
            string value;

            return dict.TryGetValue(key, out value)
                       ? value
                       : default(string);
        }

        public static string[] GetValue(this Dictionary<System.Type, string[]> dict, System.Type key, string[] defaultValue = default(string[]))
        {
            string[] value;

            return dict.TryGetValue(key, out value)
                       ? value
                       : default(string[]);
        }
    }
}