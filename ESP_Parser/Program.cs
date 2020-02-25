using System;
using HtmlAgilityPack;
using System.IO;
using System.Net;
using System.Web;
using System.Text.RegularExpressions;
using System.Collections.Generic;


namespace ESP_Parser
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            string LoadPage(string url)
            {
                var result = "";
                var request = (HttpWebRequest)WebRequest.Create(url);
                var response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var receiveStream = response.GetResponseStream();
                    if (receiveStream != null)
                    {
                        StreamReader readStream;
                        if (response.CharacterSet == null)
                            readStream = new StreamReader(receiveStream);
                        else
                            readStream = new StreamReader(receiveStream);
                        result = readStream.ReadToEnd();
                        readStream.Close();
                    }
                    response.Close();
                }
                return result;
            }
            var pageContent = LoadPage(@"https://www.guitar-world.ru/catalog/guitars/elektrogitary/lsn200frmchm.html");
            var document = new HtmlDocument();
            document.LoadHtml(pageContent);
            HtmlNode grubId(string id)
            {
                var grubberId = document.DocumentNode.SelectSingleNode("//div[@id='" + id + "']");
                return grubberId;
            }
            string grubContent(string content)
            {
                var nodeContent = document.DocumentNode.SelectSingleNode("//tr[th='" + content + "']");
                var nodeMed = nodeContent.SelectSingleNode("td");
                string nodeValue = nodeMed.InnerText;
                return nodeValue; 
            }
            

        //var desc = document.DocumentNode.SelectSingleNode("//div[@id='descr']");
            var desc = grubId("descr");
            string descRes = desc.InnerText;
            string numberOfStrings = grubContent("&Kcy;&ocy;&lcy;&icy;&chcy;&iecy;&scy;&tcy;&vcy;&ocy; &scy;&tcy;&rcy;&ucy;&ncy;");
            string body = grubContent("&Kcy;&ocy;&rcy;&pcy;&ucy;&scy;");
            Transliterate Body = new Transliterate();
            Console.WriteLine(Body.MyDecoding(body));
            string writePath = "esp.txt";

            ////////////////////////
            /*
            string CleanInput(string strIn)
            {
            // Replace invalid characters with empty strings.
            try
            {
            return Regex.Replace(strIn, @"[^\s\w\.@-]", "",
            RegexOptions.None, TimeSpan.FromSeconds(1.5));
            }
            // If we timeout when replacing invalid characters,
            // we should return Empty.
            catch (RegexMatchTimeoutException)
            {
            return String.Empty;
            }
            }
            */
            ///////////////////////
            //string descResult = CleanInput(descRes);
            ///////////////////////
            /*public static Dictionary<string, string> translit =
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
            };*/
            try
            {
                using (StreamWriter sw = new StreamWriter(writePath, false, System.Text.Encoding.Default))
                {
                    sw.WriteLine(HttpUtility.HtmlDecode(descRes) + "\n");
                    sw.WriteLine(numberOfStrings+"\n");
                    sw.WriteLine(Body.MyDecoding(body));
                    Console.WriteLine(HtmlEntity.DeEntitize(body));
                    Console.WriteLine("Done");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}