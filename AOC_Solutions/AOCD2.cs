using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC_2024_Day1.AOC_Solutions
{
    public class AOCD2
    {
        private static readonly string inputFile = "C:\\Users\\user\\Desktop\\Ariel's Folder\\Training\\AdventOfCode_2024\\AOC_2024_Day1\\AOC_Resources\\Edisons_AOCD2.txt";
        private List<List<int>> fullList = new List<List<int>>();

        public AOCD2()
        {
            init();
        }

        public void solve1()
        {
            int safeCount = 0;
            Console.WriteLine("Solving 1");
            for(int i = 0; i < fullList.Count; i++)
            {
                if (isSafe(fullList[i]))
                {
                    safeCount++;
                    Console.WriteLine("[" + printReport(fullList[i]) + "] is Safe!");
                }
                else
                {
                    Console.WriteLine("[" + printReport(fullList[i]) + "] is Unsafe!");
                }
            }
            Console.WriteLine("Final: " + safeCount);
        }

        public void solve2()
        {
            int safeCount = 0;
            Console.WriteLine("Solving 2");
            for (int i = 0; i < fullList.Count; i++)
            {
                if (isSafe(fullList[i]))
                {
                    safeCount++;
                    Console.WriteLine("[" + printReport(fullList[i]) + "] is Safe!");
                }
                else
                {
                    if (problemDampener(fullList[i]))
                    {
                        safeCount++;
                    }
                    else
                    {
                        Console.WriteLine("[" + printReport(fullList[i]) + "] is Unsafe no matter where you remove a number!");
                    }
                }
            }
            Console.WriteLine("Final: " + safeCount);
        }

        private bool isSafe(List<int> report)
        {
            bool isConsistent = true;
            bool isDecrease = false;
            if (report[0] > report[1])
            {
                isDecrease = true;
            }
            else
            {
                isDecrease = false;
            }

            for (int j = 0; j < (report.Count - 1); j++)
            {
                if (isDecrease) //decreasing
                {
                    if (report[j] > report[j + 1])
                    {
                        int diff = report[j] - report[j + 1];
                        if (diff < 1 || diff > 3)
                        {
                            isConsistent = false;
                        }
                    }
                    else
                    {
                        isConsistent = false;
                    }
                }
                else // increasing
                {
                    if (report[j] < report[j + 1])
                    {
                        int diff = report[j + 1] - report[j];
                        if (diff < 1 || diff > 3)
                        {
                            isConsistent = false;
                        }
                    }
                    else
                    {
                        isConsistent = false;
                    }
                }
            }
            return isConsistent;
        }

        private bool problemDampener(List<int> report)
        {
            bool result = false;
            for(int rem = 0; rem < report.Count; rem++)
            {
                List<int> dampenedList = new List<int>();
                for(int x = 0; x < report.Count; x++)
                {
                    if(x != rem)
                    {
                        dampenedList.Add(report[x]);
                    }
                }

                if (isSafe(dampenedList))
                {
                    Console.WriteLine("[" + printReport(report) + "] is safe after removing at index [" + rem + "]");
                    result = true;
                }

                if (result)
                {
                    return result;
                }
            }
            return result;
        }

        private void printList(List<List<int>> list)
        {
            foreach (List<int> item in list)
            {
                foreach(int item2 in item)
                {
                    Console.Write(" " + item2);
                }
                Console.WriteLine();
            }
        }

        private string printReport(List<int> list)
        {
            string ret = "";
            foreach (int item in list)
            {
                ret += (item + " ");
            }
            return ret;
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
                    string[] split = line.Split(" ");
                    List<int> tempList = new List<int>();
                    for(int i = 0; i < split.Length; i++)
                    {
                        tempList.Add(int.Parse(split[i]));
                    }
                    fullList.Add(tempList);
                    line = sr.ReadLine();
                }
                Console.WriteLine("| " + count + " lines read");
                sr.Close();
                Console.WriteLine("| AOC Day 2 Resources Initialized! ");
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
