using System;
using System.IO;
using System.Collections.Generic;
using CsvHelper;
using System.Globalization;
using System.Net;
using System.Text.RegularExpressions;

namespace ESP_Parser
{
    public class SubGrub
    {
        public IList<CsvLine> records = new List<CsvLine>();
        void WriteCsv(string selected)
        {
            try
            {
                using (var sw = new StreamWriter(selected + ".csv", append: true))
                using (var cw = new CsvWriter(sw, CultureInfo.InvariantCulture))
                {
                    cw.WriteRecords(records);
                    records.Clear();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
            public void Grub(string selected)
        {
            Console.WriteLine("Selected " + selected + " systems. Time to grub!");

            try
            {
                using (StreamReader sr = new StreamReader(selected + "Links.txt"))
                {
                    int count = 1;
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        GrubAmplifier grubber = new GrubAmplifier();
                        switch (selected)
                        {
                            case "ls":
                                grubber.Category = "Звуковое оборудование > Активные акустические системы";
                                grubber.AttrGroup = "Акустические системы";
                                grubber.imgPath = selected;
                                break;
                            case "pls":
                                grubber.Category = "Звуковое оборудование > Пассивные акустические системы";
                                grubber.AttrGroup = "Акустические системы";
                                grubber.imgPath = selected;
                                break;
                            case "mixers":
                                grubber.Category = "Звуковое оборудование > Микшеры";
                                grubber.AttrGroup = "Микшеры";
                                grubber.imgPath = selected;
                                break;
                            case "mics":
                                grubber.Category = "Микрофоны и радиосистемы";
                                grubber.AttrGroup = "Микрофоны";
                                grubber.imgPath = selected;
                                break;
                            case "headphones":
                                grubber.Category = "Звуковое оборудование > Наушники";
                                grubber.AttrGroup = "Наушники";
                                grubber.imgPath = selected;
                                break;
                            case "monitors":
                                grubber.Category = "Студийное оборудование > Студийные мониторы";
                                grubber.AttrGroup = "Студийные мониторы";
                                grubber.imgPath = selected;
                                break;
                            default:
                                break;
                        }
                        Console.WriteLine("-------------------" + count + "--------------------");
                        count++;
                        Console.WriteLine(line);
                        grubber.addr = line;
                        records.Add(grubber.WriteCsv());
                        string model = grubber.Grub()[1];
                        string img = grubber.grubImg(line);
                        Console.WriteLine("Артикул " + model);
                        Console.WriteLine("Название " + grubber.Grub()[0]);
                        Console.WriteLine("Цена " + grubber.Grub()[2]);
                        Console.WriteLine("Бренд " + grubber.Grub()[3]);
                        //Console.WriteLine("Model is " + model + " and it goes to img");
                        try
                        {
                            using (WebClient client = new WebClient())
                            {
                                client.DownloadFile(img, @"/Users/olimpinz/Projects/ESP_Parser/ESP_Parser/bin/Debug/netcoreapp3.1/" + selected + "/" + Regex.Replace(model, @"\s|\/", "") + ".png");
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Unable to download image");
                            Console.WriteLine(e.Message);
                            try
                            {
                                using (StreamWriter sw = new StreamWriter(selected + "imgLinks.txt", append: true))
                                {
                                    sw.WriteLine(img);
                                }
                            }
                            catch (Exception f)
                            {
                                Console.WriteLine(f.Message);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Error reading file");
                throw;
            }
            WriteCsv(selected);
        }
        public SubGrub(string Selected)
        {
            string selected = Selected;
        }
    }
}
