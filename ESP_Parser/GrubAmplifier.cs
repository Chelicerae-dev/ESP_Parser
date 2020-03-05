using System;
using HtmlAgilityPack;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Collections.Generic;



namespace ESP_Parser
{
    public class GrubAmplifier
    {
        public string addr { get; set; }
        string LoadPage(string addr)     //HtmlAgilityPack initial page load module
        {
            try
            {
                var result = "";
                var request = (HttpWebRequest)WebRequest.Create(addr);
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
            catch
            {
                throw;
            }
        }
        public string Category { get; set; }     //Goods category 
        public string AttrGroup { get; set; }    //goes to Attributes_Group in CSV
        public string imgPath { get; set; }         //goes to image path in csv
        public string grubImg(string addr)
        {
            var pageContent = LoadPage(addr);
            var document = new HtmlDocument();  //Creating new page to parse
            document.LoadHtml(pageContent);     //Creating new page to parse
            var nodeImgAddr = document.DocumentNode.SelectSingleNode("//img[@class=\"image_0\"]");
            string imgLink = "https://www.amplifier.ru" + nodeImgAddr.GetAttributeValue("src", "default");
            return imgLink;
        }
        public string[] Grub() 
        {
            string[] temp = new string[8];
            var pageContent = LoadPage(addr);
            var document = new HtmlDocument();  //Creating new page to parse
            document.LoadHtml(pageContent);     //Creating new page to parse

            HtmlNode grubId(string id)          //Guitar-World description node
            {
                var grubberId = document.DocumentNode.SelectSingleNode("//div[@id=\"" + id + "\"]");
                return grubberId;
            }
                       ////// Full specs table below
            List<string> grubSpecs = new List<string>();    //making list for all specs availible (which are different at every page
            {
                try
                {
                    var TableNode = document.DocumentNode.SelectSingleNode("//table[@class=\"shop_attributes\"]").SelectSingleNode("tbody");
                    var SpecName = TableNode.SelectNodes("tr/td[@itemprop=\"name\"]");
                    var SpecValue = TableNode.SelectNodes("tr/td[@itemprop=\"value\"]");
                    int i = 0;
                    foreach (HtmlNode count in SpecName)
                    {
                        grubSpecs.Add(Regex.Replace(SpecName[i].InnerText, @"^\s+|\s+$|\n", "") + " : " + Regex.Replace(SpecValue[i].InnerText, @"^\s+|\s+$|\n", ""));
                        i++;
                    }
                }
                catch
                {
                    Console.WriteLine("No specs!");
                }
            }
            string Specs()  //turning specs list into string
            {
                string SpecsTemp = "";
                int SpecCount = 0;
                foreach (string spec in grubSpecs)
                {
                    SpecsTemp += spec + "\n";
                    SpecCount++;
                }
                return SpecsTemp;
            }
            string attr_group()     //making Attribute_group
            {
                int i = 0;
                string temp = "";
                foreach (string spec in grubSpecs)
                {
                    temp += "\n " + AttrGroup;
                }
                return temp;
            }
            string grubBrand()       //getting brand
            {
                var nodeBrand = document.DocumentNode.SelectSingleNode("//b[@id=\"prodbrand\"]");
                //Console.WriteLine(nodeBrand.InnerText);
                return nodeBrand.InnerText;
            }
            var desc = grubId("tab-fullDescriptionProd");         //var with "description" part
            HtmlNode featMethod()
            {
                try
                {
                    var temp = grubId("tab-featuresProd");
                    return temp;
                }
                catch
                {
                    return null;
                }
            }
            var feat = featMethod();
            string grubPrice()       //itemprop = "price" for Amplifier.ru
            {
                try
                {
                    var nodePrice = document.DocumentNode.SelectSingleNode("//span[@itemprop=\"price\"]");
                    return nodePrice.InnerText.Replace("\"", "").Replace(" ", "");
                }
                catch
                {
                    Console.WriteLine("Unable to grub price!");
                    return "";
                }

            }
            string grubModel()       //itemprop = "model" for Amplifier
            {
                var nodeModel = document.DocumentNode.SelectSingleNode("//span[@itemprop=\"model\"]");
                //Console.WriteLine(nodeModel.InnerText);
                return nodeModel.InnerText;
            }
            string grubName()            //h1 id = "prodtitle" for Amplifier
            {
                var nodeName = document.DocumentNode.SelectSingleNode("//h1[@id=\"prodtitle\"]");
                //Console.WriteLine(nodeName.InnerText);
                return Regex.Replace(              //removing whitespaces
                    nodeName.InnerText, @"^\s+|\s+$", "");
            }
            ///////////
            string brand = grubBrand();         //Getting value for brand
            string imageTemp = Regex.Replace(grubModel(), @"\s|\/", ""); //model.Replace(" ", "").Replace("/", "");
            string image = imageTemp + ".png";
            Transliterate trans = new Transliterate();   //Initializing Transliterate class for further usage
            temp[0] = grubName();
            temp[1] = grubModel();
            temp[2] = Regex.Replace(grubPrice(), @"\s|р\.", "");
            temp[3] = brand;
            if (feat != null)
            {
                temp[4] = desc.InnerHtml + "\n<h3>Особенности</h3>\n" + feat.InnerHtml;
            }
            else;
            {
                temp[4] = desc.InnerHtml;
            }
            temp[5] = (Specs() != null) ? trans.MyDecoding(Specs()) : "";
            temp[6] = attr_group();
            temp[7] = image;
            return temp;
        }
            public CsvLine WriteCsv()
            {
                CsvLine ToCsv = new CsvLine();
                ToCsv.name = Grub()[0];
                ToCsv.model = Grub()[1];
                ToCsv.price = Grub()[2];
                ToCsv.categories = Category; //для микшеров
                ToCsv.quantity = 2;
                ToCsv.manufacturer = Grub()[3];
                ToCsv.description = Grub()[4];
                ToCsv.attributes = Grub()[5];
                ToCsv.attributes_group = Grub()[6];
                ToCsv.options = "";
                ToCsv.option_type = "";
                ToCsv.images = "/catalog/" + imgPath + "/" + Grub()[7];
                return ToCsv;
            }
        public GrubAmplifier()
        {
        }
    }
}
