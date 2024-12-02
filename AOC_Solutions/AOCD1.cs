using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace AOC_2024_Day1.AOC_Solutions
{
    public class AOCD1
    {
        List<int> leftList { get; set; }
        List<int> rightList { get; set; }
        private static readonly string inputFile = "C:\\Users\\user\\Desktop\\Ariel's Folder\\Training\\AdventOfCode_2024\\AOC_2024_Day1\\AOC_Resources\\AOCD1.txt";

        public AOCD1()
        {
            leftList = new List<int>();
            rightList = new List<int>();
            init();
        }

        public void solve1()
        {
            int sum = 0;
            Console.WriteLine("Solving");
            leftList.Sort();
            rightList.Sort();
            for (int i = 0; i < leftList.Count; i++)
            {
                int diff = leftList[i] - rightList[i];
                diff = Math.Abs(diff);
                sum += diff;
            }
            Console.WriteLine("Final: " + sum);
        }

        public void solve2()
        {
            Console.WriteLine("Solving 2");
            List<int> resList = new List<int>();
            for (int i = 0; i < leftList.Count; i++)
            {
                int similarityCount = 0;
                for (int j = 0; j < rightList.Count; j++)
                {
                    if (rightList[j] == leftList[i])
                    {
                        similarityCount++;
                    }
                }
                resList.Add(leftList[i] * similarityCount);
            }
            Console.WriteLine("Solve 2");
            printList(resList);
            Console.WriteLine("Final: " + resList.Sum());
        }

        private void printList(List<int> list)
        {
            foreach (int item in list) Console.WriteLine(item);
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
                    //List<int> temp = new List<int>();
                    count++;
                    string[] split = line.Split("   ");
                    leftList.Add(int.Parse(split[0]));
                    rightList.Add(int.Parse(split[1]));
                    Console.WriteLine("LeftList In:[" + split[0] + "] | rightList In:[" + split[1] + "]");
                    line = sr.ReadLine();
                }
                Console.WriteLine("| " + count + " pairs read");
                sr.Close();
                Console.WriteLine("| AOC Day 1 Resources Initialized! ");
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
