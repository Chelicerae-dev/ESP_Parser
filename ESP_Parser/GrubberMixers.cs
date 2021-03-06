﻿using System;
using HtmlAgilityPack;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Collections.Generic;



namespace ESP_Parser
{
    public class GrubberMixers
    {
        public string grubImg(string Address)
        {
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11;
            string LoadPage(string url)     //HtmlAgilityPack initial page load module
            {
                try
                {
                    var result = "";
                    var request = (HttpWebRequest)WebRequest.Create(url);
                    var response = (HttpWebResponse)request.GetResponse();
                    Console.WriteLine("Link for image is " + url);
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
            var pageContent = LoadPage(Address);
            var document = new HtmlDocument();  //Creating new page to parse
            document.LoadHtml(pageContent);     //Creating new page to parse
            var nodeImgAddr = document.DocumentNode.SelectSingleNode("//img[@class=\"image_0\"]");
            string imgLink = "https://www.amplifier.ru" + nodeImgAddr.GetAttributeValue("src", "default");
            //Console.WriteLine(imgLink + "is imgLink");
            return imgLink;
        }
        public virtual CsvLine Grub(string addr)
        {
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
            string attr_group()
            {
                int i = 0;
                string temp = "";
                foreach (string spec in grubSpecs)
                {
                    temp += "\n Микшеры";
                }
                return temp;

            }
            string grubContent(string content)  // node selector by content
            {
                var nodeContent = document.DocumentNode.SelectSingleNode("//tr[td=\"" + content + "\"]");
                var nodeMed = nodeContent.LastChild;
                string nodeValue = nodeMed.InnerText;
                return nodeValue;
            }
            string grubBrand()       //labelBrand = "Производитель:" for Guitar-World
            {
                var nodeBrand = document.DocumentNode.SelectSingleNode("//b[@id=\"prodbrand\"]");
                Console.WriteLine(nodeBrand.InnerText);
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
                Console.WriteLine(nodeModel.InnerText);
                return nodeModel.InnerText;
            }
            string grubName()            //h1 id = "prodtitle" for Amplifier
            {
                var nodeName = document.DocumentNode.SelectSingleNode("//h1[@id=\"prodtitle\"]");
                Console.WriteLine(nodeName.InnerText);
                return Regex.Replace(              //removing whitespaces
                    nodeName.InnerText, @"^\s+|\s+$", "");
            }
            string grubImgTemp()
            {
                var nodeImgAddr = document.DocumentNode.SelectSingleNode("//img[@class=\"image_0\"]");
                string imgLink = "https://www.amplifier.ru" + nodeImgAddr.Attributes["src"].Value;
                Console.WriteLine(imgLink);
                return imgLink;
            }

            ///////////
            string brand = grubBrand();         //Getting value for brand
            string imageTemp = Regex.Replace(grubModel(), @"\s|\/", ""); //model.Replace(" ", "").Replace("/", "");
            string image = imageTemp + ".png";
            Transliterate trans = new Transliterate();   //Initializing Transliterate class for further usage
                                                         //Console.WriteLine(trans.MyDecoding(body));   //
            string imageAddr()
            {
                try
                {
                    string temp = grubImgTemp();
                    return temp;
                }
                catch
                {
                    Console.WriteLine("Unable to grub img");
                    return "";
                }
            }
            string imageAddress = imageAddr();

            CsvLine ToCsv = new CsvLine();
            ToCsv.name = grubName();
            ToCsv.model = grubModel();
            ToCsv.price = grubPrice().Replace(" ", "").Replace("р.", "");
            ToCsv.categories = "Звуковое оборудование > Микшеры"; //для микшеров
            ToCsv.quantity = 2;
            ToCsv.manufacturer = brand;
            if (feat != null)
            {
                ToCsv.description = desc.InnerHtml + "\n<h3>Особенности</h3>\n" + feat.InnerHtml;
            }
            else
            {
                ToCsv.description = desc.InnerHtml;
            }
            if (Specs() != null)
            {
                ToCsv.attributes = trans.MyDecoding(Specs());
            }
            else
            {
                ToCsv.attributes = "";
            }
            ToCsv.attributes_group = attr_group();
            ToCsv.options = "";
            ToCsv.option_type = "";
            ToCsv.images = "/catalog/mixers/" + image;
            CsvLine lister(string addr)
            {
                Grub(addr);
                return ToCsv;
            }
            return ToCsv;
        }
        public GrubberMixers(string Addr)
        {
            string addr = Addr;
        }
    }
}
