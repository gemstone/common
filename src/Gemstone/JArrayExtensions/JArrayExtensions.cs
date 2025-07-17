using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Gemstone.JArrayExtensions
{
    /// <summary>
    /// Defines extension methods for JArray.
    /// </summary>
    public static class JArrayExtensions
    {
        /// <summary>
        /// Rebuilds original array so that items from originalArray are replaced by any same‑key items in the override array
        /// </summary>
        public static JArray MergeByKey(this JArray originalArray, JArray overrideArray, string uniqueKeyField)
        {
            // 1) Build lookup from original
            Dictionary<string, JObject> lookup = originalArray
                .OfType<JObject>()
                .Where(o => o[uniqueKeyField] is not null)
                .ToDictionary(o => o[uniqueKeyField]!.ToString()!, o => o);

            // 2) Override
            foreach (JObject item in overrideArray.OfType<JObject>())
            {
                string? key = item[uniqueKeyField]?.ToString();
                if (!string.IsNullOrEmpty(key) && lookup.ContainsKey(key))
                    lookup[key] = item;
            }

            return new JArray(lookup.Values);
        }

    }
}
