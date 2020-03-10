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
                            case "egc":
                                grubber.Category = "Всё для гитар > Гитарные комбо";
                                grubber.AttrGroup = "Гитарное усиление";
                                grubber.imgPath = selected;
                                grubber.SiteSelect("guitar-world");
                                Console.WriteLine("Electric guitar combos from guitar-world");
                                break;
                            case "egh":
                                grubber.Category = "Всё для гитар > Гитарные усилители и кабинеты";
                                grubber.AttrGroup = "Гитарное усиление";
                                grubber.imgPath = selected;
                                grubber.SiteSelect("guitar-world");
                                Console.WriteLine("Electric guitar heads from guitar-world");
                                break;
                            case "ebc":
                                grubber.Category = "Всё для гитар > Басовые комбо";
                                grubber.AttrGroup = "Гитарное усиление";
                                grubber.imgPath = selected;
                                grubber.SiteSelect("guitar-world");
                                Console.WriteLine("Electric bass combos from guitar-world");
                                break;
                            case "ebh":
                                grubber.Category = "Всё для гитар > Басовые усилители и кабинеты";
                                grubber.AttrGroup = "Гитарное усиление";
                                grubber.imgPath = selected;
                                grubber.SiteSelect("guitar-world");
                                Console.WriteLine("Electric guitar combos from guitar-world");
                                break;
                            case "egp":
                                grubber.Category = "Всё для гитар > Педали эффектов";
                                grubber.AttrGroup = "Гитарное усиление";
                                grubber.imgPath = selected;
                                grubber.SiteSelect("guitar-world");
                                Console.WriteLine("Electric guitar pedals from guitar-world");
                                break;
                            case "ebp":
                                grubber.Category = "Всё для гитар > Педали эффектов";
                                grubber.AttrGroup = "Гитарное усиление";
                                grubber.imgPath = selected;
                                grubber.SiteSelect("guitar-world");
                                Console.WriteLine("Electric bass pedals from guitar-world");
                                break;
                            case "ags":
                                grubber.Category = "Всё для гитар > Струны для акустических гитар";
                                grubber.AttrGroup = "Струны";
                                grubber.imgPath = selected;
                                grubber.SiteSelect("guitar-world");
                                Console.WriteLine("Acoustic strings from guitar-world");
                                break;
                            case "aguitars":
                                grubber.Category = "Гитары > Акустические гитары";
                                grubber.AttrGroup = "Акустические гитары";
                                grubber.imgPath = selected;
                                grubber.SiteSelect("guitar-world");
                                Console.WriteLine("Acoustic guitars from guitar-world");
                                break;
                            case "amps":
                                grubber.Category = "Звуковое оборудование > Усилители";
                                grubber.AttrGroup = "Усилители";
                                grubber.imgPath = selected;
                                Console.WriteLine("Amps from Amplifier");
                                break;
                            case "cguitars":
                                grubber.Category = "Гитары > Классические гитары";
                                grubber.AttrGroup = "Акустические гитары";
                                grubber.imgPath = selected;
                                grubber.SiteSelect("guitar-world");
                                Console.WriteLine("Classic guitars from guitar-world");
                                break;
                            case "cgs":
                                grubber.Category = "Всё для гитар > Струны для классических гитар";
                                grubber.AttrGroup = "Струны";
                                grubber.imgPath = selected;
                                grubber.SiteSelect("guitar-world");
                                Console.WriteLine("Classic strings from guitar-world");
                                break;
                            case "gcables":
                                grubber.Category = "Всё для гитар > Гитарные провода";
                                grubber.AttrGroup = "Коммутация";
                                grubber.imgPath = selected;
                                grubber.SiteSelect("guitar-world");
                                Console.WriteLine("Guitar cables from guitar-world");
                                break;
                            case "gcases":
                                grubber.Category = "Всё для гитар > Чехлы и кейсы";
                                grubber.AttrGroup = "Струны";
                                grubber.imgPath = selected;
                                grubber.SiteSelect("guitar-world");
                                Console.WriteLine("Bags & cases from guitar-world");
                                break;
                            case "gpickups":
                                grubber.Category = "Всё для гитар > Звукосниматели";
                                grubber.AttrGroup = "Звукосниматели";
                                grubber.imgPath = selected;
                                grubber.SiteSelect("guitar-world");
                                Console.WriteLine("Pickups from guitar-world");
                                break;
                            case "picks":
                                grubber.Category = "Всё для гитар > Медиаторы";
                                grubber.AttrGroup = "Медиаторы";
                                grubber.imgPath = selected;
                                grubber.SiteSelect("guitar-world");
                                Console.WriteLine("Picks from guitar-world");
                                break;
                            case "straps":
                                grubber.Category = "Всё для гитар > Ремни";
                                grubber.AttrGroup = "Ремни";
                                grubber.imgPath = selected;
                                grubber.SiteSelect("guitar-world");
                                Console.WriteLine("Straps from guitar-world");
                                break;
                            case "tuners":
                                grubber.Category = "Всё для гитар > Тюнеры и метрономы";
                                grubber.AttrGroup = "Тюнеры";
                                grubber.imgPath = selected;
                                grubber.SiteSelect("guitar-world");
                                Console.WriteLine("Tuners from guitar-world");
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
