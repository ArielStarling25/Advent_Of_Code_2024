using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC_2024_Day1.AOC_Solutions
{
    public class AOCD11
    {
        private static readonly string inputFile = "C:\\Users\\user\\Desktop\\Ariel's Folder\\Training\\AdventOfCode_2024\\AOC_2024_Day1\\AOC_Resources\\AOCD11.txt";

        private static List<long> stoneArrangement = new List<long>();

        private static readonly int BLINKS = 25;
        private static readonly int EXTBLINKS = 75;
        private static readonly int HAHABLINKS = 100;

        public AOCD11()
        {
            init();
        }

        public void solve1()
        {            int result = 0;
            Stopwatch timer = new Stopwatch();
            timer.Start();
            List<long> copy = makeCopy(stoneArrangement);
            printStones(copy);
            for (int i = 0; i < BLINKS; i++)
            {
                copy = blink(copy);
                //Console.WriteLine(i + "|Curr Len:[" + copy.Count + "]|At:[" + timer.ElapsedMilliseconds + "ms]");
            }
            timer.Stop();
            result = copy.Count;
            Console.WriteLine("Final 1: [" + result + "] Finished in:[" + timer.ElapsedMilliseconds + "ms]");
        }

        public void solve2()
        {
            int result = 0;
            Stopwatch timer = new Stopwatch();
            timer.Start();
            List<long> copy = makeCopy(stoneArrangement);
            printStones(copy);
            for (int i = 0; i < EXTBLINKS; i++)
            {
                copy = blink(copy);
                Console.WriteLine(i + "|Curr Len:[" + copy.Count + "]|At:[" + timer.ElapsedMilliseconds + "ms]");
            }
            timer.Stop();
            result = copy.Count;
            Console.WriteLine("Final 2: [" + result + "] Finished in:[" + timer.ElapsedMilliseconds + "ms]");
        }

        public void solve2Optimised()
        {
            long result = 0;
            Stopwatch timer = new Stopwatch();
            timer.Start();
            List<long> copy = makeCopy(stoneArrangement);
            for(int i = 0; i < copy.Count; i++)
            {
                result += blinker2(75, copy[i]);
                Console.WriteLine("Progress:[" + (i+1) + "/" + copy.Count + "|Result:[" + result + "]");
            }
            timer.Stop();
            Console.WriteLine("Final 2: [" + result + "] Finished in:[" + timer.ElapsedMilliseconds + "ms]");
        }

        public void solve2Blazing()
        {
            long result = 0;
            Stopwatch timer = new Stopwatch();
            timer.Start();
            Dictionary<long, long> rec = makeDict(stoneArrangement);
            for(int i = 0; i < EXTBLINKS; i++)
            {   
                Dictionary<long, long> updatedRec = new Dictionary<long, long>();
                foreach(var (key, value) in rec)
                {
                    //blink
                    updatedRec = blink(updatedRec, key, value);
                }
                rec = updatedRec;
            }
            result = (long)rec.Sum(key => key.Value);
            timer.Stop();
            Console.WriteLine("Final 2: [" + result + "] Finished in:[" + timer.ElapsedMilliseconds + "ms]");
        }

        private long blinker(int numOfBlinks, List<long> inputList) // for optimised approach, recursive
        {
            List<List<long>> stones = new List<List<long>>();
            long output = 0;

            if(numOfBlinks > 0)
            {
                for (int i = 0; i < inputList.Count; i++)
                {
                    List<long> indivStones = new List<long>();
                    indivStones.Add(inputList[i]);
                    stones.Add(indivStones);
                }
                int blinkCount = numOfBlinks - 1;   
                for (int i = 0; i < stones.Count; i++)
                {
                    output += blinker(blinkCount, blink(stones[i]));
                }
                return output;
            }
            else
            {
                return inputList.Count;
            }
        }

        private int blinker2(int num, long stone) // Max Optimization
        {
            if (num > 0)
            {
                if (stone == 0)
                {
                    return blinker2(num - 1, 1);
                }
                else if (stone.ToString().Length % 2 == 0)
                {
                    return blinker2(num - 1, long.Parse(stone.ToString().Substring(0, stone.ToString().Length/2))) + blinker2(num - 1, long.Parse(stone.ToString().Substring(stone.ToString().Length / 2)));
                }
                else
                {
                    return blinker2(num - 1, stone * 2024);
                }
            }
            else
            {
                return 1;
            }
        }

        public List<long> blink(List<long> input)
        {
            List<long> stones = makeCopy(input);
            for(int i = 0; i < stones.Count; i++)
            {
                if (stones[i] == 0)
                {
                    stones[i] = 1;
                }
                else if (isEven(countDigits(stones[i])))
                {
                    string convToStr = stones[i].ToString();
                    string half1 = convToStr.Substring(0, convToStr.Length / 2);
                    string half2 = convToStr.Substring(convToStr.Length / 2);
                    //Console.WriteLine("HALF1:" + half1 + "|HALF2:" + half2);
                    stones.RemoveAt(i);
                    stones.Insert(i, long.Parse(half2));
                    stones.Insert(i, long.Parse(half1));
                    i++;
                }
                else
                {
                    stones[i] = stones[i] * 2024;
                }
            }
            return stones;
        }

        private Dictionary<long, long> blink(Dictionary<long, long> newRecord, long key, long value) // using Dictionary
        {
            if(key == 0)
            {
                newRecord = updateRecord(newRecord, 1, value);
            }
            else if(countDigits(key) % 2 == 0)
            {
                newRecord = updateRecord(newRecord, long.Parse(key.ToString().Substring(0, key.ToString().Length / 2)), value);
                newRecord = updateRecord(newRecord, long.Parse(key.ToString().Substring(key.ToString().Length / 2)), value);
            }
            else
            {
                newRecord = updateRecord(newRecord, key * (long)2024, value);
            }
            return newRecord;
        }

        private Dictionary<long, long> updateRecord(Dictionary<long, long> record, long key, long value)
        {
            if (record.ContainsKey(key))
            {
                record[key] += value;
            }
            else
            {
                record.Add(key, value);
            }
            return record;
        }

        private int countDigits(long input)
        {
            string str = input.ToString();
            return str.Length;
        }

        private bool isEven(int input)
        {
            return (input % 2 == 0);
        }

        private List<long> makeCopy(List<long> stones)
        {
            List<long> newcopy = new List<long>();
            foreach(long stone in stones)
            {
                newcopy.Add(stone);
            }
            return newcopy;
        }

        private Dictionary<long, long> makeDict(List<long> stones)
        {
            Dictionary<long, long> stoneRecord = new Dictionary<long, long>();
            foreach(long stone in stones)
            {
                stoneRecord.Add(stone, 1);
            }
            return stoneRecord;
        }

        private List<long> subList(List<long> stones, int from, int to) // from and to are inclusive indexes
        {
            List<long> newcopy = new List<long>();
            if(from < to && to < stones.Count)
            {
                for (int i = from; i <= to; i++)
                {
                    newcopy.Add(stones[i]);
                }
            }
            return newcopy;
        }

        public void printStones(List<long> stones)
        {
            Console.WriteLine("--------");
            foreach(long st in stones)
            {
                Console.Write(st + " ");
            }
            Console.WriteLine();
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
                    string[] strs = line.Split(' ');
                    foreach (string str in strs)
                    {
                        stoneArrangement.Add(long.Parse(str));
                    }
                    line = sr.ReadLine();
                }
                Console.WriteLine("| " + count + " lines read");
                printStones(stoneArrangement);
                sr.Close();
                Console.WriteLine("| AOC Day 11 Resources Initialized! ");
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
