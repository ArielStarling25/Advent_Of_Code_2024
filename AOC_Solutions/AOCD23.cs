using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC_2024_Day1.AOC_Solutions
{
    public class AOCD23
    {
        private static readonly string inputFile = "C:\\Users\\user\\Desktop\\Ariel's Folder\\Training\\AdventOfCode_2024\\AOC_2024_Day1\\AOC_Resources\\AOCD23_Example.txt";

        private static Dictionary<string, List<string>> globalComp = new Dictionary<string, List<string>>();

        public AOCD23()
        {
            init();
            foreach(KeyValuePair<string, List<string>> item in globalComp)
            {
                Console.WriteLine("-----------");
                Console.WriteLine("Key: " + item.Key);
                foreach(string val in item.Value)
                {
                    Console.WriteLine("Values: " + val);
                }
            }
        }

        public void solve1()
        {
            int result = 0;
            Stopwatch timer = new Stopwatch();
            timer.Start();
            //Code
            result = setFinder("t");
            timer.Stop();
            Console.WriteLine("Final 1: [" + result + "] Finished in:[" + timer.ElapsedMilliseconds + "ms]");
        }

        public void solve2()
        {
            int result = 0;
            Stopwatch timer = new Stopwatch();
            timer.Start();
            //Code

            timer.Stop();
            Console.WriteLine("Final 2: [" + result + "] Finished in:[" + timer.ElapsedMilliseconds + "ms]");
        }

        public int setFinder(string containing)
        {
            List<(string, string, string)> lanParties = new List<(string, string, string)>();
            foreach(KeyValuePair<string, List<string>> comps1 in globalComp)
            {
                string comp1 = comps1.Key;
                foreach(KeyValuePair<string, List<string>> comps2 in globalComp)
                {
                    string comp2 = comps2.Key;
                    if (comp1 == comp2) continue;
                    foreach(KeyValuePair<string, List<string>> comps3 in globalComp)
                    {
                        string comp3 = comps3.Key;
                        if (comp2 == comp3) continue;
                        if (comp1 == comp2 && comp2 == comp3) continue;
                        if (globalComp[comp1].Contains(comp2) && globalComp[comp1].Contains(comp3) && globalComp[comp2].Contains(comp1) && globalComp[comp2].Contains(comp3) && globalComp[comp3].Contains(comp1) && globalComp[comp3].Contains(comp2))
                        {
                            if(!lanParties.Contains((comp1, comp2, comp3)))
                            {
                                lanParties.Add((comp1, comp2, comp3));
                                lanParties.Add((comp2, comp3, comp1));
                                lanParties.Add((comp3, comp1, comp2));

                                lanParties.Add((comp1, comp3, comp2));
                                lanParties.Add((comp2, comp1, comp3));
                                lanParties.Add((comp3, comp2, comp1));
                            }
                        }
                    }
                }
            }
            List<(string, string, string)> confLanParties = new List<(string, string, string)>();
            for(int i = 0; i < lanParties.Count; i += 6) // removing duplicates
            {
                confLanParties.Add(lanParties[i]);
            }
            Console.WriteLine("Lan Parties");

            int res = 0;
            foreach ((string, string, string) item in confLanParties)
            {
                if(item.Item1.StartsWith(containing) || item.Item2.StartsWith(containing) || item.Item3.StartsWith(containing))
                {
                    res++;
                }
                Console.WriteLine(item);
            }
            Console.WriteLine("Count: " + confLanParties.Count);

            return res;
        }

        private void largestSetFinder()
        {
            List<List<string>> sets = new List<List<string>>();
            foreach(KeyValuePair<string, List<string>> computer in globalComp)
            {
                
            }
        }

        private void init()
        {
            string line;
            int count = 0;
            try
            {
                StreamReader sr = new StreamReader(inputFile);
                line = sr.ReadLine();
                while (line != null)
                {
                    count++;
                    //VVV Do file reading operations here VVV
                    string[] split = line.Split(new char[] {'-'});
                    if(split.Length > 1)
                    {
                        if (globalComp.TryAdd(split[0], null))
                        {
                            globalComp[split[0]] = new List<string>();
                        }
                        if (globalComp.TryAdd(split[1], null))
                        {
                            globalComp[split[1]] = new List<string>();
                        }

                        globalComp[split[0]].Add(split[1]);
                        globalComp[split[1]].Add(split[0]);
                    }
                    line = sr.ReadLine();
                }
                Console.WriteLine("| " + count + " lines read");
                sr.Close();
                Console.WriteLine("| AOC Day 23 Resources Initialized! ");
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
