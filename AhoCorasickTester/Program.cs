using System;
using System.Collections.Generic;
using System.Linq;
using AhoCorasick;

namespace ConsoleApp1
{
    class Program
    {
        // Have to make our own testing project since Resharper currently has a HUGE bug
        // which keeps it from being able to debug unit tests.
        static void Main(string[] args)
        {
            var testInput = new List<string> { "a", "mel", "lon", "el", "An" };
            var testSearches = new List<string> { "Apple", "Melon", "Orange", "Watermelon" };

            var acs = AhoCorasickString.Create(testInput);
            foreach (var word in testSearches)
            {
                Console.WriteLine(word);
                Console.WriteLine("Parts {");
                foreach (var part in acs.LocateParts(word))
                {
                    Console.WriteLine($"\t{part}");
                }
                Console.WriteLine("}\n");
            }
            Console.ReadKey();
        }
    }
}
