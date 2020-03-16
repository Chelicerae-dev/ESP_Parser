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
                    SubGrub eguitars = new SubGrub("eguitars");
                    eguitars.Grub("eguitars");
                    break;

                /*Console.WriteLine("Selected electric guitars. Time to grub!");
                 * try
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
                break;*/
                //Console.ReadLine();
                case "ebass":
                    SubGrub ebass = new SubGrub("ebass");
                    ebass.Grub("ebass");
                    break;
                case "ls":
                    SubGrub ls = new SubGrub("ls");
                    ls.Grub("ls");
                    break;
                        /*Console.WriteLine("Selected loudspeaker systems. Time to grub!");

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
                        break;*/
                case "pls":
                    SubGrub pls = new SubGrub("pls");
                    pls.Grub("pls");
                    break;
                case "mixers":
                    SubGrub mixers = new SubGrub("mixers");
                    mixers.Grub("mixers");
                    break;
                case "exit":
                        Console.WriteLine("Bye!");
                        //status = false;
                        break;
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
                    goto case "aguitars";
                case "aguitars":
                    SubGrub aguitars = new SubGrub("aguitars");
                    aguitars.Grub("aguitars");
                    goto case "amps";
                case "amps":
                    SubGrub amps = new SubGrub("amps");
                    amps.Grub("amps");
                    goto case "cgs";
                case "cgs":
                    SubGrub cgs = new SubGrub("cgs");
                    cgs.Grub("cgs");
                    goto case "cguitars";
                case "cguitars":
                    SubGrub cguitars = new SubGrub("cguitars");
                    cguitars.Grub("cguitars");
                    goto case "gcables";
                case "gcables":
                    SubGrub gcables = new SubGrub("gcables");
                    gcables.Grub("gcables");
                    goto case "gcases";
                case "gcases":
                    SubGrub gcases = new SubGrub("gcases");
                    gcases.Grub("gcases");
                    break;
                case "gpickups":
                    SubGrub gpickups = new SubGrub("gpickups");
                    gpickups.Grub("gpickups");
                    goto case "picks";
                case "picks":
                    SubGrub picks = new SubGrub("picks");
                    picks.Grub("picks");
                    goto case "straps";
                case "straps":
                    SubGrub straps = new SubGrub("straps");
                    straps.Grub("straps");
                    break;
                case "tuners":
                    SubGrub tuners = new SubGrub("tuners");
                    tuners.Grub("tuners");
                    break;
                case "egs":
                    SubGrub egs = new SubGrub("egs");
                    egs.Grub("egs");
                    break;
                case "hp":
                    SubGrub hp = new SubGrub("hp");
                    hp.Grub("hp");
                    goto case "conf";
                case "conf":
                    SubGrub conf = new SubGrub("conf");
                    conf.Grub("conf");
                    goto case "ibp";
                case "ibp":
                    SubGrub ibp = new SubGrub("ibp");
                    ibp.Grub("ibp");
                    goto case "lss";
                case "lss":
                    SubGrub lss = new SubGrub("lss");
                    lss.Grub("lss");
                    goto case "speakers";
                case "speakers":
                    SubGrub speakers = new SubGrub("speakers");
                    speakers.Grub("speakers");
                    break;
                case "pop":
                    SubGrub pop = new SubGrub("pop");
                    pop.Grub("pop");
                    break;
                case "casioep":
                    SubGrub casioep = new SubGrub("casioep");
                    casioep.Grub("casioep");
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
