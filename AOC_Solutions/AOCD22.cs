using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC_2024_Day1.AOC_Solutions
{
    public class AOCD22
    {
        private static readonly string inputFile = "C:\\Users\\user\\Desktop\\Ariel's Folder\\Training\\AdventOfCode_2024\\AOC_2024_Day1\\AOC_Resources\\AOCD22.txt";

        private static List<long> secrets = new List<long>();

        public AOCD22()
        {
            init();
            foreach (long i in secrets)
            {
                Console.WriteLine(i);
            }
            Console.WriteLine("---");
        }

        private long mix(long s, long r)
        {
            return s ^= r;
        }

        // ^ 16777216
        private long prune(long s)
        {
            return s & 0xFFFFFF;
        }

        // * 64
        // / 32
        // * 2048
        private long g(long s)
        {
            s=(s^(s<<6))&0xFFFFFF;
            s=(s^(s>>5))&0xFFFFFF;
            s=(s^(s<<11))&0xFFFFFF;
            return s;
        }

        public void solve1()
        {
            long result = 0;
            Stopwatch timer = new Stopwatch();
            timer.Start();
            foreach(long n in secrets)
            {
                long s=n;
                for(int i=0;i<2000;i++)
                {
                    s=(s^(s<<6))&0xFFFFFF;
                    s=(s^(s>>5))&0xFFFFFF;
                    s=(s^(s<<11))&0xFFFFFF;
                }
                result+=s;
            }
            timer.Stop();
            Console.WriteLine("Final 1: [" + result + "] Finished in:[" + timer.ElapsedMilliseconds + "ms]");
        }

        private long sequencingMonkey() // find the sequence for the first buyer and get the monkey's sequence from there
        {
            List<int> lastNums = new List<int>();
            List<int> s = new List<int>();
            long number = secrets[0];
            for (int i = 0; i < 2000; i++)
            {
                number = g(number);
                int lastDigit = (int)number % 10;
                lastNums.Add(lastDigit);
            }
            for (int i = 0; i < lastNums.Count - 1; i++)
            {
                s.Add(lastNums[i + 1] - lastNums[i]);
            }

            long storedHighest = 0;
            //(int, int, int, int) storedBestSequence = (0, 0, 0, 0);
            // Dictionary to store cached results for each sequence
            Dictionary<(int, int, int, int), long> sequenceCache = new Dictionary<(int, int, int, int), long>();
            List<List<int>> lastNumsSeq = new List<List<int>>();
            foreach(long num in secrets)
            {
                long number2 = num;
                List<int> lastNums2 = new List<int>();
                for (int i = 0; i < 2000; i++)
                {
                    number2 = g(number2);
                    int lastDigit = (int)number2 % 10;
                    lastNums2.Add(lastDigit);
                }
                lastNumsSeq.Add(lastNums2);
            }
            for(int i = 4; i < s.Count; i++)
            {
                //(int, int, int, int) monkeySequence = (sequence[i-3], sequence[i-2], sequence[i-1], sequence[i]);
                if (sequenceCache.ContainsKey((s[i - 3], s[i - 2], s[i - 1], s[i])))
                {
                    long cachedResult = sequenceCache[(s[i - 3], s[i - 2], s[i - 1], s[i])];
                    if (storedHighest <= cachedResult) storedHighest = cachedResult;
                    //Console.WriteLine("Sequence: " + (sequence[i - 3], sequence[i - 2], sequence[i - 1], sequence[i]) + " Result (Cached): " + cachedResult + " | Stored Highest: " + storedHighest + " with Sequence: " + storedBestSequence);
                    continue; // Skip recalculating if already cached
                }
                long res = monkeyBuyer((s[i - 3], s[i - 2], s[i - 1], s[i]), lastNumsSeq);
                sequenceCache[(s[i - 3], s[i - 2], s[i - 1], s[i])] = res; // Cache the result
                if(storedHighest <= res) storedHighest = res;
                //Console.WriteLine("Sequence: " + (s[i - 3], s[i - 2], s[i - 1], s[i]) + " Result: " + res + " | Stored Highest: " + storedHighest + " with Sequence: " /* +storedBestSequence*/);
            }
            return storedHighest;
        }

        private long monkeyBuyer((int, int, int, int) monkeySequence, List<List<int>> lastNumSeq)
        {
            long result = 0;
            foreach (List<int> lastNums in lastNumSeq)
            {
                for(int i = 0; i < lastNums.Count-4; i++)
                {
                    if (lastNums[i+1]-lastNums[i]==monkeySequence.Item1&& 
                        lastNums[i+2]-lastNums[i+1]==monkeySequence.Item2&&
                        lastNums[i+3]-lastNums[i+2]==monkeySequence.Item3&&
                        lastNums[i+4]-lastNums[i+3]==monkeySequence.Item4)
                    {
                        result += lastNums[i + 4];
                        break;
                    }
                }
            }
            return result;
        }

        public void solve2()
        {
            long result = 0;
            Stopwatch timer = new Stopwatch();
            timer.Start();
            //Code
            result = sequencingMonkey();
            timer.Stop();
            Console.WriteLine("Final 2: [" + result + "] Finished in:[" + timer.ElapsedMilliseconds + "ms]");
        }

        private long genSecNum2(long secNum)
        {
            return (((((secNum ^ (secNum << 6)) % 16777216) ^ (((secNum ^ (secNum << 6)) % 16777216) >> 5)) % 16777216) ^ (((((secNum ^ (secNum << 6)) % 16777216) ^ (((secNum ^ (secNum << 6)) % 16777216) >> 5)) % 16777216) << 11)) % 16777216;
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
                    secrets.Add(int.Parse(line)); 
                    line = sr.ReadLine();
                }
                Console.WriteLine("| " + count + " lines read");
                sr.Close();
                Console.WriteLine("| AOC Day {DayNum} Resources Initialized! ");
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
