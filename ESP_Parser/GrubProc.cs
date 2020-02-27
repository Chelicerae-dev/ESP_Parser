using System;
using System.IO;
using System.Collections.Generic;
using CsvHelper;
using System.Globalization;
using System.Web;
using System.Net;


namespace ESP_Parser
{
    public class GrubProc
    {
        public void GrubProccess(string variant)
        {
            IList<CsvLine> records = new List<CsvLine>();
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
            switch (variant)
            {
                case "eguitars":
                    Console.WriteLine("Selected electric guitars. Time to grub!");

                    try
                    {
                        using (StreamReader sr = new StreamReader("EGuitarsLinks.txt"))
                        {
                            string line;
                            while ((line = sr.ReadLine()) != null)
                            {
                                GrubberEGuitars grubber = new GrubberEGuitars(line);

                                Console.WriteLine(line);
                                records.Add(grubber.Grub(line));
                                string model = grubber.Grub(line).model;
                                string img = grubber.grubImg(line);
                                try
                                {
                                    using (WebClient client = new WebClient())
                                    {
                                        client.DownloadFile(img, @"/Users/olimpinz/Projects/ESP_Parser/ESP_Parser/bin/Debug/netcoreapp3.1/img/" + model.Replace(" ", "").Replace("/", "") + ".png");
                                    }
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine("Unable to download image");
                                    Console.WriteLine(e.Message);
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
                    WriteCsv("EGuitars");
                    break;
                //case "guitar amps":

            }

        }
        public GrubProc()
        {
        }
    }
}
