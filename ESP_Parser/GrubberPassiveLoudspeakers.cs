using System;
using HtmlAgilityPack;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Collections.Generic;


namespace ESP_Parser
{
    public class GrubberPassiveLoudspeakers
    {
        public string grubImg(string Address)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11;
            string LoadPage(string url)     //HtmlAgilityPack initial page load module
            {
                try
                {
                    var result = "";
                    var request = (HttpWebRequest)WebRequest.Create(url);
                    var response = (HttpWebResponse)request.GetResponse();
                    //Console.WriteLine("Link for image is " + url);
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
        public CsvLine Grub(string addr)
        {
            string LoadPage(string url)     //HtmlAgilityPack initial page load module
            {
                var result = "";
                var request = (HttpWebRequest)WebRequest.Create(url);
                var response = (HttpWebResponse)request.GetResponse();
                Console.WriteLine("Current content link is " + url);
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

            /*static string address(string addr)    //not needed anymore, but I'll leave it here for a while
            {
                string addressTemp = "@" + addr;
                return addressTemp.ToString();
            }
            */
            var pageContent = LoadPage(addr);
            var document = new HtmlDocument();  //Creating new page to parse
            document.LoadHtml(pageContent);     //Creating new page to parse
            HtmlNode grubId(string id)          //Guitar-World description node
            {
                var grubberId = document.DocumentNode.SelectSingleNode("//div[@id=\"" + id + "\"]");
                return grubberId;
            }
            string grubContent(string content)  //Guitar-World node selector by content
            {
                var nodeContent = document.DocumentNode.SelectSingleNode("//tr[td=\"" + content + "\"]");
                var nodeMed = nodeContent.LastChild;
                string nodeValue = nodeMed.InnerText;
                return nodeValue;
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
                    temp += "\n Акустические системы";
                }
                return temp;

            }
            string grubBrand()       //labelBrand = "Производитель:" for Guitar-World
            {
                var nodeBrand = document.DocumentNode.SelectSingleNode("//b[@id=\"prodbrand\"]");
                //var nodePBrand = nodeBrand.SelectSingleNode("p");
                //string textBrand = nodePBrand.InnerText;
                Console.WriteLine(nodeBrand.InnerText);
                return nodeBrand.InnerText;
            }
            var desc = grubId("tab-fullDescriptionProd");         //var with "description" part
            var feat = grubId("tab-featuresProd");                                                           ///////////
            ///
            string grubPrice()       //itemprop = "price" for Amplifier.ru
            {
                try
                {
                    var nodePrice = document.DocumentNode.SelectSingleNode("//span[@itemprop=\"price\"]");
                    return nodePrice.InnerText.Replace("\"", "").Replace(" ", "");
                    //Console.WriteLine(nodeCheck.NextSibling.Name);
                    //Console.WriteLine(nodeCheck.NextSibling.InnerText);
                    /*switch (nodeCheck.Count)
                    {
                        case 1:
                            Console.WriteLine(nodePrice);                 //single span (no discount)
                            string textPrice = nodePrice.InnerText;
                            Console.WriteLine(textPrice);
                            string temp = Regex.Replace(                  //removing spaces
                                textPrice, @"\s", "");
                            return temp.Replace("руб.", "");              //removing letters

                        case 3:                                           //multiple spans (discount)
                            var nodeSCheck = nodePrice.FirstChild.NextSibling;
                            string textDPrice = nodeSCheck.InnerText;
                            Console.WriteLine(textDPrice);
                            string tempD = Regex.Replace(                  //removing spaces
                                textDPrice, @"\s", "");
                            return tempD.Replace("руб.", "");              //removing letters
                        default:
                            return "WTF";
                    }*/
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
                //var nodePModel = nodeModel.SelectSingleNode("p");
                //string textModel = nodePModel.InnerText;
                Console.WriteLine(nodeModel.InnerText);
                return nodeModel.InnerText;
            }
            string grubName()            //h1 id = "prodtitle" for Amplifier
            {
                var nodeName = document.DocumentNode.SelectSingleNode("//h1[@id=\"prodtitle\"]");
                //var nodeH1Name = nodeName.SelectSingleNode("h1");
                //string nodeHName = nodeH1Name.InnerText;
                Console.WriteLine(nodeName.InnerText);
                return Regex.Replace(              //removing whitespaces
                    nodeName.InnerText, @"^\s+|\s+$", "");
            }
            string grubImgTemp()
            {
                var nodeImgAddr = document.DocumentNode.SelectSingleNode("//img[@class=\"image_0\"]");
                string imgLink = "https://www.amplifier.ru" + nodeImgAddr.Attributes["src"].Value;
                //Console.WriteLine(imgLink);
                return imgLink;
            }

            ///////////
            string brand = grubBrand();         //Getting value for brand
            string ampMethod()
            {
                try
                {
                    string temp = grubContent("&Ucy;&scy;&icy;&lcy;&icy;&tcy;&iecy;&lcy;&softcy;"); //Getting value for amp
                    return temp;
                }
                catch
                {
                    return "";
                }
            }
            string amp = ampMethod(); //Assigning value for # of strings variable
            string sensMethod()
            {
                try
                {
                    string temp = grubContent("&CHcy;&ucy;&vcy;&scy;&tcy;&vcy;&icy;&tcy;&iecy;&lcy;&softcy;&ncy;&ocy;&scy;&tcy;&softcy;");    //getting value for body wood
                    return temp;
                }
                catch
                {
                    return "";
                }
            }

            string sens = sensMethod();    //Assigning value for body wood
            string outputMethod()
            {
                try

                {
                    string temp = grubContent("&Vcy;&khcy;&ocy;&dcy;&ncy;&ycy;&iecy; &rcy;&acy;&zcy;&hardcy;&iecy;&mcy;&ycy;");       //Getting value for outputs
                    return temp;
                }
                catch
                {
                    return "-";
                }
            }
            string output = outputMethod();   //Assigning value for top wood
            string ohmMethod()
            {
                try
                {
                    string temp = grubContent("&Scy;&ocy;&pcy;&rcy;&ocy;&tcy;&icy;&vcy;&lcy;&iecy;&ncy;&icy;&iecy;");  //Getting value for ohmage
                    return temp;
                }
                catch
                {
                    try
                    {
                        string temp = grubContent("&Ncy;&ocy;&mcy;&icy;&ncy;&acy;&lcy;&softcy;&ncy;&ocy;&iecy; &scy;&ocy;&pcy;&rcy;&ocy;&tcy;&icy;&vcy;&lcy;&iecy;&ncy;&icy;&iecy;");
                        return temp;
                    }
                    catch
                    {
                        return "";
                    }
                }
            }
            string ohm = ohmMethod();  //Assigning value for ohmage
            string inputMethod()
            {
                try
                {
                    string temp = grubContent("&Pcy;&ocy;&dcy;&kcy;&lcy;&yucy;&chcy;&iecy;&ncy;&icy;&iecy;");         //Getting value for inputs
                    return temp;
                }
                catch
                {
                    try
                    {
                        string temp = grubContent("&Vcy;&khcy;&ocy;&dcy;&ycy;");         //Getting value for inputs
                        return temp;
                    }
                    catch
                    {
                        try
                        {
                            string temp = grubContent("&Vcy;&khcy;&ocy;&dcy;&ncy;&ycy;&iecy; &rcy;&acy;&zcy;&hardcy;&iecy;&mcy;&ycy;");
                            return temp;
                        }
                        catch
                        {
                            return "";
                        }
                    }
                }
            }
            string input = inputMethod();         //Assigning value for input
            string powerMethod()
            {
                try
                {
                    string temp = grubContent("&Mcy;&ocy;&shchcy;&ncy;&ocy;&scy;&tcy;&softcy;");      //Getting value power
                    return temp;
                }
                catch
                {
                    try
                    {
                        string temp = grubContent("&Ncy;&ocy;&mcy;&icy;&ncy;&acy;&lcy;&softcy;&ncy;&acy;&yacy; &mcy;&ocy;&shchcy;&ncy;&ocy;&scy;&tcy;&softcy; RMS");
                        return temp;
                    }
                    catch
                    {
                        return "";
                    }
                }
            }
            string power = powerMethod();      //Assigning value for power

            string responseMethod()
            {
                try
                {
                    string temp = grubContent(">&CHcy;&acy;&scy;&tcy;&ocy;&tcy;&ncy;&ycy;&jcy; &dcy;&icy;&acy;&pcy;&acy;&zcy;&ocy;&ncy;");     //Getting value for response
                    return temp;
                }
                catch
                {
                    {
                        try
                        {
                            string temp = grubContent("&Ncy;&acy;&kcy;&lcy;&acy;&dcy;&kcy;&acy; &ncy;&acy; &gcy;&rcy;&icy;&fcy;");
                            return temp;
                        }
                        catch
                        {
                            try
                            {
                                string temp = grubContent("&CHcy;&acy;&scy;&tcy;&ocy;&tcy;&ncy;&ycy;&jcy; &dcy;&icy;&acy;&pcy;&acy;&zcy;&ocy;&ncy; &lpar;-10 &dcy;&Bcy;&rpar;");
                                return temp;
                            }
                            catch
                            {
                                return "";
                            }
                        }
                    }
                }
            }
            string response = responseMethod();   //Assigning value for response
                                                  ///////
            string name = grubName();        //Name of system
            string price = grubPrice();      //Price
            string model = grubModel();      //Model
            string splMethod()
            {
                try
                {
                    string temp = grubContent("&Zcy;&vcy;&ucy;&kcy;&ocy;&vcy;&ocy;&iecy; &dcy;&acy;&vcy;&lcy;&iecy;&ncy;&icy;&iecy; SPL"); //Getting value for spl
                    return temp;
                }
                catch
                {
                    try
                    {
                        string temp = grubContent("&Zcy;&vcy;&ucy;&kcy;&ocy;&vcy;&ocy;&iecy; &dcy;&acy;&vcy;&lcy;&iecy;&ncy;&icy;&iecy; SPL"); //Getting value for spl
                        return temp;
                    }
                    catch
                    {
                        return "";
                    }
                }
            }
            string spl = splMethod(); //Assigning to numberOfFrets
            string dimMethod()
            {
                try
                {
                    string temp = grubContent("&Rcy;&acy;&zcy;&mcy;&iecy;&rcy;&ycy;");         //Getting value for color
                    return temp;
                }
                catch
                {
                    try
                    {
                        string temp = grubContent("&Gcy;&acy;&bcy;&acy;&rcy;&icy;&tcy;&ycy;");
                        return temp;
                    }
                    catch
                    {
                        try
                        {
                            string temp = grubContent("&Gcy;&acy;&bcy;&acy;&rcy;&icy;&tcy;&ycy; &lpar;&Vcy;x&SHcy;x&Gcy;&rpar;");
                            return temp;
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                            return "";
                        }
                    }
                }
            }
            string dim = dimMethod();    //Assigning value for color
            string weightMethod()
            {
                try
                {
                    string temp = grubContent("&Vcy;&iecy;&scy;");        //Getting value for bridge type
                    return temp;
                }
                catch
                {

                    return "";

                }
            }
            string weight = weightMethod();        //Assigning value for bridge type
            string speaker1Method()
            {
                try
                {
                    string sp1Temp = grubContent("&Ncy;&CHcy; &dcy;&icy;&ncy;&acy;&mcy;&icy;&kcy;");       //Getting value for neck pickup
                    return sp1Temp;
                }
                catch
                {
                    try
                    {
                        string temp = grubContent("&Dcy;&icy;&ncy;&acy;&mcy;&icy;&kcy;&icy;");
                        return temp;
                    }
                    catch
                    {
                        try
                        {
                            string temp = grubContent("&Icy;&zcy;&lcy;&ucy;&chcy;&acy;&tcy;&iecy;&lcy;&icy;");
                            return temp;
                        }
                        catch
                        {
                            return "";
                        }
                    }
                }
            }
            string speaker1 = speaker1Method();       //Assigning value for neck pickup
            string speaker2Method()
            {
                try
                {
                    string sp2Temp = grubContent("&Vcy;&CHcy; &dcy;&rcy;&acy;&jcy;&vcy;&iecy;&rcy;");       //Getting value for bridge pickup
                    return sp2Temp;
                }
                catch
                {
                    try
                    {
                        string temp = grubContent("&Ncy;&CHcy; &icy;&zcy;&lcy;&ucy;&chcy;&acy;&tcy;&iecy;&lcy;&softcy;");
                        return temp;
                    }
                    catch
                    {
                        return "";
                    }
                }
            }
            string speaker2 = speaker2Method();     //Assigning value for bridge pickup
            string controlsMethod()
            {
                try
                {
                    string controlsTemp = grubContent("&Rcy;&ucy;&chcy;&kcy;&icy; &ucy;&pcy;&rcy;&acy;&vcy;&lcy;&iecy;&ncy;&icy;&yacy;");      //Getting value for controls
                    return controlsTemp;
                }
                catch
                {
                    return "";
                }
            }
            string controls = controlsMethod();      //Assigning value for controls
                                                     //string misc = grubContent("");          //Getting value for misc (to be changed?)
            string imageTemp = model.Replace(" ", "").Replace("/", "");
            //Regex.Replace(                  //creating temporary image variable and removing spaces
            //              model, @"\s+\/", "");l
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
            //Console.WriteLine(imageAddress);
            CsvLine ToCsv = new CsvLine();
            ToCsv.name = name;
            ToCsv.model = model;
            ToCsv.price = price.Replace(" ", "").Replace("р.", "");
            ToCsv.categories = "Звуковое оборудование > Пассивные акустические системы"; //для акустических систем
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
            /*@"Усилитель : " + trans.MyDecoding(amp) + "\n" +
                                "Мощность : " + trans.MyDecoding(power) + "\n" +
                                "Динамики : " + trans.MyDecoding(speaker1) + " " + trans.MyDecoding(speaker2) +  "\n" +
                                "Бренд : " + brand + "\n" +
                                "Частотный диапазон : " + trans.MyDecoding(response) + "\n" +
                                "Входы : " + trans.MyDecoding(input) + "\n" +
                                "Выходы : " + trans.MyDecoding(output) + "\n" +
                                "Сопротивление : " + trans.MyDecoding(ohm) + "\n" +
                                "Максимальный УЗД/SPL : " + trans.MyDecoding(spl) + "\n" +
                                "Чувствительность : " + trans.MyDecoding(sens) + "\n" +
                                "Габариты : " + trans.MyDecoding(dim) + "\n" +
                                "Вес : " + trans.MyDecoding(weight) + "\n" +
                            //    "Бридж : " + trans.MyDecoding(bridge) + "\n" +
                            //    "Звукосниматели : " + trans.MyDecoding(pickups1) + trans.MyDecoding(pickups2) + "\n" +
                            //    "Органы управления : " + HttpUtility.HtmlDecode(trans.MyDecoding(controls)) + "\n" +
                                "Прочее : ";*/
            ToCsv.attributes_group = attr_group();
            /*@"Акустические системы
Акустические системы
Акустические системы
Акустические системы
Акустические системы
Акустические системы
Акустические системы
Акустические системы
Акустические системы
Акустические системы
Акустические системы
Акустические системы
Акустические системы
Акустические системы";*/
            ToCsv.options = "";
            ToCsv.option_type = "";
            ToCsv.images = "/catalog/pls/" + image;
            CsvLine lister(string addr)
            {
                Grub(addr);
                return ToCsv;
            }
            return ToCsv;
        }
        public GrubberPassiveLoudspeakers(string Addr)
        {
            string addr = Addr;
        }
    }
}
