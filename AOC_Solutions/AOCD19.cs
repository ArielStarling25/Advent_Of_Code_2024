using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AOC_2024_Day1.AOC_Solutions
{
    public class AOCD19
    {
        private static readonly string inputFile = "C:\\Users\\user\\Desktop\\Ariel's Folder\\Training\\AdventOfCode_2024\\AOC_2024_Day1\\AOC_Resources\\AOCD19.txt";

        private List<string> towels = new List<string>();
        private List<Pattern> patterns = new List<Pattern>();

        public AOCD19()
        {
            init();
        }

        public void solve1()
        {
            int result = 0;
            Stopwatch timer = new Stopwatch();
            timer.Start();
            //Code
            result = possibleDesigns(patterns, towels);
            timer.Stop();
            Console.WriteLine("Final 1: [" + result + "] Finished in:[" + timer.ElapsedMilliseconds + "ms]");
        }

        public void solve2()
        {
            long result = 0;
            Stopwatch timer = new Stopwatch();
            timer.Start();
            //Code
            result = totalPossibleConfigs(patterns, towels);
            timer.Stop();
            Console.WriteLine("Final 2: [" + result + "] Finished in:[" + timer.ElapsedMilliseconds + "ms]");
        }

        private static int possibleDesigns(List<Pattern> patterns, List<string> towels)
        {
            int count = 0;
            foreach (Pattern pattern in patterns)
            {
                if (possibleToConfig(pattern.desiredPattern, towels))
                {
                    count++;
                }
            }
            return count;
        }

        // finds if it can find a valid configuration by iterating through the entire design at each 'i', and comparing if any pattern can match the constructed substring  
        private static bool possibleToConfig(string design, List<string> patterns)
        {
            bool[] dp = new bool[design.Length + 1];
            dp[0] = true;

            for (int i = 1; i <= design.Length; i++)
            {
                foreach (string pattern in patterns)
                {
                    if (i >= pattern.Length && design.Substring(i - pattern.Length, pattern.Length) == pattern)
                    {
                        dp[i] = dp[i] || dp[i - pattern.Length];
                    }
                }
            }
            return dp[design.Length];
        }

        private static long totalPossibleConfigs(List<Pattern> patterns, List<string> towels)
        {
            long count = 0;
            foreach (Pattern pattern in patterns)
            {
                count += possibleWaysToConfig(pattern.desiredPattern, towels);
            }
            return count;
        }

        // similar approach to part 1, but instead of returning a boolean value, it returns a long value where it counts how many times that there is a substring match
        private static long possibleWaysToConfig(string design, List<string> patterns)
        {
            long[] dp = new long[design.Length + 1];
            dp[0] = 1;

            for (int i = 1; i <= design.Length; i++)
            {
                foreach (string pattern in patterns)
                {
                    if (i >= pattern.Length && design.Substring(i - pattern.Length, pattern.Length) == pattern)
                    {
                        dp[i] += dp[i - pattern.Length];
                    }
                }
            }
            return dp[design.Length];
        }

        internal class Pattern{
            public string desiredPattern {  get; set; }

            public Pattern(string input)
            {
                desiredPattern = input;
            }

            public override string ToString()
            {
                string val = "Pattern:[" + desiredPattern + "]"; 
                return val;
            }
        }

        private void printList(List<string> strings)
        {
            Console.WriteLine("Printing List --- ");
            foreach (string s in strings)
            {
                Console.WriteLine("Towel: [" + s + "]");
            }
        }

        private void printList(List<Pattern> patterns)
        {
            Console.WriteLine("Printing List --- ");
            foreach (Pattern p in patterns)
            {
                Console.WriteLine(p);
            }
        }

        private List<Pattern> makeCopy(List<Pattern> input)
        {
            List<Pattern> copy = new List<Pattern>();
            foreach(Pattern i in input)
            {
                copy.Add(new Pattern(i.desiredPattern));
            }
            return copy;
        }

        private void init()
        {
            string line;
            int count = 0;
            bool nextSect = false;
            try
            {
                StreamReader sr = new StreamReader(inputFile);
                line = sr.ReadLine();
                while (line != null)
                {
                    count++;
                    //VVV Do file reading operations here VVV
                    if(line.Length < 2)
                    {
                        nextSect = true;
                        line = sr.ReadLine();
                        continue;
                    }

                    if (!nextSect)
                    {
                        string[] split = line.Split(',');
                        foreach(string s in split)
                        {
                            towels.Add(s.Trim());
                        }
                    }
                    else
                    {
                        Pattern pat = new Pattern(line);
                        patterns.Add(pat);
                    }
                    line = sr.ReadLine();
                }
                Console.WriteLine("| " + count + " lines read");
                sr.Close();
                printList(towels);
                printList(patterns);
                Console.WriteLine("| AOC Day 19 Resources Initialized! ");
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex2)
            {
                Console.WriteLine(ex2.Message);
            }
        }
    }
}
