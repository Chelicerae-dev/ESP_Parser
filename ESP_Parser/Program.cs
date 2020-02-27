using System;
using System.IO;
using System.Collections.Generic;
using CsvHelper;
using System.Globalization;


namespace ESP_Parser
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.WriteLine(@"Enter grubbing variant. Variants availible:
eguitars for electic guitars (requires EGuitarsLinks.txt);
guitar amps for guitar amps (requires GuitarAmpsLinks.txt);
aguitars for acoustic guitars (requires AGuitarsLinks.txt);                              
");
            
            string variant = Console.ReadLine();
            GrubProc grub = new GrubProc();
            grub.GrubProccess(variant);
            Console.WriteLine("Done");
        }
    }
}


/*IList<CsvLine> records = new List<CsvLine>();
            try
            {
                using (StreamReader sr = new StreamReader("links.txt"))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        GrubberEGuitars grubber = new GrubberEGuitars(line);

                        Console.WriteLine(line);
                        records.Add(grubber.Grub(line));                     
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Error reading file");
                throw;
            }
            try
            {
                using (var sw = new StreamWriter("esp.csv", append: true))
                using (var cw = new CsvWriter(sw, CultureInfo.InvariantCulture))
                {
                    cw.WriteRecords(records);
                    Console.WriteLine(grubber.line + "written.");
                    records.Clear();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }*/
