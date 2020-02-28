using System;
using HtmlAgilityPack;
using System.IO;
using System.Net;
using System.Web;
using System.Text.RegularExpressions;

namespace ESP_Parser
{
    public class GrubberEGCombos
    {
        public string grubImg(string Address)
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
            var pageContent = LoadPage(Address);
            var document = new HtmlDocument();  //Creating new page to parse
            document.LoadHtml(pageContent);     //Creating new page to parse
            var nodeImgAddr = document.DocumentNode.SelectSingleNode("//a[@data-fancybox-group=\"gallery\"]");
            string imgLink = "https://www.guitar-world.ru" + nodeImgAddr.Attributes["href"].Value;
            Console.WriteLine(imgLink);
            return imgLink;
        }
        public CsvLine Grub(string addr)
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
                                                ///////////
            ///
            string grubPrice(string labelPrice)       //labelPrice = "product-price" for Guitar-World
            {
                try
                {
                    var nodePrice = document.DocumentNode.SelectSingleNode("//span[@class=\"" + labelPrice + "\"]");
                    var nodeCheck = nodePrice.ChildNodes;
                    Console.WriteLine(nodeCheck.Count);
                    Console.WriteLine(nodeCheck);
                    //Console.WriteLine(nodeCheck.NextSibling.Name);
                    //Console.WriteLine(nodeCheck.NextSibling.InnerText);
                    switch (nodeCheck.Count)
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
                    }
                }
                catch
                {
                    Console.WriteLine("Unable to grub price!");
                    return "";
                }

            }
            string grubModel(string labelModel)       //labelModel = "Артикул:" for Guitar-World
            {
                var nodeModel = document.DocumentNode.SelectSingleNode("//div[label='" + labelModel + "']");
                var nodePModel = nodeModel.SelectSingleNode("p");
                string textModel = nodePModel.InnerText;
                Console.WriteLine(textModel);
                return textModel;
            }
            string grubName(string Name)            //Name = "header-for-light" for Guitar-World
            {
                var nodeName = document.DocumentNode.SelectSingleNode("//div[@class=\"" + Name + "\"]");
                var nodeH1Name = nodeName.SelectSingleNode("h1");
                string nodeHName = nodeH1Name.InnerText;
                Console.WriteLine(nodeHName);
                return Regex.Replace(              //removing whitespaces
                    nodeHName, @"^\s+|\s+$", "");
            }
            string grubImg(string ImgAddr)
            {
                var nodeImgAddr = document.DocumentNode.SelectSingleNode("//a[@data-fancybox-group=\"" + ImgAddr + "\"]");
                string imgLink = "https://www.guitar-world.ru" + nodeImgAddr.Attributes["href"].Value;
                Console.WriteLine(imgLink);
                Console.WriteLine(nodeImgAddr.Attributes["href"].Value);
                return imgLink;
            }

            ///////////
            string brand = grubBrand("Производитель:");         //Getting value for brand
            string powerMethod()
            {
                try
                {
                    string temp = grubContent("&Mcy;&ocy;&shchcy;&ncy;&ocy;&scy;&tcy;&softcy; RMS"); //Getting value for wattage
                    return temp;
                }
                catch
                {
                    return "";
                }
            }
            string power = powerMethod(); //Assigning value for wattage
            string weightMethod()
            {
                try
                {
                    string temp = grubContent("&Vcy;&iecy;&scy;");    //getting value for body wood
                    return temp;
                }
                catch
                {
                    return "";
                }
            }

            string weight = weightMethod();    //Assigning value for body wood
            string dimMethod()
            {
                try

                {
                    string temp = grubContent("&Vcy;&iecy;&rcy;&khcy; &kcy;&ocy;&rcy;&pcy;&ucy;&scy;&acy;");       //Getting value for dimensions
                    return temp;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return "-";
                }
            }
            string dim = dimMethod();   //Assigning value for dimensions
            /////////////////////////////////////////////////////////////////////
            string constructionMethod()
            {
                try
                {
                    string temp = grubContent("&Rcy;&acy;&zcy;&mcy;&iecy;&rcy;&ycy;");  //Getting value for 
                    return temp;
                }
                catch
                {
                    return "";
                }
            }
            string construction = constructionMethod();  //Assigning value for neck construction
            string scaleMethod()
            {
                try
                {
                    string temp = grubContent("&Mcy;&iecy;&ncy;&zcy;&ucy;&rcy;&acy;");         //Getting value for scale lenght
                    return temp;
                }
                catch
                {
                    return "";
                }
            }
            string scale = scaleMethod();         //Assigning value for scale lenght
            string neckwoodMethod()
            {
                try
                {
                    string temp = grubContent("&Gcy;&rcy;&icy;&fcy;");      //Getting value for neck wood
                    return temp;
                }
                catch
                {
                    return "";
                }
            }
            string neckWood = neckwoodMethod();      //Assigning value for neck wood

            string fretboardMethod()
            {
                try
                {
                    string fretboardTemp = grubContent("&Ncy;&acy;&kcy;&lcy;&acy;&dcy;&kcy;&acy; &gcy;&rcy;&icy;&fcy;&acy;");     //Getting value for fretboard wood
                    return fretboardTemp;
                }
                catch (Exception e)
                {
                    {
                        try
                        {
                            Console.WriteLine("Failed a bit with fretboard node type" + e.Message);
                            string fretboardTemp = grubContent("&Ncy;&acy;&kcy;&lcy;&acy;&dcy;&kcy;&acy; &ncy;&acy; &gcy;&rcy;&icy;&fcy;");
                            return fretboardTemp;
                        }
                        catch
                        {
                            return "";
                        }
                    }
                }
            }
            string fretboard = fretboardMethod();   //Assigning value for fretboard wood
                                                    ///////
            string name = grubName("header-for-light");        //Name of guitar
            string price = grubPrice("product-price");
            string model = grubModel("Артикул:");
            //////
            string fretsMethod()
            {
                try
                {
                    string fretsTemp = grubContent("&Lcy;&acy;&dcy;&ycy;"); //Getting value for number of frets
                    return fretsTemp;
                }
                catch
                {
                    string fretsTemp = "";
                    return fretsTemp;
                }
            }
            string numberOfFrets = fretsMethod(); //Assigning to numberOfFrets
            string colorMethod()
            {
                try
                {
                    string colorTemp = grubContent("&TScy;&vcy;&iecy;&tcy;");         //Getting value for color
                    return colorTemp;
                }
                catch
                {
                    try
                    {
                        Console.WriteLine("Color variant 2");
                        string colorTemp = grubContent("&TScy;&vcy;&iecy;&tcy; &kcy;&ocy;&rcy;&pcy;&ucy;&scy;&acy;");
                        return colorTemp;
                    }
                    catch
                    {
                        return "";
                    }
                }
            }
            string color = colorMethod();    //Assigning value for color
            string bridgeMethod()
            {
                try
                {
                    string bridgeTemp = grubContent("&Bcy;&rcy;&icy;&dcy;&zhcy;");        //Getting value for bridge type
                    return bridgeTemp;
                }
                catch
                {
                    return "";
                }
            }
            string bridge = bridgeMethod();        //Assigning value for bridge type
            string pickups1Method()
            {
                try
                {
                    string pu1Temp = grubContent("&Dcy;&acy;&tcy;&chcy;&icy;&kcy; &ucy; &gcy;&rcy;&icy;&fcy;&acy;");       //Getting value for neck pickup
                    return pu1Temp;
                }
                catch
                {
                    return "";
                }
            }
            string pickups1 = pickups1Method();       //Assigning value for neck pickup
            string pickups2Method()
            {
                try
                {
                    string pu2Temp = grubContent("&Dcy;&acy;&tcy;&chcy;&icy;&kcy; &ucy; &bcy;&rcy;&icy;&dcy;&zhcy;&acy;");       //Getting value for bridge pickup
                    return pu2Temp;
                }
                catch
                {
                    return "";
                }
            }
            string pickups2 = pickups2Method();     //Assigning value for bridge pickup
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
            //              model, @"\s+\/", "");
            string image = imageTemp + ".png";
            Transliterate trans = new Transliterate();   //Initializing Transliterate class for further usage
                                                         //Console.WriteLine(trans.MyDecoding(body));   //
            string imageAddr()
            {
                try
                {
                    string temp = grubImg("gallery");
                    return temp;
                }
                catch
                {
                    Console.WriteLine("Unable to grub img");
                    return "";
                }
            }
            string imageAddress = imageAddr();
            Console.WriteLine(imageAddress);
            CsvLine ToCsv = new CsvLine();
            ToCsv.name = name;
            ToCsv.model = model;
            ToCsv.price = price;
            ToCsv.categories = "Гитары > Бас-гитары"; //для бас-гитар
            ToCsv.quantity = 2;
            ToCsv.manufacturer = brand;
            ToCsv.description = desc.InnerText;
            /*ToCsv.attributes = @"Количество струн : " +  + "\n" +
                                    "Материал корпуса : " + trans.MyDecoding() + "\n" +
                                    "Бренд : " + brand + "\n" +
                                    "Материал топа : " + trans.MyDecoding() + "\n" +
                                    "Крепление грифа : " + trans.MyDecoding() + "\n" +
                                    "Мензура : " + trans.MyDecoding() + "\n" +
                                    "Материал грифа : " + trans.MyDecoding() + "\n" +
                                    "Материал накладки : " + trans.MyDecoding() + "\n" +
                                    "Количество ладов : " + trans.MyDecoding() + "\n" +
                                    "Цвет : " + trans.MyDecoding() + "\n" +
                                    "Бридж : " + trans.MyDecoding() + "\n" +
                                    "Звукосниматели : " + trans.MyDecoding()  + "\n" +
                                    "Органы управления : " + HttpUtility.HtmlDecode(trans.MyDecoding()) + "\n" +
                                    "Прочее : ";
            /*Тип усилителя
             *Петля эффектов
             *Лампы в оконечнике
             *Мощность, RMS, Вт
             *Канал 2
             *Канал 1
             *Каналов
             *Выходы
             *Входы
             *Бренд
             *D.I выход
             *Динамики
             *Управление
             *Лампы в преампе
             *Тип
             *Вес
             *Габариты
             *Прочее
             */
            ToCsv.attributes_group = @"Гитарное усиление
Гитарное усиление
Гитарное усиление
Гитарное усиление
Гитарное усиление
Гитарное усиление
Гитарное усиление
Гитарное усиление
Гитарное усиление
Гитарное усиление
Гитарное усиление
Гитарное усиление
Гитарное усиление
Гитарное усиление";
            ToCsv.options = "";
            ToCsv.option_type = "";
            ToCsv.images = "/catalog/ebasses/" + image;
            CsvLine lister(string addr)
            {
                Grub(addr);
                return ToCsv;
            }
            return ToCsv;
        }
        public GrubberEGCombos()
        {
        }
    }
}
