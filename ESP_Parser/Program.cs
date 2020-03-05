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
ebass for electric basses (requires EBassLinks.txt);
guitar amps for guitar amps (requires GuitarAmpsLinks.txt);
aguitars for acoustic guitars (requires AGuitarsLinks.txt);
ls for active loudspeakers (requires LSLinks.txt);
pls for passive loudspeakers (requires PLSLinks.txt);
");
            
            string variant = Console.ReadLine();
            GrubProc grub = new GrubProc();
            grub.GrubProccess(variant);
            Console.WriteLine("Done");

        }
    }
}