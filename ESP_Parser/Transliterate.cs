using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Globalization;

namespace ESP_Parser
{
    public class Transliterate
    {
        protected static Dictionary<string, string> translit =
        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
                {"a", "а"},
                {"b", "б"},
                {"v", "в"},
                {"g", "г"},
                {"d", "д"},
                {"ie", "е"},
                {"io", "ё"},
                {"zh", "ж"},
                {"z", "з"},
                {"i", "и"},
                {"j", "й"},
                {"k", "к"},
                {"l", "л"},
                {"m", "м"},
                {"n", "н"},
                {"o", "о"},
                {"p", "п"},
                {"r", "р"},
                {"s", "с"},
                {"t", "т"},
                {"u", "у"},
                {"f", "ф"},
                {"h", "х"},
                {"ts", "ц"},
                {"ch", "ч"},
                {"sh", "ш"},
                {"kh", "х" },
                {"shch", "щ"},
                {"hard", "ъ"},
                {"y", "ы"},
                {"soft", "ь"},
                {"e", "э"},
                {"yu", "ю"},
                {"ya", "я"}
        };

        public string MyDecoding(string value)
        {
            return Regex
              .Replace(value, @"&(?<letter>[A-Za-z]+)cy;", m => {
                  string v = m.Groups["letter"].Value;

                  return char.IsUpper(v[0])
          ? CultureInfo.InvariantCulture.TextInfo.ToTitleCase(translit[v])
          : translit[v];
              }
              );
        }
 
    }
}
