using System;
using System.Collections.Generic;
using AhoCorasick;

namespace ConsoleApp1
{
    class Program
    {
        // Have to make our own testing project since Resharper currently has a HUGE bug
        // which keeps it from being able to debug unit tests.
        static void Main()
        {
            var testInput = new List<string> { "a", "mel", "lon", "el", "An" };
            var testSearches = new List<string> { "Apple", "Melon", "Orange", "Watermelon" };

            Console.WriteLine("Test 1");
            var acs = AhoCorasickString.Create(testInput);
            foreach (var word in testSearches)
            {
                Console.WriteLine(word);
                Console.WriteLine("Parts {");
                foreach (var part in acs.LocateParts(word, true))
                {
                    Console.WriteLine($"\t{part.Position}: {part.Value}");
                }
                Console.WriteLine("}\n");
            }

            Console.WriteLine("Test 2");
            // Doesn't find "bc"
            testInput = new List<string> { "abcd", "bc" };
            testSearches = new List<string> { "abcd"};

            acs = AhoCorasickString.Create(testInput);
            foreach (var word in testSearches)
            {
                Console.WriteLine(word);
                Console.WriteLine("Parts {");
                foreach (var part in acs.LocateParts(word, true))
                {
                    Console.WriteLine($"\t{part.Position}: {part.Value}");
                }
                Console.WriteLine("}\n");
            }

            Console.WriteLine("Test 3");

            testInput = new List<string> { "a", "ab", "ab", "abc", "abcd", "bcdar", "ar", "arr", "arre" };
            testSearches = new List<string> { "abcd", "rrabcde", "darrell", "area", "abcdarrell" };

            acs = AhoCorasickString.Create(testInput);
            foreach (var word in testSearches)
            {
                Console.WriteLine(word);
                Console.WriteLine("Parts {");
                foreach (var part in acs.LocateParts(word, true))
                {
                    Console.WriteLine($"\t{part.Position}: {part.Value}");
                }
                Console.WriteLine("}\n");
            }
            Console.ReadKey();
        }
    }
}
