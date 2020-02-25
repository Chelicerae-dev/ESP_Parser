using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Globalization;

namespace ESP_Parser
{
    public class Transliterate
    {
        /*static string body;
        private string transliteratedText = Regex.Replace(
               body,
             @"&(?<letter>[A-Za-z]+)cy;",
               m => m.Groups["letter"].Value);

        //Console.WriteLine(transliteratedText);*/
        ///////////////////////
        protected static Dictionary<string, string> translit =
        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
                {"a", "а"},
                {"b", "б"},
                {"v", "в"},
                {"g", "г"},
                {"d", "д"},
                {"ie", "е"},
                //{"", "ё"}, //TODO: define the letter transliteration
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
                {"shch", "щ"},
                //{"", "ъ"}, //TODO: define the letter transliteration
                {"y", "ы"},
                //{"", "ь"}, //TODO: define the letter transliteration
                //{"", "э"}, //TODO: define the letter transliteration
                //{"", "ю"}, //TODO: define the letter transliteration
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
