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

        public void solve1()
        {
            long result = 0;
            Stopwatch timer = new Stopwatch();
            timer.Start();
            //Code
            foreach(long num in secrets)
            {
                long number = num;
                for(int i = 0; i < 2000; i++)
                {
                    number = genSecNum(number);
                }
                Console.WriteLine(num + ": " + number);
                result += number;
            }
            timer.Stop();
            Console.WriteLine("Final 1: [" + result + "] Finished in:[" + timer.ElapsedMilliseconds + "ms]");
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

        private long sequencingMonkey() // find the sequence for the first buyer and get the monkey's sequence from there
        {
            List<int> lastNums = new List<int>();
            List<int> sequence = new List<int>();
            long number = secrets[0];
            for (int i = 0; i < 2000; i++)
            {
                number = genSecNum(number);
                int lastDigit = (int)number % 10;
                lastNums.Add(lastDigit);
            }
            for (int i = 0; i < lastNums.Count - 1; i++)
            {
                sequence.Add(lastNums[i + 1] - lastNums[i]);
                //Console.WriteLine("LastNum: " + lastNums[i + 1] + "|Diff: " + (lastNums[i + 1] - lastNums[i]));
            }

            long storedHighest = 0;
            (int, int, int, int) storedBestSequence = (0, 0, 0, 0);
            for(int i = 4; i < sequence.Count; i++)
            {
                (int, int, int, int) monkeySequence = (sequence[i-3], sequence[i-2], sequence[i-1], sequence[i]);
                long res = monkeyBuyer(monkeySequence);
                if(storedHighest <= res)
                {
                    storedHighest = res;
                    storedBestSequence = monkeySequence;
                }
            }
            Console.WriteLine("Stored Highest: " + storedHighest);
            Console.WriteLine("Best Sequence: " + storedBestSequence);

            return storedHighest;
        }

        private long monkeyBuyer((int, int, int, int) monkeySequence)
        {
            long result = 0;
            foreach (long num in secrets)
            {
                List<int> lastNums = new List<int>();
                List<int> sequence = new List<int>();
                long number = num;
                for (int i = 0; i < 2000; i++)
                {
                    number = genSecNum(number);
                    int lastDigit = (int)number % 10;
                    lastNums.Add(lastDigit);
                }
                for(int i = 0; i < lastNums.Count-1; i++)
                {
                    sequence.Add(lastNums[i+1] - lastNums[i]);
                    //Console.WriteLine("LastNum: " + lastNums[i+1] + "|Diff: " + (lastNums[i+1] - lastNums[i]));
                }

                for(int i = 3; i < sequence.Count; i++)
                {
                    if (sequence[i-3] == monkeySequence.Item1 && sequence[i-2] == monkeySequence.Item2 && sequence[i-1] == monkeySequence.Item3 && sequence[i] == monkeySequence.Item4)
                    {
                        //Console.WriteLine("Adding: " + lastNums[i + 1]);
                        result += lastNums[i + 1];
                        break;
                    }
                }
            }
            //Console.WriteLine("Result: " + result);
            return result;
        }

        private long genSecNum(long oldSecNum)
        {
            long secNum = oldSecNum;
            secNum = prune(mix(secNum, secNum * 64));
            secNum = prune(mix(secNum, secNum / 32));
            secNum = prune(mix(secNum, secNum * 2048));
            return secNum;
        }

        private long mix(long secNum, long resultant)
        {
            return secNum ^= resultant;
        }

        private long prune(long secNum)
        {
            return secNum % 16777216;
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
