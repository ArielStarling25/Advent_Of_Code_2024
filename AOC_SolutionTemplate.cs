using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC_2024_Day1
{
    public class AOC_SolutionTemplate
    {
        private static readonly string inputFile = "C:\\Users\\user\\Desktop\\Ariel's Folder\\Training\\AdventOfCode_2024\\AOC_2024_Day1\\AOC_Resources\\AOCD5.txt";

        public AOC_SolutionTemplate()
        {
            init();
        }

        public void solve1()
        {
            int result = 0;
            Stopwatch timer = new Stopwatch();
            timer.Start();
            //Code

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
