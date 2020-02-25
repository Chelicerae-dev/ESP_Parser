using System;
using HtmlAgilityPack;
using System.IO;
using System.Net;
using System.Web;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace ESP_Parser
{
    public class Grubber
    {
        public void Main(string[] args)
        {
            string addr = "";
            string LoadPage(string url)     //HtmlAgilityPack initial page load module
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

            static string address(string addr)
            {
                string addressTemp = "@'" + addr + "'";
                return addressTemp.ToString();
            }
            
            //string adr = address(addr);
            var pageContent = LoadPage("@'" + address(addr)+"'");
            var document = new HtmlDocument();  //Creating new page to parse
            document.LoadHtml(pageContent);     //Creating new page to parse
            HtmlNode grubId(string id)          //Guitar-World description node
            {
                var grubberId = document.DocumentNode.SelectSingleNode("//div[@id='" + id + "']");
                return grubberId;
            }
            string grubContent(string content)  //Guitar-World node selector by content
            {
                var nodeContent = document.DocumentNode.SelectSingleNode("//tr[th='" + content + "']");
                var nodeMed = nodeContent.SelectSingleNode("td");
                string nodeValue = nodeMed.InnerText;
                return nodeValue;
            }
            string grubBrand(string labelBrand)       //labelBrand = "Производитель:" for Guitar-World
            {
                var nodeBrand = document.DocumentNode.SelectSingleNode("//div[label='" + labelBrand + "']");
                var nodePBrand = nodeBrand.SelectSingleNode("p");
                string textBrand = nodePBrand.InnerText;
                Console.WriteLine(textBrand);
                return textBrand;
            }
            var desc = grubId("descr");         //var with "description" part
            string descRes = desc.InnerText;    //useless(?)


            string brand = grubBrand("Производитель:");         //Getting value for brand
            string numberOfStrings = grubContent("&Kcy;&ocy;&lcy;&icy;&chcy;&iecy;&scy;&tcy;&vcy;&ocy; &scy;&tcy;&rcy;&ucy;&ncy;"); //Getting value for # of strings variable
            string body = grubContent("&Kcy;&ocy;&rcy;&pcy;&ucy;&scy;");    //getting value for body wood
            string topWoodmethod()
            {
                try

                {
                    string topWoodTemp = grubContent("&Vcy;&iecy;&rcy;&khcy; &kcy;&ocy;&rcy;&pcy;&ucy;&scy;&acy;");       //Getting value for top wood
                    return topWoodTemp;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return "-";
                }
            }
            string topWood = topWoodmethod();
            string construction = grubContent("&Kcy;&ocy;&ncy;&scy;&tcy;&rcy;&ucy;&kcy;&tscy;&icy;&yacy;");  //Getting value for neck construction
            string scale = grubContent("&Mcy;&iecy;&ncy;&zcy;&ucy;&rcy;&acy;");         //Getting value for scale lenght
            string neckWood = grubContent("&Gcy;&rcy;&icy;&fcy;");      //Getting value for neck wood

            string fretboardMethod()
            {
                try
                {
                    string fretboardTemp = grubContent("&Ncy;&acy;&kcy;&lcy;&acy;&dcy;&kcy;&acy; &gcy;&rcy;&icy;&fcy;&acy;");     //Getting value for fretboard wood
                    return fretboardTemp;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Failed a bit with fretboard node type" + e.Message);
                    string fretboardTemp = grubContent("&Ncy;&acy;&kcy;&lcy;&acy;&dcy;&kcy;&acy; &ncy;&acy; &gcy;&rcy;&icy;&fcy;");
                    return fretboardTemp;
                }
            }
            string fretboard = fretboardMethod();

            string numberOfFrets = grubContent("&Lcy;&acy;&dcy;&ycy;"); //Getting value for number of frets
            string color = grubContent("&TScy;&vcy;&iecy;&tcy;");         //Getting value for color
            string bridge = grubContent("&Bcy;&rcy;&icy;&dcy;&zhcy;");        //Getting value for bridge type
            string pickups1 = grubContent("&Dcy;&acy;&tcy;&chcy;&icy;&kcy; &ucy; &gcy;&rcy;&icy;&fcy;&acy;");       //Getting value for pickups
            string pickups2 = grubContent("&Dcy;&acy;&tcy;&chcy;&icy;&kcy; &ucy; &bcy;&rcy;&icy;&dcy;&zhcy;&acy;");
            string controls = grubContent("&Rcy;&ucy;&chcy;&kcy;&icy; &ucy;&pcy;&rcy;&acy;&vcy;&lcy;&iecy;&ncy;&icy;&yacy;");      //Getting value for controls
            //string misc = grubContent("");          //Getting value for misc (to be changed?)

            Transliterate trans = new Transliterate();   //Initializing Transliterate class for further usage
            Console.WriteLine(trans.MyDecoding(body));   //
            string writePath = "esp.txt";


            try
            {
                using (StreamWriter sw = new StreamWriter(writePath, false, System.Text.Encoding.Default))
                {
                    sw.WriteLine(HttpUtility.HtmlDecode(descRes) + "\n");
                    sw.WriteLine("Количество струн : " + numberOfStrings + "\n");
                    sw.WriteLine("Материал корпуса : " + trans.MyDecoding(body));
                    sw.WriteLine("Бренд : " + brand);
                    sw.WriteLine("Материал топа : " + trans.MyDecoding(topWood));
                    sw.WriteLine("Крепление грифа : " + trans.MyDecoding(construction));
                    sw.WriteLine("Мензура : " + trans.MyDecoding(scale));
                    sw.WriteLine("Материал грифа : " + trans.MyDecoding(neckWood));
                    sw.WriteLine("Материал накладки : " + trans.MyDecoding(fretboard));
                    sw.WriteLine("Количество ладов : " + trans.MyDecoding(numberOfFrets));
                    sw.WriteLine("Цвет : " + trans.MyDecoding(color));
                    sw.WriteLine("Бридж : " + trans.MyDecoding(bridge));
                    sw.WriteLine("Звукосниматели : " + trans.MyDecoding(pickups1) + trans.MyDecoding(pickups2));
                    sw.WriteLine("Органы управления : " + HttpUtility.HtmlDecode(trans.MyDecoding(controls)));
                    sw.WriteLine("Прочее : " /*+ trans.MyDecoding(misc)*/);
                    Console.WriteLine("Done");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }           
        }
        public Grubber(string Addr)
        {
            string addr = Addr;
        }

    }
}
