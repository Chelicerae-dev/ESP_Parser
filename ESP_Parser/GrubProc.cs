using System;
using System.IO;
using System.Collections.Generic;
using CsvHelper;
using System.Globalization;
using System.Net;

namespace ESP_Parser
{
    public class GrubProc
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
        public void GrubProccess(string variant)
        {
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
                                            client.DownloadFile(img, @"/Users/olimpinz/Projects/ESP_Parser/ESP_Parser/bin/Debug/netcoreapp3.1/eguitars/" + model.Replace(" ", "").Replace("/", "") + ".png");
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
                        //Console.ReadLine();
                    case "ebass":
                        Console.WriteLine("Selected electric basses. Time to grub!");

                        try
                        {
                            using (StreamReader sr = new StreamReader("EBassLinks.txt"))
                            {
                                int count = 1;
                                string line;
                                while ((line = sr.ReadLine()) != null)
                                {
                                    GrubberEBass grubber = new GrubberEBass(line);
                                    Console.WriteLine(count);
                                    count++;
                                    Console.WriteLine(line);
                                    records.Add(grubber.Grub(line));
                                    string model = grubber.Grub(line).model;
                                    string img = grubber.grubImg(line);
                                    try
                                    {
                                        using (WebClient client = new WebClient())
                                        {
                                            client.DownloadFile(img, @"/Users/olimpinz/Projects/ESP_Parser/ESP_Parser/bin/Debug/netcoreapp3.1/ebass/" + model.Replace(" ", "").Replace("/", "") + ".png");
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
                        WriteCsv("EBass");
                        break;
                    case "ls":
                        Console.WriteLine("Selected loudspeaker systems. Time to grub!");

                        try
                        {
                            using (StreamReader sr = new StreamReader("LSLinks.txt"))
                            {
                                int count = 1;
                                string line;
                                while ((line = sr.ReadLine()) != null)
                                {
                                    GrubberLoudspeakers grubber = new GrubberLoudspeakers(line);
                                    Console.WriteLine("-------------------" + count + "--------------------");
                                    count++;
                                    Console.WriteLine(line);
                                    records.Add(grubber.Grub(line));
                                    string model = grubber.Grub(line).model;
                                    string img = grubber.grubImg(line);
                                    Console.WriteLine("Model is " + model + " and it goes to img");
                                    try
                                    {
                                        using (WebClient client = new WebClient())
                                        {
                                            client.DownloadFile(img, @"/Users/olimpinz/Projects/ESP_Parser/ESP_Parser/bin/Debug/netcoreapp3.1/ls/" + model.Replace(" ", "").Replace("/", "") + ".png");
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine("Unable to download image");
                                        Console.WriteLine(e.Message);
                                        try
                                        {
                                            using (StreamWriter sw = new StreamWriter("imgLinks.txt", append: true))
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
                        WriteCsv("LS");
                        break;
                    case "pls":
                        Console.WriteLine("Selected passive loudspeaker systems. Time to grub!");

                        try
                        {
                            using (StreamReader sr = new StreamReader("PLSLinks.txt"))
                            {
                                int count = 1;
                                string line;
                                while ((line = sr.ReadLine()) != null)
                                {
                                    GrubberPassiveLoudspeakers grubber = new GrubberPassiveLoudspeakers(line);
                                    Console.WriteLine("-------------------" + count + "--------------------");
                                    count++;
                                    Console.WriteLine(line);
                                    records.Add(grubber.Grub(line));
                                    string model = grubber.Grub(line).model;
                                    string img = grubber.grubImg(line);
                                    //Console.WriteLine("Model is " + model + " and it goes to img");
                                    try
                                    {
                                        using (WebClient client = new WebClient())
                                        {
                                            client.DownloadFile(img, @"/Users/olimpinz/Projects/ESP_Parser/ESP_Parser/bin/Debug/netcoreapp3.1/pls/" + model.Replace(" ", "").Replace("/", "") + ".png");
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine("Unable to download image");
                                        Console.WriteLine(e.Message);
                                        try
                                        {
                                            using (StreamWriter sw = new StreamWriter("imgLinks.txt", append: true))
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
                        WriteCsv("PLS");
                        return;
                    case "mixers":
                        Console.WriteLine("Selected mixers. Time to grub!");

                        try
                        {
                            using (StreamReader sr = new StreamReader("MixersLinks.txt"))
                            {
                                int count = 1;
                                string line;
                                while ((line = sr.ReadLine()) != null)
                                {
                                    GrubberMixers grubber = new GrubberMixers(line);
                                    Console.WriteLine("-------------------" + count + "--------------------");
                                    count++;
                                    Console.WriteLine(line);
                                    records.Add(grubber.Grub(line));
                                    string model = grubber.Grub(line).model;
                                    string img = grubber.grubImg(line);
                                    Console.WriteLine("Model is " + model + " and it goes to img");
                                    try
                                    {
                                        using (WebClient client = new WebClient())
                                        {
                                            client.DownloadFile(img, @"/Users/olimpinz/Projects/ESP_Parser/ESP_Parser/bin/Debug/netcoreapp3.1/mixers/" + model.Replace(" ", "").Replace("/", "") + ".png");
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine("Unable to download image");
                                        Console.WriteLine(e.Message);
                                        try
                                        {
                                            using (StreamWriter sw = new StreamWriter("imgLinks.txt", append: true))
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
                        WriteCsv("Mixers");
                        break;
                case "exit":
                        Console.WriteLine("Bye!");
                        //status = false;
                        return;
                case "mics":
                    SubGrub mics = new SubGrub("mics");
                    mics.Grub("mics");
                    break;
                case "headphones":
                    SubGrub headphones = new SubGrub("headphones");
                    headphones.Grub("headphones");
                    break;
                case "monitors":
                    SubGrub monitors = new SubGrub("monitors");
                    monitors.Grub("monitors");
                    break;
                case "egc":
                    SubGrub egc = new SubGrub("egc");
                    egc.Grub("egc");
                    break;
                case "egh":
                    SubGrub egh = new SubGrub("egh");
                    egh.Grub("egh");
                    goto case "ebc";
                case "ebc":
                    SubGrub ebc = new SubGrub("ebc");
                    ebc.Grub("ebc");
                    goto case "ebh";
                case "ebh":
                    SubGrub ebh = new SubGrub("ebh");
                    ebh.Grub("ebh");
                    goto case "egp";
                case "egp":
                    SubGrub egp = new SubGrub("egp");
                    egp.Grub("egp");
                    goto case "ebp";
                case "ebp":
                    SubGrub ebp = new SubGrub("ebp");
                    ebp.Grub("ebp");
                    break;
                case "ags":
                    SubGrub ags = new SubGrub("ags");
                    ags.Grub("ags");
                    break;
                case "aguitars":
                    SubGrub aguitars = new SubGrub("aguitars");
                    aguitars.Grub("aguitars");
                    break;
                case "amps":
                    SubGrub amps = new SubGrub("amps");
                    amps.Grub("amps");
                    break;
                case "cgs":
                    SubGrub cgs = new SubGrub("cgs");
                    cgs.Grub("cgs");
                    break;
                case "cguitars":
                    SubGrub cguitars = new SubGrub("cguitars");
                    cguitars.Grub("cguitars");
                    break;
                case "gcables":
                    SubGrub gcables = new SubGrub("gcables");
                    gcables.Grub("gcables");
                    break;
                case "gcases":
                    SubGrub gcases = new SubGrub("gcases");
                    gcases.Grub("gcases");
                    break;
                case "gpickups":
                    SubGrub gpickups = new SubGrub("gpickups");
                    gpickups.Grub("gpickups");
                    break;
                case "picks":
                    SubGrub picks = new SubGrub("picks");
                    picks.Grub("picks");
                    break;
                case "straps":
                    SubGrub straps = new SubGrub("straps");
                    straps.Grub("straps");
                    break;
                case "tuners":
                    SubGrub tuners = new SubGrub("tuners");
                    tuners.Grub("tuners");
                    break;

            }
            //}
            //while (status == true);
        }
        public GrubProc()
        {
        }
    }
}
