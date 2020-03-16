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
        public string Category { get; set; }     //Goods category 
        public string AttrGroup { get; set; }    //goes to Attributes_Group in CSV
        public string imgPath { get; set; }         //goes to image path in csv
        //
        public string SiteAddress { get; set; }
        protected string TableNodeXPath { get; set; }
        protected string SpecNameNodeXPath { get; set; }
        protected string SpecValueNodeXPath { get; set; }
        protected string BrandNodeXPath { get; set; }
        protected string DescNodeXPath { get; set; }
        protected string FeaturesNodeXpath { get; set; }
        protected string PriceNodeXPath { get; set; }
        protected string ModelNodeXPath { get; set; }
        protected string NameNodeXPath { get; set; }
        protected string NodeByIdXPath { get; set; }
        protected string ImgXPath { get; set; }
        protected string AvailXPath { get; set; }
        public void SiteSelect(string site)
        {
            switch (site)
                {
                case "amplifier": 
                    SiteAddress = "https://www.amplifier.ru";
                    TableNodeXPath = "//table[@class=\"shop_attributes\"]";
                    SpecNameNodeXPath = "tr/td[@itemprop=\"name\"]";
                    SpecValueNodeXPath = "tr/td[@itemprop=\"value\"]";
                    BrandNodeXPath = "//b[@id=\"prodbrand\"]";
                    DescNodeXPath = "tab-fullDescriptionProd";
                    FeaturesNodeXpath = "tab-featuresProd";
                    PriceNodeXPath = "//span[@itemprop=\"price\"]";
                    ModelNodeXPath = "//span[@itemprop=\"model\"]";
                    NameNodeXPath = "//h1[@id=\"prodtitle\"]";
                    NodeByIdXPath = "//div[@id=\"";
                    ImgXPath = "//img[@class=\"image_0\"]";
                    AvailXPath = "//div[@id=\"prodavailability\"]/span";
                    break;
                case "guitar-world":
                    SiteAddress = "https://www.guitar-world.ru";
                    TableNodeXPath = "//table[@class=\"specificProd\"]"; //
                    SpecNameNodeXPath = "tr/th[@itemprop=\"name\"]";    //
                    SpecValueNodeXPath = "tr/td[@itemprop=\"value\"]";  //
                    BrandNodeXPath = "//div[label=\"Производитель:\"]/p";   //
                    DescNodeXPath = "descr";    //
                    FeaturesNodeXpath = "tab-featuresProd"; //???
                    PriceNodeXPath = "//span[@class=\"product-price\"]"; //
                    ModelNodeXPath = "//div[label=\"Артикул:\"]/p";     //
                    NameNodeXPath = "//div[@class=\"header-for-light\"]/h1";  //header-for-light
                    NodeByIdXPath = "//div[@id=\"";
                    ImgXPath = "//img[@class=\"image_0\"]";
                    AvailXPath = "//span[@class=\"green\"]";
                    break;
                case "grandpianos":
                    SiteAddress = "https://www.grandpianos.ru";
                    TableNodeXPath = "//div[@class=\"tech textpage\"]";
                    SpecNameNodeXPath = "ul/li";
                    SpecValueNodeXPath = null;
                    BrandNodeXPath = "//*[@id=\"tovar\"]/div[2]/div[1]/div/p[1]/text()";
                    DescNodeXPath = "//div[@class=\"des textpage tpl-variable-part\"]";
                    FeaturesNodeXpath = null;
                    ModelNodeXPath = "//h1[@class=\"title tpl-variable-part\"]";
                    NameNodeXPath = "//h1[@class=\"title tpl-variable-part\"]";
                    NodeByIdXPath = "//div[@id=\"";
                    ImgXPath = "/html/body/div[4]/section/div/section[2]/div[1]/div[1]/div/ul/li[1]/a/img";
                    PriceNodeXPath = "/html/body/div[4]/section/div/section[2]/div[2]/div[1]/div/form/div[1]/span[1]/text()";
                    break;
            }   
        }
        protected string LoadPage(string addr)     //HtmlAgilityPack initial page load module
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


        public virtual string grubImg(string addr)
        {
            var pageContent = LoadPage(addr);
            var document = new HtmlDocument();  //Creating new page to parse
            document.LoadHtml(pageContent);     //Creating new page to parse
            var nodeImgAddr = document.DocumentNode.SelectSingleNode(ImgXPath);
            //Console.WriteLine(nodeImgAddr.Attributes["src"].Value);
            try
            {
                string imgLink = SiteAddress + nodeImgAddr.GetAttributeValue("src", "default");
                //string imgLink = SiteAddress + nodeImgAddr.Attributes["src"].Value;
                return imgLink;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey();
                return "";
            }
        }
        
        public virtual string[] Grub() 
        {
            string[] temp = new string[9];
            var pageContent = LoadPage(addr);
            var document = new HtmlDocument();  //Creating new page to parse
            document.LoadHtml(pageContent);     //Creating new page to parse

            HtmlNode grubId(string id)          //Guitar-World description node
            {
                var grubberId = document.DocumentNode.SelectSingleNode(NodeByIdXPath + id + "\"]");
                return grubberId;
            }
                       ////// Full specs table below
            List<string> grubSpecs = new List<string>();    //making list for all specs availible (which are different at every page
            {
                try
                {
                    var TableNode = document.DocumentNode.SelectSingleNode(TableNodeXPath).SelectSingleNode("tbody");
                    var SpecName = TableNode.SelectNodes(SpecNameNodeXPath);
                    var SpecValue = TableNode.SelectNodes(SpecValueNodeXPath);
                    int i = 0;
                    foreach (HtmlNode count in SpecName)
                    {
                        grubSpecs.Add(Regex.Replace(SpecName[i].InnerText, @"^\s+|\s+$|\n", "") + " : " + Regex.Replace(SpecValue[i].InnerText, @"^\s+|\s+$|\n", ""));
                        i++;
                    }
                }
                catch
                {
                    try
                    {
                        var TableNode = document.DocumentNode.SelectSingleNode(TableNodeXPath).SelectSingleNode("tbody");
                        var SpecName = TableNode.SelectNodes("tr/td/h5");
                        int i = 0;
                        foreach (HtmlNode count in SpecName)
                        {
                            grubSpecs.Add("Особенности : " + Regex.Replace(SpecName[i].InnerText, @"^\s+|\s+$|\n", ""));
                            i++;
                        }
                    }
                    catch
                    {
                        try //grandpianos case
                        {
                            var TableNode = document.DocumentNode.SelectSingleNode(TableNodeXPath);
                            var SpecLine = TableNode.SelectNodes(SpecNameNodeXPath);
                            int i = 0;
                            foreach (HtmlNode count in SpecLine)
                            {
                                string SpecName = Regex.Split(SpecLine[i].InnerText, @"\:")[0];
                                string SpecValue = Regex.Split(SpecLine[i].InnerText, @"\:")[1];
                                grubSpecs.Add(Regex.Replace(SpecName, @"^\s+|\s+$|\n", "") + " : " + Regex.Replace(SpecValue, @"^\s+|\s+$|\n", ""));
                                i++;
                            }
                        }
                        catch
                        {
                            Console.WriteLine("No specs!");
                        }
                    }
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
                try
                {
                    var nodeBrand = document.DocumentNode.SelectSingleNode(BrandNodeXPath);
                    //Console.WriteLine(nodeBrand.InnerText);
                    return nodeBrand.InnerText;
                }
                catch
                {
                    try
                    {
                        var nodeBrand = document.DocumentNode.SelectNodes(BrandNodeXPath).FindFirst("/i[\"Бренд: \"]" );
                        //nodeBrand.RemoveAllChildren();
                        return Regex.Replace(nodeBrand.InnerText, "\"", "");
                    }
                    catch
                    {
                        Console.WriteLine(document.DocumentNode.SelectSingleNode(BrandNodeXPath).InnerText);
                        Console.WriteLine("Too bad, no brand found");
                        return "";
                    }
                    /*Console.WriteLine("No brand");
                    return "";*/
                }
            }
            string Avail()
            {
                try  //amplifier and g-w
                {
                    string AvailClass = document.DocumentNode.SelectSingleNode(AvailXPath).GetAttributeValue("class", "None");
                    Console.WriteLine(AvailClass);
                    switch (AvailClass)
                    {
                        case "label_action success":    //amplifier - availible
                            return "2";
                        case "label_action warning":    //amplifier - not in stock
                            return "0";
                        case "label_action danger":    //amplifier - not in stock
                            return "0";
                        case "green":       //guitar-world - availible or in stock
                            return document.DocumentNode.SelectSingleNode(AvailXPath).InnerText == "есть в наличии" ? "2" : "0";
                        case "red":     //guitar-world - discontinued, not availible
                            return "0";
                        default:
                            return "0";
                    }

                }
                catch
                {
                    /*try  //guitar-world
                    {
                        string AvailCheck = document.DocumentNode.SelectSingleNode(AvailXPath).InnerText;
                        switch (AvailCheck)
                        {
                            case "есть в наличии":
                                Console.WriteLine("Availibility detected and set to 2");
                                return "2";                              
                            case "доступен под заказ":
                                Console.WriteLine("Availibility detected (not in stock) and set to 0");
                                return "0";
                            default:
                                return "0";
                        }

                    }
                    catch
                    {
                        Console.WriteLine("Availibility check failed");
                        return "0";
                    }*/
                    Console.WriteLine("Failed to get avail");
                    return "0";
                }

            }
            HtmlNode desc()
            {
                try
                {
                    var temp = grubId(DescNodeXPath);         //var with "description" part
                    return temp;
                }
                catch
                {
                    return null;
                }
            }

            HtmlNode featMethod()
            {
                try
                {
                    var temp = grubId(FeaturesNodeXpath);
                    return temp;
                }
                catch
                {
                    return null;
                }
            }
            var feat = featMethod();
            string grubPrice()       //getting price
            {
                if (SiteAddress == "https://www.amplifier.ru")
                {
                    try
                    {
                        var nodePrice = document.DocumentNode.SelectSingleNode(PriceNodeXPath);
                        return nodePrice.InnerText.Replace("\"", "").Replace(" ", "");
                    }
                    catch
                    {
                        Console.WriteLine("Unable to grub price!");
                        return "";
                    }
                }
                else if (SiteAddress == "https://www.guitar-world.ru")   //commented in case there'll be more sites
                {
                    try
                    {
                        var nodePrice = document.DocumentNode.SelectSingleNode(PriceNodeXPath);
                        var nodeCheck = nodePrice.ChildNodes;
                        //Console.WriteLine(nodeCheck.Count);
                        //Console.WriteLine(nodeCheck);
                        switch (nodeCheck.Count)
                        {
                            case 1:
                                //Console.WriteLine(nodePrice);                 //single span (no discount)
                                string textPrice = nodePrice.InnerText;
                                //Console.WriteLine(textPrice);
                                string temp = Regex.Replace(                  //removing spaces
                                    textPrice, @"\s|руб\.", "");
                                return temp;              //removing letters

                            case 3:                                           //multiple spans (discount)
                                var nodeSCheck = nodePrice.FirstChild.NextSibling;
                                string textDPrice = nodeSCheck.InnerText;
                                //Console.WriteLine(textDPrice);
                                string tempD = Regex.Replace(                  //removing spaces
                                    textDPrice, @"\s|руб\.", "");
                                return tempD;              //removing letters
                            default:
                                return "";
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Unable to grub price!");
                        return "";
                    }
                }
                else //if (SiteAddress == "https://www.grandpianos.ru")
                {
                    try
                    {
                        var nodePrice = document.DocumentNode.SelectSingleNode(PriceNodeXPath);
                        if (nodePrice.FirstChild != null)
                        {
                            nodePrice.RemoveAllChildren();
                        }
                        return nodePrice.InnerText.Replace("\"", "").Replace(" ", "");
                    }
                    catch
                    {
                        Console.WriteLine("Unable to grub price!");
                        return "";
                    }
                }
            }
            string grubModel()       //itemprop = "model" for Amplifier
            {
                var nodeModel = document.DocumentNode.SelectSingleNode(ModelNodeXPath);
                //Console.WriteLine(nodeModel.InnerText);
                return nodeModel.InnerText;
            }
            string grubName()            //h1 id = "prodtitle" for Amplifier
            {
                var nodeName = document.DocumentNode.SelectSingleNode(NameNodeXPath);
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
            if (desc() != null && feat != null)
            {
                temp[4] = desc().InnerHtml + "\n<h3>Особенности</h3>\n" + feat.InnerHtml;
            }
            else if (desc() != null)
            {
                temp[4] = desc().InnerHtml;
            }
            else
            {
                temp[4] = "";
            }
            temp[5] = (Specs() != null) ? trans.MyDecoding(Specs()) : "";
            temp[6] = attr_group();
            temp[7] = image;
            temp[8] = Avail();
            return temp;
        }
            public CsvLine WriteCsv()
            {
                CsvLine ToCsv = new CsvLine();
                ToCsv.name = WebUtility.HtmlDecode(Grub()[0]);
                ToCsv.model = Grub()[1];
                ToCsv.price = Grub()[2];
                ToCsv.categories = Category; //для микшеров
                ToCsv.quantity = Int32.Parse(Grub()[8]);
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
